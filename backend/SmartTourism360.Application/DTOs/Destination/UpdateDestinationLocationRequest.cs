using System.ComponentModel.DataAnnotations;

namespace SmartTourism360.Application.DTOs.Destination
{
    public class UpdateDestinationLocationRequest
    {
        [Required(ErrorMessage = "Vĩ độ (Latitude) không được để trống.")]
        [Range(-90.0, 90.0, ErrorMessage = "Vĩ độ phải nằm trong khoảng từ -90 đến 90.")]
        public decimal Latitude { get; set; }

        [Required(ErrorMessage = "Kinh độ (Longitude) không được để trống.")]
        [Range(-180.0, 180.0, ErrorMessage = "Kinh độ phải nằm trong khoảng từ -180 đến 180.")]
        public decimal Longitude { get; set; }
    }
}
