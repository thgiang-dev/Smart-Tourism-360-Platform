using System;

namespace SmartTourism360.Application.DTOs.Media
{
    public class MediaFileDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; } = null!;
        public string StoredFileName { get; set; } = null!;
        public string FileUrl { get; set; } = null!;
        public string StoragePath { get; set; } = null!;
        public string MediaType { get; set; } = null!;
        public string MimeType { get; set; } = null!;
        public string Extension { get; set; } = null!;
        public long FileSize { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public int? Duration { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string? AltText { get; set; }
        public string? Caption { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
