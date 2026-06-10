using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartTourism360.Application.DTOs.Route
{
    public class AddRouteDestinationsBulkRequest
    {
        [Required(ErrorMessage = "Danh sách địa điểm không được để trống.")]
        [MinLength(1, ErrorMessage = "Phải chọn ít nhất 1 địa điểm để thêm.")]
        public List<AddRouteDestinationRequest> Items { get; set; } = new();
    }
}
