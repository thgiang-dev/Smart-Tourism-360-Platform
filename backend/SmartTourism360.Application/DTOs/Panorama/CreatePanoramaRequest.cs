using System;
using System.ComponentModel.DataAnnotations;

namespace SmartTourism360.Application.DTOs.Panorama
{
    public class CreatePanoramaRequest
    {
        [Required(ErrorMessage = "Tiêu đề không được trống.")]
        [StringLength(200, ErrorMessage = "Tiêu đề không được vượt quá 200 ký tự.")]
        public string Title { get; set; } = null!;

        [StringLength(1000, ErrorMessage = "Mô tả không được vượt quá 1000 ký tự.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "ID hình ảnh panorama không được trống.")]
        public Guid PanoramaImageId { get; set; }

        public Guid? ThumbnailId { get; set; }

        public int DisplayOrder { get; set; } = 0;

        [Range(-180, 180, ErrorMessage = "Initial Yaw phải nằm trong khoảng -180 đến 180.")]
        public decimal InitialYaw { get; set; } = 0;

        [Range(-90, 90, ErrorMessage = "Initial Pitch phải nằm trong khoảng -90 đến 90.")]
        public decimal InitialPitch { get; set; } = 0;

        [Range(10, 120, ErrorMessage = "Initial Fov phải nằm trong khoảng 10 đến 120.")]
        public decimal InitialFov { get; set; } = 90;
    }
}
