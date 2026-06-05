using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartTourism360.Application.Common.Models;
using SmartTourism360.Application.DTOs.Media;
using SmartTourism360.Application.Interfaces;
using SmartTourism360.Domain.Entities;
using SmartTourism360.Infrastructure.Data;

namespace SmartTourism360.Infrastructure.Services
{
    public class MediaService : IMediaService
    {
        private readonly SmartTourism360DbContext _context;
        private readonly IStorageService _storageService;

        public MediaService(SmartTourism360DbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task<ApiResponse<MediaFileDto>> UploadAsync(Stream fileStream, string fileName, string contentType, string? mediaType, string? caption, string? altText, Guid userId)
        {
            var extension = Path.GetExtension(fileName).ToLower();
            
            // 1. Phân loại mediaType nếu chưa được xác định rõ
            var detectedType = mediaType;
            if (string.IsNullOrWhiteSpace(detectedType))
            {
                detectedType = extension switch
                {
                    ".jpg" or ".jpeg" or ".png" or ".webp" or ".gif" => "image",
                    ".mp4" => "video",
                    ".mp3" or ".wav" => "audio",
                    _ => "unknown"
                };
            }

            if (detectedType == "unknown")
            {
                return ApiResponse<MediaFileDto>.FailureResponse("Định dạng tập tin tải lên không được hỗ trợ.");
            }

            // 2. Validate Kích thước tối đa
            long maxBytes = detectedType switch
            {
                "image" => 5 * 1024 * 1024,
                "panorama" => 15 * 1024 * 1024,
                "video" => 50 * 1024 * 1024,
                "audio" => 10 * 1024 * 1024,
                _ => 5 * 1024 * 1024
            };

            if (fileStream.Length > maxBytes)
            {
                var limitMb = maxBytes / (1024 * 1024);
                return ApiResponse<MediaFileDto>.FailureResponse($"Dung lượng tập tin vượt quá giới hạn cho phép của {detectedType} ({limitMb}MB).");
            }

            // 3. Đọc kích thước ảnh (Width/Height) nếu là Image/Panorama để tối ưu hiển thị
            int? width = null;
            int? height = null;
            if (detectedType == "image" || detectedType == "panorama")
            {
                var dims = GetImageDimensions(fileStream, extension);
                width = dims.width;
                height = dims.height;
                
                // Reset stream position if it was seeked
                if (fileStream.CanSeek)
                {
                    fileStream.Position = 0;
                }
            }

            // 4. Upload lên MinIO Storage
            string storagePath = await _storageService.UploadFileAsync(fileStream, fileName, contentType, detectedType);
            var fileUrl = _storageService.GetPublicUrl(storagePath);
            var storedName = Path.GetFileName(storagePath);

            // 5. Lưu thông tin Metadata vào PostgreSQL Database
            var media = new MediaFile
            {
                FileName = fileName,
                StoredFileName = storedName,
                FileUrl = fileUrl,
                StoragePath = storagePath,
                StorageProvider = "minio",
                MediaType = detectedType,
                MimeType = contentType,
                Extension = extension,
                FileSize = fileStream.Length,
                Width = width,
                Height = height,
                Caption = caption,
                AltText = altText,
                UploadedBy = userId,
                CreatedAt = DateTime.UtcNow
            };

            await _context.MediaFiles.AddAsync(media);
            await _context.SaveChangesAsync();

            return ApiResponse<MediaFileDto>.SuccessResponse(MapToDto(media), "Tải lên tập tin media thành công.");
        }

        public async Task<ApiResponse<PagedResult<MediaFileDto>>> GetMediaFilesAsync(string? mediaType, int page, int pageSize)
        {
            var query = _context.MediaFiles.AsQueryable();

            if (!string.IsNullOrWhiteSpace(mediaType))
            {
                query = query.Where(m => m.MediaType == mediaType);
            }

            var totalItems = await query.CountAsync();
            var items = await query
                .OrderByDescending(m => m.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtos = items.Select(MapToDto).ToList();

            var result = new PagedResult<MediaFileDto>
            {
                Items = dtos,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };

            return ApiResponse<PagedResult<MediaFileDto>>.SuccessResponse(result, "Lấy danh sách tập tin thành công.");
        }

        public async Task<ApiResponse<MediaFileDto>> GetByIdAsync(Guid id)
        {
            var media = await _context.MediaFiles.FindAsync(id);
            if (media == null)
            {
                return ApiResponse<MediaFileDto>.FailureResponse("Không tìm thấy tập tin media.");
            }

            return ApiResponse<MediaFileDto>.SuccessResponse(MapToDto(media), "Lấy chi tiết tập tin thành công.");
        }

        public async Task<ApiResponse<MediaFileDto>> UpdateAsync(Guid id, UpdateMediaRequest request)
        {
            var media = await _context.MediaFiles.FindAsync(id);
            if (media == null)
            {
                return ApiResponse<MediaFileDto>.FailureResponse("Không tìm thấy tập tin media để cập nhật.");
            }

            media.Caption = request.Caption;
            media.AltText = request.AltText;
            media.UpdatedAt = DateTime.UtcNow;

            _context.MediaFiles.Update(media);
            await _context.SaveChangesAsync();

            return ApiResponse<MediaFileDto>.SuccessResponse(MapToDto(media), "Cập nhật chú thích tập tin thành công.");
        }

        public async Task<ApiResponse<bool>> DeleteAsync(Guid id)
        {
            var media = await _context.MediaFiles.FindAsync(id);
            if (media == null)
            {
                return ApiResponse<bool>.FailureResponse("Không tìm thấy tập tin media để xóa.");
            }

            // 1. Kiểm tra ràng buộc xem có địa điểm nào đang dùng làm cover hay không
            var isCoverForDest = await _context.Destinations.AnyAsync(d => d.CoverImageId == id);
            var isThumbForTour = await _context.VirtualTours.AnyAsync(t => t.ThumbnailId == id);
            var isImgForPano = await _context.Panoramas.AnyAsync(p => p.PanoramaImageId == id || p.ThumbnailId == id);
            
            if (isCoverForDest || isThumbForTour || isImgForPano)
            {
                return ApiResponse<bool>.FailureResponse("Tập tin đang được sử dụng làm ảnh bìa địa điểm, ảnh tour hoặc ảnh toàn cảnh và không thể xóa.");
            }

            // 2. Xóa trên MinIO
            await _storageService.DeleteFileAsync(media.StoragePath);

            // 3. Xóa trong Database
            _context.MediaFiles.Remove(media);
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Xóa tập tin media thành công.");
        }

        private static MediaFileDto MapToDto(MediaFile m)
        {
            return new MediaFileDto
            {
                Id = m.Id,
                FileName = m.FileName,
                StoredFileName = m.StoredFileName,
                FileUrl = m.FileUrl,
                StoragePath = m.StoragePath,
                MediaType = m.MediaType,
                MimeType = m.MimeType,
                Extension = m.Extension,
                FileSize = m.FileSize,
                Width = m.Width,
                Height = m.Height,
                Duration = m.Duration,
                ThumbnailUrl = m.ThumbnailUrl,
                AltText = m.AltText,
                Caption = m.Caption,
                CreatedAt = m.CreatedAt
            };
        }

        // Đọc chiều rộng/chiều cao ảnh trực tiếp từ byte stream không dùng thư viện ngoài
        private static (int? width, int? height) GetImageDimensions(Stream stream, string extension)
        {
            try
            {
                if (extension == ".png")
                {
                    byte[] buffer = new byte[24];
                    stream.Position = 0;
                    stream.ReadExactly(buffer, 0, 24);
                    if (buffer[0] == 0x89 && buffer[1] == 0x50 && buffer[2] == 0x4E && buffer[3] == 0x47) 
                    {
                        int width = (buffer[16] << 24) | (buffer[17] << 16) | (buffer[18] << 8) | buffer[19];
                        int height = (buffer[20] << 24) | (buffer[21] << 16) | (buffer[22] << 8) | buffer[23];
                        return (width, height);
                    }
                }
                else if (extension == ".jpg" || extension == ".jpeg")
                {
                    stream.Position = 0;
                    int b1 = stream.ReadByte();
                    int b2 = stream.ReadByte();
                    if (b1 == 0xFF && b2 == 0xD8) 
                    {
                        while (true)
                        {
                            int marker = stream.ReadByte();
                            if (marker == -1) break;
                            if (marker != 0xFF) continue;
                            
                            while (marker == 0xFF) { marker = stream.ReadByte(); }

                            if (marker >= 0xC0 && marker <= 0xC3) 
                            {
                                int len = (stream.ReadByte() << 8) | stream.ReadByte();
                                stream.ReadByte(); // precision
                                int height = (stream.ReadByte() << 8) | stream.ReadByte();
                                int width = (stream.ReadByte() << 8) | stream.ReadByte();
                                return (width, height);
                            }
                            else if (marker == 0xD9 || marker == 0xDA) 
                            {
                                break;
                            }
                            else
                            {
                                int len = (stream.ReadByte() << 8) | stream.ReadByte();
                                stream.Position += (len - 2);
                            }
                        }
                    }
                }
            }
            catch
            {
                // Trả về null nếu xảy ra lỗi đọc
            }
            return (null, null);
        }
    }
}
