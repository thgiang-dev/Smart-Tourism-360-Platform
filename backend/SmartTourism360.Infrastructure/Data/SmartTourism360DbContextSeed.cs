using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using SmartTourism360.Domain.Entities;
using SmartTourism360.Application.Interfaces;

namespace SmartTourism360.Infrastructure.Data
{
    public static class SmartTourism360DbContextSeed
    {
        public static async Task SeedAsync(SmartTourism360DbContext context, IStorageService? storageService = null)
        {
            // Đảm bảo Database đã được tạo và áp dụng migration
            await context.Database.MigrateAsync();

            // 1. Seed Roles
            if (!await context.Roles.AnyAsync())
            {
                var roles = new[]
                {
                    new Role { Name = "Super Admin", Code = "superadmin", Description = "Quản trị viên cấp cao nhất hệ thống" },
                    new Role { Name = "Admin", Code = "admin", Description = "Quản trị viên nội dung và địa điểm" }
                };

                await context.Roles.AddRangeAsync(roles);
                await context.SaveChangesAsync();
            }

            // 2. Seed Users
            if (!await context.Users.AnyAsync())
            {
                var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.Code == "admin");
                var superadminRole = await context.Roles.FirstOrDefaultAsync(r => r.Code == "superadmin");

                if (superadminRole != null)
                {
                    var superAdmin = new User
                    {
                        FullName = "Hệ Thống Super Admin",
                        Email = "superadmin@smarttourism360.vn",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                        RoleId = superadminRole.Id,
                        Status = "active"
                    };

                    await context.Users.AddAsync(superAdmin);
                }

                if (adminRole != null)
                {
                    var admin = new User
                    {
                        FullName = "Quản trị viên Demo",
                        Email = "admin@smarttourism360.vn",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                        RoleId = adminRole.Id,
                        Status = "active"
                    };

                    await context.Users.AddAsync(admin);
                }

                await context.SaveChangesAsync();
            }
            else
            {
                var usersToUpdate = await context.Users.ToListAsync();
                bool modified = false;
                foreach (var u in usersToUpdate)
                {
                    if (!u.PasswordHash.StartsWith("$2"))
                    {
                        u.PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123");
                        modified = true;
                    }
                }
                if (modified)
                {
                    await context.SaveChangesAsync();
                }
            }

            // 3. Seed Regions
            if (!await context.Regions.AnyAsync())
            {
                var region = new Region
                {
                    Name = "Cần Thơ Demo",
                    Slug = "can-tho-demo",
                    Description = "Vùng du lịch đô thị sông nước miền Tây Nam Bộ",
                    Province = "Cần Thơ",
                    CenterLatitude = 10.03711m,
                    CenterLongitude = 105.78825m,
                    DefaultZoom = 13,
                    Status = "published"
                };

                await context.Regions.AddAsync(region);
                await context.SaveChangesAsync();
            }

            // 4. Seed Categories
            if (!await context.Categories.AnyAsync())
            {
                var categories = new[]
                {
                    new Category { Name = "Văn hóa - Lịch sử", Slug = "van-hoa-lich-su", Description = "Đình, nhà cổ, di tích, bảo tàng", Icon = "landmark", Color = "#7C3AED", DisplayOrder = 1 },
                    new Category { Name = "Tâm linh", Slug = "tam-linh", Description = "Chùa, miếu, nhà thờ, tín ngưỡng", Icon = "building", Color = "#F59E0B", DisplayOrder = 2 },
                    new Category { Name = "Sinh thái", Slug = "sinh-thai", Description = "Khu sinh thái, sông nước, bến tàu", Icon = "tree-pine", Color = "#22C55E", DisplayOrder = 3 },
                    new Category { Name = "Nông nghiệp", Slug = "nong-nghiep", Description = "Vườn trái cây, trang trại trải nghiệm", Icon = "flower", Color = "#65A30D", DisplayOrder = 4 },
                    new Category { Name = "Làng nghề", Slug = "lang-nghe", Description = "Thủ công truyền thống, sản phẩm OCOP", Icon = "wrench", Color = "#C2410C", DisplayOrder = 5 },
                    new Category { Name = "Ẩm thực", Slug = "am-thuc", Description = "Quán ăn địa phương, đặc sản", Icon = "utensils", Color = "#EF4444", DisplayOrder = 6 },
                    new Category { Name = "Lưu trú", Slug = "luu-tru", Description = "Homestay, nhà vườn nghỉ dưỡng", Icon = "home", Color = "#2563EB", DisplayOrder = 7 }
                };

                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }

