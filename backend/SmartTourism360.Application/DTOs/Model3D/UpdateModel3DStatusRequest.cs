using System.ComponentModel.DataAnnotations;

namespace SmartTourism360.Application.DTOs.Model3D
{
    public class UpdateModel3DStatusRequest
    {
        [Required(ErrorMessage = "Trạng thái là bắt buộc.")]
        [RegularExpression("^(draft|published|archived)$", ErrorMessage = "Trạng thái không hợp lệ.")]
        public string Status { get; set; } = null!;
    }
}
