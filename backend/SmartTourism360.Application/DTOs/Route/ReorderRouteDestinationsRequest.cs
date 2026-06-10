using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartTourism360.Application.DTOs.Route
{
    public class ReorderRouteDestinationsRequest
    {
        [Required(ErrorMessage = "Danh sách sắp xếp không được để trống.")]
        [MinLength(1, ErrorMessage = "Danh sách sắp xếp phải có ít nhất 1 phần tử.")]
        public List<ReorderItemDto> Items { get; set; } = new();
    }

    public class ReorderItemDto
    {
        [Required(ErrorMessage = "Id của điểm đến trong tuyến (RouteDestinationId) không được để trống.")]
        public Guid RouteDestinationId { get; set; }

        [Required(ErrorMessage = "Thứ tự hiển thị không được để trống.")]
        [Range(1, 1000, ErrorMessage = "Thứ tự hiển thị phải lớn hơn hoặc bằng 1.")]
        public int DisplayOrder { get; set; }
    }
}