            // 5. Seed Destinations & Media (Idempotent)
            var regionCanTho = await context.Regions.FirstOrDefaultAsync(r => r.Slug == "can-tho-demo");
            var cultureCat = await context.Categories.FirstOrDefaultAsync(c => c.Slug == "van-hoa-lich-su");
            var spiritualCat = await context.Categories.FirstOrDefaultAsync(c => c.Slug == "tam-linh");
            var ecoCat = await context.Categories.FirstOrDefaultAsync(c => c.Slug == "sinh-thai");
            var agriCat = await context.Categories.FirstOrDefaultAsync(c => c.Slug == "nong-nghiep");

            if (regionCanTho != null && cultureCat != null && spiritualCat != null && ecoCat != null && agriCat != null)
            {
                // Danh sách 7 địa điểm mẫu
                var destSeeds = new[]
                {
                    new {
                        Slug = "chua-ong-can-tho",
                        Name = "Chùa Ông Cần Thơ",
                        CategoryId = spiritualCat.Id,
                        ShortDescription = "Ngôi chùa cổ mang đậm kiến trúc Trung Hoa tọa lạc ngay bến Ninh Kiều.",
                        FullDescription = "Chùa Ông Cần Thơ (tên gốc Quảng Triệu Hội Quán) là di tích lịch sử văn hóa cấp quốc gia tiêu biểu tại Cần Thơ, xây dựng từ năm 1894 với kiến trúc chữ Quốc bằng gốm sứ và gỗ tinh xảo.",
                        Address = "Số 32 đường Hai Bà Trưng, Tân An, Ninh Kiều, Cần Thơ",
                        Latitude = 10.0348704m,
                        Longitude = 105.7865246m,
                        CoverUrl = "https://images.unsplash.com/photo-1599707367072-cd6ada2bc375?auto=format&fit=crop&w=600&q=80"
                    },
                    new {
                        Slug = "nha-co-binh-thuy",
                        Name = "Nhà Cổ Bình Thủy",
                        CategoryId = cultureCat.Id,
                        ShortDescription = "Ngôi nhà cổ mang phong cách kiến trúc Pháp - Hoa đặc sắc trăm tuổi.",
                        FullDescription = "Nhà cổ Bình Thủy được xây dựng năm 1870 bởi dòng họ Dương, là một trong những ngôi nhà cổ đẹp nhất miền Tây với sự giao thoa kiến trúc Đông - Tây hoàn hảo.",
                        Address = "Số 26/1A đường Bùi Hữu Nghĩa, Bình Thủy, Cần Thơ",
                        Latitude = 10.0763267m,
                        Longitude = 105.7745778m,
                        CoverUrl = "https://images.unsplash.com/photo-1596422846543-75c6fc18a52b?auto=format&fit=crop&w=600&q=80"
                    },
                    new {
                        Slug = "cho-noi-cai-rang",
                        Name = "Chợ Nổi Cái Răng",
                        CategoryId = ecoCat.Id,
                        ShortDescription = "Nét văn hóa giao thương độc đáo trên sông nước miền Tây.",
                        FullDescription = "Chợ nổi Cái Răng là di sản văn hóa phi vật thể quốc gia, nơi giao thương nông sản tấp nập của khu vực Đồng bằng sông Cửu Long diễn ra trực tiếp trên những mạn thuyền từ sáng sớm.",
                        Address = "Sông Cái Răng, Cái Răng, Cần Thơ",
                        Latitude = 10.0055278m,
                        Longitude = 105.7423333m,
                        CoverUrl = "https://images.unsplash.com/photo-1528127269322-539801943592?auto=format&fit=crop&w=600&q=80"
                    },
                    new {
                        Slug = "cau-can-tho",
                        Name = "Cầu Cần Thơ",
                        CategoryId = cultureCat.Id,
                        ShortDescription = "Cây cầu dây văng có nhịp chính dài nhất Đông Nam Á khi hoàn thành năm 2010.",
                        FullDescription = "Cầu Cần Thơ bắc qua sông Hậu, nối liền thành phố Cần Thơ và tỉnh Vĩnh Long. Đây là biểu tượng phát triển giao thông và kiến trúc hiện đại của vùng ĐBSCL.",
                        Address = "Quốc lộ 1A, Hưng Phú, Cái Răng, Cần Thơ",
                        Latitude = 10.033621m,
                        Longitude = 105.807851m,
                        CoverUrl = "https://images.unsplash.com/photo-1545638870-12206753a76f?auto=format&fit=crop&w=600&q=80"
                    },
                    new {
                        Slug = "ben-ninh-kieu",
                        Name = "Bến Ninh Kiều",
                        CategoryId = ecoCat.Id,
                        ShortDescription = "Địa danh du lịch và văn hóa tiêu biểu, công viên bên bờ sông Hậu thơ mộng.",
                        FullDescription = "Bến Ninh Kiều nay là Công viên Ninh Kiều, hình thành từ thế kỷ 19, là điểm hẹn sông nước tuyệt vời ngắm nhìn cầu Cần Thơ và phố thị về đêm sầm uất.",
                        Address = "Đường Hai Bà Trưng, Tân An, Ninh Kiều, Cần Thơ",
                        Latitude = 10.033282m,
                        Longitude = 105.788544m,
                        CoverUrl = "https://images.unsplash.com/photo-1507525428034-b723cf961d3e?auto=format&fit=crop&w=600&q=80"
                    },
                    new {
                        Slug = "thien-vien-truc-lam-phuong-nam",
                        Name = "Thiền Viện Trúc Lâm Phương Nam",
                        CategoryId = spiritualCat.Id,
                        ShortDescription = "Thiền viện lớn nhất vùng Tây Nam Bộ mang đậm kiến trúc thời Lý - Trần.",
                        FullDescription = "Thiền viện Trúc Lâm Phương Nam tọa lạc trên diện tích 4ha, khánh thành năm 2014, là điểm du lịch tâm linh thanh tịnh với vườn cảnh quan, các gian thờ gỗ quý.",
                        Address = "Ấp Mỹ Nhơn, Mỹ Khánh, Phong Điền, Cần Thơ",
                        Latitude = 10.016335m,
                        Longitude = 105.698055m,
                        CoverUrl = "https://images.unsplash.com/photo-1540959733332-eab4deceeaf7?auto=format&fit=crop&w=600&q=80"
                    },
                    new {
                        Slug = "lang-du-lich-my-khanh",
                        Name = "Làng Du Lịch Mỹ Khánh",
                        CategoryId = agriCat.Id,
                        ShortDescription = "Tổ hợp du lịch sinh thái nông nghiệp, vườn trái cây và trò chơi dân gian miền Tây.",
                        FullDescription = "Làng du lịch Mỹ Khánh là điểm đến đậm chất Nam Bộ trải nghiệm hái trái cây tại vườn, thưởng thức ẩm thực dân gian, đua heo, đua chó và tham quan nhà cổ.",
                        Address = "Số 335 lộ vòng cung, Mỹ Khánh, Phong Điền, Cần Thơ",
                        Latitude = 10.012586m,
                        Longitude = 105.714488m,
                        CoverUrl = "https://images.unsplash.com/photo-1501854140801-50d01698950b?auto=format&fit=crop&w=600&q=80"
                    }
                };

                foreach (var dSeed in destSeeds)
                {
                    var dest = await context.Destinations.Include(d => d.CoverImage).FirstOrDefaultAsync(d => d.Slug == dSeed.Slug);
                    if (dest == null)
                    {
                        // 5.1. Tạo cover image
                        var coverMediaId = Guid.NewGuid();
                        var storagePath = $"media/images/covers/{dSeed.Slug}.jpg";
                        
                        // Cố gắng tải và upload cover lên MinIO
                        var uploadedMedia = await DownloadAndUploadImageAsync(storageService, dSeed.CoverUrl, $"{dSeed.Slug}-cover.jpg", "image/jpeg", "images/covers");
                        
                        var coverMedia = new MediaFile
                        {
                            Id = coverMediaId,
                            FileName = $"{dSeed.Slug}-cover.jpg",
                            StoredFileName = uploadedMedia?.storagePath != null ? Path.GetFileName(uploadedMedia.Value.storagePath) : $"{dSeed.Slug}-cover.jpg",
                            FileUrl = uploadedMedia?.fileUrl ?? dSeed.CoverUrl,
                            StoragePath = uploadedMedia?.storagePath ?? "fallback_remote",
                            StorageProvider = uploadedMedia?.storagePath != null ? "minio" : "remote",
                            MediaType = "image",
                            MimeType = "image/jpeg",
                            Extension = ".jpg",
                            FileSize = 0,
                            CreatedAt = DateTime.UtcNow
                        };

                        await context.MediaFiles.AddAsync(coverMedia);

                        // 5.2. Tạo destination
                        dest = new Destination
                        {
                            RegionId = regionCanTho.Id,
                            CategoryId = dSeed.CategoryId,
                            Name = dSeed.Name,
                            Slug = dSeed.Slug,
                            ShortDescription = dSeed.ShortDescription,
                            FullDescription = dSeed.FullDescription,
                            Address = dSeed.Address,
                            Latitude = dSeed.Latitude,
                            Longitude = dSeed.Longitude,
                            Location = new Point((double)dSeed.Longitude, (double)dSeed.Latitude) { SRID = 4326 },
                            CoverImageId = coverMediaId,
                            Status = "published",
                            HasVirtualTour = false,
                            CreatedAt = DateTime.UtcNow
                        };

                        await context.Destinations.AddAsync(dest);
                        await context.SaveChangesAsync();
                    }
                }

                // 6. Seed Virtual Tours cho Cầu Cần Thơ & Nhà Cổ Bình Thủy
                // LƯU Ý: Không đè Virtual Tour của Chùa Ông Cần Thơ (chỉ tạo nếu chưa có tour cho địa điểm đó)

                // 6.1. Seed Tour cho Nhà Cổ Bình Thủy
                var nhaCoDest = await context.Destinations.FirstOrDefaultAsync(d => d.Slug == "nha-co-binh-thuy");
                if (nhaCoDest != null)
                {
                    var hasTour = await context.VirtualTours.AnyAsync(t => t.DestinationId == nhaCoDest.Id);
                    if (!hasTour)
                    {
                        await SeedTourForNhaCoBinhThuyAsync(context, storageService, nhaCoDest);
                    }
                }

                // 6.2. Seed Tour cho Cầu Cần Thơ
                var cauCanThoDest = await context.Destinations.FirstOrDefaultAsync(d => d.Slug == "cau-can-tho");
                if (cauCanThoDest != null)
                {
                    var hasTour = await context.VirtualTours.AnyAsync(t => t.DestinationId == cauCanThoDest.Id);
                    if (!hasTour)
                    {
                        await SeedTourForCauCanThoAsync(context, storageService, cauCanThoDest);
                    }
                }
            }
        }

