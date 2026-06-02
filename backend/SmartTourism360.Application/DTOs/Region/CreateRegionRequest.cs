using System;
using System.ComponentModel.DataAnnotations;

namespace SmartTourism360.Application.DTOs.Region
{
    public class CreateRegionRequest
    {
        [Required(ErrorMessage = "Tên khu vực không được để trống.")]
        [MaxLength(200, ErrorMessage = "Tên khu vực không được vượt quá 200 ký tự.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Slug không được để trống.")]
        [MaxLength(200, ErrorMessage = "Slug không được vượt quá 200 ký tự.")]
        [RegularExpression(@"^[a-z0-9-]+$", ErrorMessage = "Slug chỉ được chứa chữ thường, số và dấu gạch ngang.")]
        public string Slug { get; set; } = null!;

        public string? Description { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }

        [Range(-90, 90, ErrorMessage = "Vĩ độ (Latitude) phải nằm trong khoảng từ -90 đến 90.")]
        public decimal? CenterLatitude { get; set; }

        [Range(-180, 180, ErrorMessage = "Kinh độ (Longitude) phải nằm trong khoảng từ -180 đến 180.")]
        public decimal? CenterLongitude { get; set; }

        [Range(1, 22, ErrorMessage = "Zoom mặc định phải từ 1 đến 22.")]
        public int? DefaultZoom { get; set; }

        public Guid? CoverImageId { get; set; }

        [RegularExpression(@"^(draft|published|archived)$", ErrorMessage = "Trạng thái không hợp lệ.")]
        public string Status { get; set; } = "draft";
    }
}
