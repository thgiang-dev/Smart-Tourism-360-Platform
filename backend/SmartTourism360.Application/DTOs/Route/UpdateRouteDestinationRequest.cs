using System.ComponentModel.DataAnnotations;

namespace SmartTourism360.Application.DTOs.Route
{
    public class UpdateRouteDestinationRequest
    {
        [Required(ErrorMessage = "Thứ tự hiển thị không được để trống.")]
        [Range(1, 1000, ErrorMessage = "Thứ tự hiển thị phải lớn hơn hoặc bằng 1.")]
        public int DisplayOrder { get; set; }

        [MaxLength(100, ErrorMessage = "Thời gian lưu trú dự kiến không được vượt quá 100 ký tự.")]
        public string? EstimatedStayTime { get; set; }

        public string? Note { get; set; }
    }
}