        private static async Task SeedTourForNhaCoBinhThuyAsync(SmartTourism360DbContext context, IStorageService? storageService, Destination destination)
        {
            var tourId = Guid.NewGuid();
            var tourThumbUrl = "https://images.unsplash.com/photo-1596422846543-75c6fc18a52b?auto=format&fit=crop&w=300&q=80";
            
            // Upload tour thumbnail
            var thumbUpload = await DownloadAndUploadImageAsync(storageService, tourThumbUrl, "nha-co-tour-thumb.jpg", "image/jpeg", "images/thumbnails");
            var tourThumb = new MediaFile
            {
                Id = Guid.NewGuid(),
                FileName = "nha-co-tour-thumb.jpg",
                StoredFileName = thumbUpload?.storagePath != null ? Path.GetFileName(thumbUpload.Value.storagePath) : "nha-co-tour-thumb.jpg",
                FileUrl = thumbUpload?.fileUrl ?? tourThumbUrl,
                StoragePath = thumbUpload?.storagePath ?? "fallback_remote",
                StorageProvider = thumbUpload?.storagePath != null ? "minio" : "remote",
                MediaType = "image",
                MimeType = "image/jpeg",
                Extension = ".jpg",
                CreatedAt = DateTime.UtcNow
            };
            await context.MediaFiles.AddAsync(tourThumb);

            // Tạo VirtualTour với DefaultPanoramaId = null để tránh lỗi Circular Dependency
            var tour = new VirtualTour
            {
                Id = tourId,
                DestinationId = destination.Id,
                Title = "Tham quan ảo Nhà Cổ Bình Thủy",
                Description = "Khám phá kiến trúc Đông Dương độc đáo giao thoa Pháp - Hoa qua lăng kính 360 độ.",
                ThumbnailId = tourThumb.Id,
                DefaultPanoramaId = null,
                Status = "published",
                CreatedAt = DateTime.UtcNow
            };
            await context.VirtualTours.AddAsync(tour);
            await context.SaveChangesAsync();

            // Upload các ảnh Panorama 360 độ (Equirectangular 2:1)
            var pano1Url = "https://photo-sphere-viewer-data.netlify.app/assets/sphere.jpg";
            var pano2Url = "https://photo-sphere-viewer-data.netlify.app/assets/sphere-truncated.jpg";

            var pano1Upload = await DownloadAndUploadImageAsync(storageService, pano1Url, "nhaco-san-truoc.jpg", "image/jpeg", "panoramas");
            var pano1Image = new MediaFile
            {
                Id = Guid.NewGuid(),
                FileName = "nhaco-san-truoc.jpg",
                StoredFileName = pano1Upload?.storagePath != null ? Path.GetFileName(pano1Upload.Value.storagePath) : "nhaco-san-truoc.jpg",
                FileUrl = pano1Upload?.fileUrl ?? pano1Url,
                StoragePath = pano1Upload?.storagePath ?? "fallback_remote",
                StorageProvider = pano1Upload?.storagePath != null ? "minio" : "remote",
                MediaType = "panorama",
                MimeType = "image/jpeg",
                Extension = ".jpg",
                Width = 4096,
                Height = 2048,
                CreatedAt = DateTime.UtcNow
            };
            await context.MediaFiles.AddAsync(pano1Image);

            var pano2Upload = await DownloadAndUploadImageAsync(storageService, pano2Url, "nhaco-phong-khach.jpg", "image/jpeg", "panoramas");
            var pano2Image = new MediaFile
            {
                Id = Guid.NewGuid(),
                FileName = "nhaco-phong-khach.jpg",
                StoredFileName = pano2Upload?.storagePath != null ? Path.GetFileName(pano2Upload.Value.storagePath) : "nhaco-phong-khach.jpg",
                FileUrl = pano2Upload?.fileUrl ?? pano2Url,
                StoragePath = pano2Upload?.storagePath ?? "fallback_remote",
                StorageProvider = pano2Upload?.storagePath != null ? "minio" : "remote",
                MediaType = "panorama",
                MimeType = "image/jpeg",
                Extension = ".jpg",
                Width = 4096,
                Height = 2048,
                CreatedAt = DateTime.UtcNow
            };
            await context.MediaFiles.AddAsync(pano2Image);

            // Tạo các Panorama
            var pano1Id = Guid.NewGuid();
            var pano2Id = Guid.NewGuid();

            var pano1 = new Panorama
            {
                Id = pano1Id,
                VirtualTourId = tourId,
                Title = "Sân trước Nhà Cổ",
                Description = "Góc nhìn bao quát khoảng sân gạch tàu cổ kính hướng vào mặt tiền lộng lẫy phong cách Pháp.",
                PanoramaImageId = pano1Image.Id,
                DisplayOrder = 1,
                InitialYaw = 0,
                InitialPitch = 0,
                Status = "published",
                CreatedAt = DateTime.UtcNow
            };

            var pano2 = new Panorama
            {
                Id = pano2Id,
                VirtualTourId = tourId,
                Title = "Trong gian chính Nhà Cổ",
                Description = "Khám phá bàn thờ gỗ khảm xà cừ cổ kính cùng các cổ vật giá trị thời Nguyễn.",
                PanoramaImageId = pano2Image.Id,
                DisplayOrder = 2,
                InitialYaw = 3.14m, // quay mặt ngược lại khi vào
                InitialPitch = 0,
                Status = "published",
                CreatedAt = DateTime.UtcNow
            };

            await context.Panoramas.AddRangeAsync(pano1, pano2);
            await context.SaveChangesAsync();

            // Đặt DefaultPanoramaId sau khi Panorama đã được lưu
            tour.DefaultPanoramaId = pano1Id;
            context.VirtualTours.Update(tour);

            // Tạo các Hotspots liên kết 2 chiều
            var hs1 = new Hotspot
            {
                Id = Guid.NewGuid(),
                PanoramaId = pano1Id,
                Type = "navigation",
                Title = "Đi vào trong nhà cổ",
                Description = "Bước qua bậc thềm Pháp cổ để khám phá nội thất tinh xảo bên trong.",
                Yaw = 0.05m,
                Pitch = -0.1m,
                TargetPanoramaId = pano2Id,
                Status = "published",
                Icon = "arrow-up",
                CreatedAt = DateTime.UtcNow
            };

            var hs2 = new Hotspot
            {
                Id = Guid.NewGuid(),
                PanoramaId = pano2Id,
                Type = "navigation",
                Title = "Trở ra ngoài sân trước",
                Description = "Quay trở lại ngắm nhìn khoảng sân vườn bonsai lộng gió.",
                Yaw = 3.1m,
                Pitch = -0.15m,
                TargetPanoramaId = pano1Id,
                Status = "published",
                Icon = "arrow-down",
                CreatedAt = DateTime.UtcNow
            };

            // Thêm các hotspot thông tin / audio
            var hsInfo = new Hotspot
            {
                Id = Guid.NewGuid(),
                PanoramaId = pano1Id,
                Type = "info",
                Title = "Kiến trúc hoa văn chạm nổi",
                Description = "Các chi tiết phù điêu đắp nổi tinh xảo trên đầu cột được nghệ nhân phục dựng nguyên bản theo phong cách Phục Hưng Pháp.",
                Yaw = 0.5m,
                Pitch = 0.2m,
                Status = "published",
                CreatedAt = DateTime.UtcNow
            };

            var hsAudio = new Hotspot
            {
                Id = Guid.NewGuid(),
                PanoramaId = pano2Id,
                Type = "audio",
                Title = "Lịch sử dòng họ Dương Bình Thủy",
                Description = "Nghe thuyết minh audio sơ lược về quá trình lập nghiệp của dòng họ Dương đất Nam Kỳ Lục Tỉnh và xây dựng dinh thự này.",
                Yaw = -0.4m,
                Pitch = -0.05m,
                ExternalUrl = "https://www.soundhelix.com/examples/mp3/SoundHelix-Song-1.mp3", // sample audio URL
                Status = "published",
                CreatedAt = DateTime.UtcNow
            };

            await context.Hotspots.AddRangeAsync(hs1, hs2, hsInfo, hsAudio);

            destination.HasVirtualTour = true;
            context.Destinations.Update(destination);
            await context.SaveChangesAsync();
        }

