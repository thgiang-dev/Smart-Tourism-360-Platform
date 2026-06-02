using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;
using SmartTourism360.Domain.Common;

namespace SmartTourism360.Domain.Entities
{
    public class Destination : BaseEntity
    {
        public Guid RegionId { get; set; }
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string ShortDescription { get; set; } = null!;
        public string? FullDescription { get; set; }
        public string? Address { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public Point? Location { get; set; } // PostGIS Point geometry (nullable for flexibility)
        public Guid? CoverImageId { get; set; }
        public string? OpeningHoursJson { get; set; }
        public string? TicketPrice { get; set; }
        public string? ContactPhone { get; set; }
        public string? ContactEmail { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? FacebookUrl { get; set; }
        public bool HasVirtualTour { get; set; } = false;
        public bool HasAudioGuide { get; set; } = false;
        public bool Has3dModel { get; set; } = false;
        public string Status { get; set; } = "draft"; // draft/published/archived

        // Navigation properties
        public Region Region { get; set; } = null!;
        public Category Category { get; set; } = null!;
        public MediaFile? CoverImage { get; set; }
        public ICollection<VirtualTour> VirtualTours { get; set; } = new List<VirtualTour>();
        public ICollection<AudioGuide> AudioGuides { get; set; } = new List<AudioGuide>();
    }
}
