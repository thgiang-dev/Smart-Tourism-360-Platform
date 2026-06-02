using System.ComponentModel.DataAnnotations;

namespace SmartTourism360.Application.DTOs.Destination
{
    public class UpdateDestinationStatusRequest
    {
        [Required(ErrorMessage = "Trạng thái không được để trống.")]
        [RegularExpression(@"^(draft|published|archived)$", ErrorMessage = "Trạng thái không hợp lệ. Chỉ chấp nhận draft, published, archived.")]
        public string Status { get; set; } = null!;
    }
}
