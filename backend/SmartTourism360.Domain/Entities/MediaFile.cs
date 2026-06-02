using System;
using SmartTourism360.Domain.Common;

namespace SmartTourism360.Domain.Entities
{
    public class MediaFile : BaseEntity
    {
        public string? OwnerType { get; set; }
        public Guid? OwnerId { get; set; }
        public string FileName { get; set; } = null!;
        public string StoredFileName { get; set; } = null!;
        public string FileUrl { get; set; } = null!;
        public string StoragePath { get; set; } = null!;
        public string StorageProvider { get; set; } = "minio"; // minio/s3/r2/local
        public string MediaType { get; set; } = null!; // image/panorama/video/audio/model3d/...
        public string MimeType { get; set; } = null!;
        public string Extension { get; set; } = null!;
        public long FileSize { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public int? Duration { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string? AltText { get; set; }
        public string? Caption { get; set; }
        public string? MetadataJson { get; set; } // Hỗ trợ lưu trữ meta động
        public Guid? UploadedBy { get; set; }
    }
}
