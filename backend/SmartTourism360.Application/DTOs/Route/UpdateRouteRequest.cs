using System;
using System.ComponentModel.DataAnnotations;

namespace SmartTourism360.Application.DTOs.Route
{
    public class UpdateRouteRequest
    {
        [Required(ErrorMessage = "Khu vực (RegionId) không được để trống.")]
        public Guid RegionId { get; set; }

        [Required(ErrorMessage = "Tên tuyến không được để trống.")]
        [MaxLength(250, ErrorMessage = "Tên tuyến không được vượt quá 250 ký tự.")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Slug không được để trống.")]
        [MaxLength(250, ErrorMessage = "Slug không được vượt quá 250 ký tự.")]
        [RegularExpression(@"^[a-z0-9-]+$", ErrorMessage = "Slug chỉ được chứa chữ thường, số và dấu gạch ngang.")]
        public string Slug { get; set; } = null!;

        public string? Description { get; set; }
        
        [MaxLength(100, ErrorMessage = "Thời lượng dự kiến không được vượt quá 100 ký tự.")]
        public string? EstimatedDuration { get; set; }

        [Range(0, 10000.0, ErrorMessage = "Khoảng cách phải là số dương.")]
        public decimal? DistanceKm { get; set; }

        [MaxLength(100, ErrorMessage = "Chủ đề không được vượt quá 100 ký tự.")]
        public string? Theme { get; set; }

        public Guid? ThumbnailId { get; set; }

        [RegularExpression(@"^(draft|published|archived)$", ErrorMessage = "Trạng thái không hợp lệ.")]
        public string Status { get; set; } = "draft";
    }
}