        private static async Task SeedTourForCauCanThoAsync(SmartTourism360DbContext context, IStorageService? storageService, Destination destination)
        {
            var tourId = Guid.NewGuid();
            var tourThumbUrl = "https://images.unsplash.com/photo-1545638870-12206753a76f?auto=format&fit=crop&w=300&q=80";
            
            // Upload tour thumbnail
            var thumbUpload = await DownloadAndUploadImageAsync(storageService, tourThumbUrl, "cau-can-tho-tour-thumb.jpg", "image/jpeg", "images/thumbnails");
            var tourThumb = new MediaFile
            {
                Id = Guid.NewGuid(),
                FileName = "cau-can-tho-tour-thumb.jpg",
                StoredFileName = thumbUpload?.storagePath != null ? Path.GetFileName(thumbUpload.Value.storagePath) : "cau-can-tho-tour-thumb.jpg",
                FileUrl = thumbUpload?.fileUrl ?? tourThumbUrl,
                StoragePath = thumbUpload?.storagePath ?? "fallback_remote",
                StorageProvider = thumbUpload?.storagePath != null ? "minio" : "remote",
                MediaType = "image",
                MimeType = "image/jpeg",
                Extension = ".jpg",
                CreatedAt = DateTime.UtcNow
            };
            await context.MediaFiles.AddAsync(tourThumb);

            // Tạo VirtualTour với DefaultPanoramaId = null để tránh lỗi Circular Dependency
            var tour = new VirtualTour
            {
                Id = tourId,
                DestinationId = destination.Id,
                Title = "Khám phá Cầu Cần Thơ từ trên cao",
                Description = "Góc nhìn Panorama 360 hoành tráng sông Hậu và toàn cảnh dây văng cầu Cần Thơ.",
                ThumbnailId = tourThumb.Id,
                DefaultPanoramaId = null,
                Status = "published",
                CreatedAt = DateTime.UtcNow
            };
            await context.VirtualTours.AddAsync(tour);
            await context.SaveChangesAsync();

            // Upload các ảnh Panorama 360 độ (Equirectangular 2:1)
            var pano1Url = "https://photo-sphere-viewer-data.netlify.app/assets/sphere.jpg";
            var pano2Url = "https://photo-sphere-viewer-data.netlify.app/assets/sphere-truncated.jpg";

            var pano1Upload = await DownloadAndUploadImageAsync(storageService, pano1Url, "cau-duoi-chan.jpg", "image/jpeg", "panoramas");
            var pano1Image = new MediaFile
            {
                Id = Guid.NewGuid(),
                FileName = "cau-duoi-chan.jpg",
                StoredFileName = pano1Upload?.storagePath != null ? Path.GetFileName(pano1Upload.Value.storagePath) : "cau-duoi-chan.jpg",
                FileUrl = pano1Upload?.fileUrl ?? pano1Url,
                StoragePath = pano1Upload?.storagePath ?? "fallback_remote",
                StorageProvider = pano1Upload?.storagePath != null ? "minio" : "remote",
                MediaType = "panorama",
                MimeType = "image/jpeg",
                Extension = ".jpg",
                Width = 4096,
                Height = 2048,
                CreatedAt = DateTime.UtcNow
            };
            await context.MediaFiles.AddAsync(pano1Image);

            var pano2Upload = await DownloadAndUploadImageAsync(storageService, pano2Url, "cau-tren-mat.jpg", "image/jpeg", "panoramas");
            var pano2Image = new MediaFile
            {
                Id = Guid.NewGuid(),
                FileName = "cau-tren-mat.jpg",
                StoredFileName = pano2Upload?.storagePath != null ? Path.GetFileName(pano2Upload.Value.storagePath) : "cau-tren-mat.jpg",
                FileUrl = pano2Upload?.fileUrl ?? pano2Url,
                StoragePath = pano2Upload?.storagePath ?? "fallback_remote",
                StorageProvider = pano2Upload?.storagePath != null ? "minio" : "remote",
                MediaType = "panorama",
                MimeType = "image/jpeg",
                Extension = ".jpg",
                Width = 4096,
                Height = 2048,
                CreatedAt = DateTime.UtcNow
            };
            await context.MediaFiles.AddAsync(pano2Image);

            // Tạo các Panorama
            var pano1Id = Guid.NewGuid();
            var pano2Id = Guid.NewGuid();

            var pano1 = new Panorama
            {
                Id = pano1Id,
                VirtualTourId = tourId,
                Title = "Dưới chân Cầu Cần Thơ",
                Description = "Góc ngắm nhìn những cột trụ bê tông khổng lồ đỡ nhịp cầu từ bến đò cũ sông Hậu.",
                PanoramaImageId = pano1Image.Id,
                DisplayOrder = 1,
                InitialYaw = 0,
                InitialPitch = 0,
                Status = "published",
                CreatedAt = DateTime.UtcNow
            };

            var pano2 = new Panorama
            {
                Id = pano2Id,
                VirtualTourId = tourId,
                Title = "Trên mặt Cầu Cần Thơ",
                Description = "Đứng giữa làn dây văng đỏ cam rực rỡ dưới nắng chiều sông nước miền Tây.",
                PanoramaImageId = pano2Image.Id,
                DisplayOrder = 2,
                InitialYaw = 1.57m,
                InitialPitch = 0,
                Status = "published",
                CreatedAt = DateTime.UtcNow
            };

            await context.Panoramas.AddRangeAsync(pano1, pano2);
            await context.SaveChangesAsync();

            // Đặt DefaultPanoramaId sau khi Panorama đã được lưu
            tour.DefaultPanoramaId = pano1Id;
            context.VirtualTours.Update(tour);

            // Tạo các Hotspots liên kết 2 chiều
            var hs1 = new Hotspot
            {
                Id = Guid.NewGuid(),
                PanoramaId = pano1Id,
                Type = "navigation",
                Title = "Lên mặt cầu chính",
                Description = "Di chuyển điểm nhìn lên làn đường bộ nhộn nhịp của cầu.",
                Yaw = 0.1m,
                Pitch = 0.3m,
                TargetPanoramaId = pano2Id,
                Status = "published",
                Icon = "arrow-up",
                CreatedAt = DateTime.UtcNow
            };

            var hs2 = new Hotspot
            {
                Id = Guid.NewGuid(),
                PanoramaId = pano2Id,
                Type = "navigation",
                Title = "Xuống bờ sông Hậu",
                Description = "Quay về điểm nhìn công viên sinh thái dưới chân cầu.",
                Yaw = -1.5m,
                Pitch = -0.5m,
                TargetPanoramaId = pano1Id,
                Status = "published",
                Icon = "arrow-down",
                CreatedAt = DateTime.UtcNow
            };

            // Thêm các hotspot thông tin
            var hsInfo = new Hotspot
            {
                Id = Guid.NewGuid(),
                PanoramaId = pano2Id,
                Type = "info",
                Title = "Kỷ lục chiều dài nhịp cầu",
                Description = "Cầu có tổng chiều dài toàn tuyến là 15,85km, nhịp chính dây văng dài 550m nối liền Vĩnh Long và Cần Thơ.",
                Yaw = 0.8m,
                Pitch = 0.1m,
                Status = "published",
                CreatedAt = DateTime.UtcNow
            };

            await context.Hotspots.AddRangeAsync(hs1, hs2, hsInfo);

            destination.HasVirtualTour = true;
            context.Destinations.Update(destination);
            await context.SaveChangesAsync();
        }

        private static async Task<(string fileUrl, string storagePath)?> DownloadAndUploadImageAsync(
            IStorageService? storageService,
            string imageUrl,
            string fileName,
            string contentType,
            string folder)
        {
            if (storageService == null) return null;
            try
            {
                using var httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromSeconds(15);
                var bytes = await httpClient.GetByteArrayAsync(imageUrl);
                using var stream = new MemoryStream(bytes);
                
                var storagePath = await storageService.UploadFileAsync(stream, fileName, contentType, folder);
                var fileUrl = storageService.GetPublicUrl(storagePath);
                return (fileUrl, storagePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Seeding Warning] Error seeding media from {imageUrl}: {ex.Message}");
                return null;
            }
        }
    }
}
