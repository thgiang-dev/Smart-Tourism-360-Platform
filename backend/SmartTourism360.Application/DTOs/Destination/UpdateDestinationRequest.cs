using System;
using System.ComponentModel.DataAnnotations;

namespace SmartTourism360.Application.DTOs.Destination
{
    public class UpdateDestinationRequest
    {
        [Required(ErrorMessage = "Khu vực (RegionId) không được để trống.")]
        public Guid RegionId { get; set; }

        [Required(ErrorMessage = "Danh mục (CategoryId) không được để trống.")]
        public Guid CategoryId { get; set; }

        [Required(ErrorMessage = "Tên địa điểm không được để trống.")]
        [MaxLength(250, ErrorMessage = "Tên địa điểm không được vượt quá 250 ký tự.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Slug không được để trống.")]
        [MaxLength(250, ErrorMessage = "Slug không được vượt quá 250 ký tự.")]
        [RegularExpression(@"^[a-z0-9-]+$", ErrorMessage = "Slug chỉ được chứa chữ thường, số và dấu gạch ngang.")]
        public string Slug { get; set; } = null!;

        [Required(ErrorMessage = "Mô tả ngắn không được để trống.")]
        [MaxLength(500, ErrorMessage = "Mô tả ngắn không được vượt quá 500 ký tự.")]
        public string ShortDescription { get; set; } = null!;

        public string? FullDescription { get; set; }
        public string? Address { get; set; }

        [Required(ErrorMessage = "Vĩ độ (Latitude) không được để trống.")]
        [Range(-90.0, 90.0, ErrorMessage = "Vĩ độ phải nằm trong khoảng từ -90 đến 90.")]
        public decimal Latitude { get; set; }

        [Required(ErrorMessage = "Kinh độ (Longitude) không được để trống.")]
        [Range(-180.0, 180.0, ErrorMessage = "Kinh độ phải nằm trong khoảng từ -180 đến 180.")]
        public decimal Longitude { get; set; }

        public Guid? CoverImageId { get; set; }

        public string? OpeningHours { get; set; }
        public string? TicketPrice { get; set; }
        public string? ContactPhone { get; set; }
        public string? ContactEmail { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? FacebookUrl { get; set; }

        [RegularExpression(@"^(draft|published|archived)$", ErrorMessage = "Trạng thái không hợp lệ.")]
        public string Status { get; set; } = "draft";
    }
}
