using System;
using System.ComponentModel.DataAnnotations;

namespace SmartTourism360.Application.DTOs.Model3D
{
    public class UpdateModel3DRequest
    {
        [Required(ErrorMessage = "Tiêu đề mô hình là bắt buộc.")]
        [StringLength(250, ErrorMessage = "Tiêu đề không được vượt quá 250 ký tự.")]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        [Required(ErrorMessage = "Tệp tin mô hình 3D là bắt buộc.")]
        public Guid MediaId { get; set; }

        public Guid? ThumbnailId { get; set; }

        [Required(ErrorMessage = "Loại đối tượng liên kết là bắt buộc.")]
        [RegularExpression("^(destination|panorama|hotspot)$", ErrorMessage = "Loại đối tượng liên kết không hợp lệ.")]
        public string TargetType { get; set; } = null!;

        [Required(ErrorMessage = "Đối tượng liên kết là bắt buộc.")]
        public Guid TargetId { get; set; }

        [StringLength(20)]
        public string? Format { get; set; }

        public int? PolygonCount { get; set; }

        [Required(ErrorMessage = "Trạng thái là bắt buộc.")]
        [RegularExpression("^(draft|published|archived)$", ErrorMessage = "Trạng thái không hợp lệ.")]
        public string Status { get; set; } = "draft";
    }
}
