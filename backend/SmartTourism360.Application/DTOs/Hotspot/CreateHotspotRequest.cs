using System;
using System.ComponentModel.DataAnnotations;

namespace SmartTourism360.Application.DTOs.Hotspot
{
    public class CreateHotspotRequest
    {
        [Required(ErrorMessage = "Loại điểm Hotspot là bắt buộc.")]
        public string Type { get; set; } = null!; // info/navigation/audio/video/image/model3d/link

        [Required(ErrorMessage = "Tiêu đề là bắt buộc.")]
        [MaxLength(200, ErrorMessage = "Tiêu đề không được vượt quá 200 ký tự.")]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        [Required(ErrorMessage = "Góc Yaw là bắt buộc.")]
        public decimal Yaw { get; set; }

        [Required(ErrorMessage = "Góc Pitch là bắt buộc.")]
        public decimal Pitch { get; set; }

        public Guid? TargetPanoramaId { get; set; }
        public Guid? MediaId { get; set; }
        public string? ExternalUrl { get; set; }
        public string? Icon { get; set; }
        public int DisplayOrder { get; set; } = 0;
    }
}
