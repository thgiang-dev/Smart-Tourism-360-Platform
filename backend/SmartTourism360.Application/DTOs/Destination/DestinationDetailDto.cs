using System;
using System.Collections.Generic;

namespace SmartTourism360.Application.DTOs.Destination
{
    public class DestinationDetailDto
    {
        public Guid Id { get; set; }
        public Guid RegionId { get; set; }
        public string RegionName { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string ShortDescription { get; set; } = null!;
        public string? FullDescription { get; set; }
        public string? Address { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public CategoryInfoDto Category { get; set; } = null!;
        public string? CoverImageUrl { get; set; }
        public Guid? CoverImageId { get; set; }
        public List<MediaFileDto> Gallery { get; set; } = new();
        public List<MediaFileDto> Videos { get; set; } = new();
        public List<AudioGuideDto> AudioGuides { get; set; } = new();
        public bool HasVirtualTour { get; set; }
        public Guid? VirtualTourId { get; set; }
        public string? OpeningHours { get; set; }
        public string? TicketPrice { get; set; }
        public string? ContactPhone { get; set; }
        public string? ContactEmail { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? FacebookUrl { get; set; }
        public string Status { get; set; } = "draft";
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class MediaFileDto
    {
        public Guid Id { get; set; }
        public string Url { get; set; } = null!;
        public string? Caption { get; set; }
        public string? MediaType { get; set; }
    }

    public class AudioGuideDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string AudioUrl { get; set; } = null!;
        public int? Duration { get; set; }
    }
}
