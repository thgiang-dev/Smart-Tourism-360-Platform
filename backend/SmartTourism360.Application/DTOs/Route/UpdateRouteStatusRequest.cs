using System.ComponentModel.DataAnnotations;

namespace SmartTourism360.Application.DTOs.Route
{
    public class UpdateRouteStatusRequest
    {
        [Required(ErrorMessage = "Trạng thái không được để trống.")]
        [RegularExpression(@"^(draft|published|archived)$", ErrorMessage = "Trạng thái không hợp lệ.")]
        public string Status { get; set; } = null!;
    }
}
