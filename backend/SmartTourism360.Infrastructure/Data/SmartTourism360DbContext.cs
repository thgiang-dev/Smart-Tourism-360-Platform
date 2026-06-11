using Microsoft.EntityFrameworkCore;
using SmartTourism360.Domain.Entities;

namespace SmartTourism360.Infrastructure.Data
{
    public class SmartTourism360DbContext : DbContext
    {
        public SmartTourism360DbContext(DbContextOptions<SmartTourism360DbContext> options)
            : base(options)
        {
        }

        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Region> Regions { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<MediaFile> MediaFiles { get; set; } = null!;
        public DbSet<Destination> Destinations { get; set; } = null!;
        public DbSet<VirtualTour> VirtualTours { get; set; } = null!;
        public DbSet<Panorama> Panoramas { get; set; } = null!;
        public DbSet<Hotspot> Hotspots { get; set; } = null!;
        public DbSet<AudioGuide> AudioGuides { get; set; } = null!;
        public DbSet<Route> Routes { get; set; } = null!;
        public DbSet<RouteDestination> RouteDestinations { get; set; } = null!;
        public DbSet<AnalyticsEvent> AnalyticsEvents { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Bật PostGIS extension
            modelBuilder.HasPostgresExtension("postgis");

            // --- Cấu hình bảng Roles & Users ---
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("roles");
                entity.HasIndex(r => r.Code).IsUnique();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.HasIndex(u => u.Email).IsUnique();
                entity.HasOne(u => u.Role)
                    .WithMany(r => r.Users)
                    .HasForeignKey(u => u.RoleId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(u => u.Avatar)
                    .WithMany()
                    .HasForeignKey(u => u.AvatarId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // --- Cấu hình bảng Regions & Categories ---
            modelBuilder.Entity<Region>(entity =>
            {
                entity.ToTable("regions");
                entity.HasIndex(r => r.Slug).IsUnique();
                entity.HasOne(r => r.CoverImage)
                    .WithMany()
                    .HasForeignKey(r => r.CoverImageId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("categories");
                entity.HasIndex(c => c.Slug).IsUnique();
            });

            // --- Cấu hình bảng MediaFiles ---
            modelBuilder.Entity<MediaFile>(entity =>
            {
                entity.ToTable("media_files");
                entity.HasIndex(m => m.MediaType);
            });

            // --- Cấu hình bảng Destinations ---
            modelBuilder.Entity<Destination>(entity =>
            {
                entity.ToTable("destinations");
                entity.HasIndex(d => new { d.RegionId, d.Slug }).IsUnique();
                entity.HasIndex(d => d.CategoryId);

                entity.HasOne(d => d.Region)
                    .WithMany(r => r.Destinations)
                    .HasForeignKey(d => d.RegionId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Category)
                    .WithMany(c => c.Destinations)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.CoverImage)
                    .WithMany()
                    .HasForeignKey(d => d.CoverImageId)
                    .OnDelete(DeleteBehavior.SetNull);

                // Cấu hình PostGIS Point location
                entity.Property(d => d.Location)
                    .HasColumnType("geometry(Point, 4326)");

                // Tạo chỉ mục không gian (Spatial index) cho Location
                entity.HasIndex(d => d.Location)
                    .HasMethod("gist");
            });

            // --- Cấu hình bảng VirtualTours & Panoramas (Cắt vòng lặp cascade) ---
            modelBuilder.Entity<VirtualTour>(entity =>
            {
                entity.ToTable("virtual_tours");
                entity.HasIndex(t => t.DestinationId);

                entity.HasOne(t => t.Destination)
                    .WithMany(d => d.VirtualTours)
                    .HasForeignKey(t => t.DestinationId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(t => t.Thumbnail)
                    .WithMany()
                    .HasForeignKey(t => t.ThumbnailId)
                    .OnDelete(DeleteBehavior.SetNull);

                // Ràng buộc Restrict tránh vòng cascade delete
                entity.HasOne(t => t.DefaultPanorama)
                    .WithMany()
                    .HasForeignKey(t => t.DefaultPanoramaId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Panorama>(entity =>
            {
                entity.ToTable("panoramas");
                entity.HasIndex(p => p.VirtualTourId);

                entity.HasOne(p => p.VirtualTour)
                    .WithMany(t => t.Panoramas)
                    .HasForeignKey(p => p.VirtualTourId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(p => p.PanoramaImage)
                    .WithMany()
                    .HasForeignKey(p => p.PanoramaImageId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.Thumbnail)
                    .WithMany()
                    .HasForeignKey(p => p.ThumbnailId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // --- Cấu hình bảng Hotspots ---
            modelBuilder.Entity<Hotspot>(entity =>
            {
                entity.ToTable("hotspots");
                entity.HasIndex(h => h.PanoramaId);
                entity.HasIndex(h => h.TargetPanoramaId);

                entity.HasOne(h => h.Panorama)
                    .WithMany(p => p.Hotspots)
                    .HasForeignKey(h => h.PanoramaId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Ràng buộc Restrict cho điểm chuyển cảnh
                entity.HasOne(h => h.TargetPanorama)
                    .WithMany()
                    .HasForeignKey(h => h.TargetPanoramaId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(h => h.Media)
                    .WithMany()
                    .HasForeignKey(h => h.MediaId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // --- Cấu hình bảng AudioGuides ---
            modelBuilder.Entity<AudioGuide>(entity =>
            {
                entity.ToTable("audio_guides");
                entity.HasIndex(a => a.TargetId);

                entity.HasOne(a => a.Media)
                    .WithMany()
                    .HasForeignKey(a => a.MediaId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // --- Cấu hình bảng Routes ---
            modelBuilder.Entity<Route>(entity =>
            {
                entity.ToTable("routes");
                entity.HasIndex(r => new { r.RegionId, r.Slug }).IsUnique();
                entity.HasIndex(r => r.Status);
                entity.HasIndex(r => r.Theme);

                entity.HasOne(r => r.Region)
                    .WithMany()
                    .HasForeignKey(r => r.RegionId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(r => r.Thumbnail)
                    .WithMany()
                    .HasForeignKey(r => r.ThumbnailId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // --- Cấu hình bảng RouteDestinations ---
            modelBuilder.Entity<RouteDestination>(entity =>
            {
                entity.ToTable("route_destinations");
                entity.HasIndex(rd => new { rd.RouteId, rd.DestinationId }).IsUnique();
                entity.HasIndex(rd => new { rd.RouteId, rd.DisplayOrder });

                entity.HasOne(rd => rd.Route)
                    .WithMany(r => r.RouteDestinations)
                    .HasForeignKey(rd => rd.RouteId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(rd => rd.Destination)
                    .WithMany()
                    .HasForeignKey(rd => rd.DestinationId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // --- Cấu hình bảng AnalyticsEvents ---
            modelBuilder.Entity<AnalyticsEvent>(entity =>
            {
                entity.ToTable("analytics_events");
                
                entity.HasIndex(e => e.EventName);
                entity.HasIndex(e => new { e.TargetType, e.TargetId });
                entity.HasIndex(e => e.CreatedAt);
                entity.HasIndex(e => e.SessionId);
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.Metadata)
                    .HasColumnType("jsonb");

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}
