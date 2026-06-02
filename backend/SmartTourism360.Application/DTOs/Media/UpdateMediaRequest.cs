using System.ComponentModel.DataAnnotations;

namespace SmartTourism360.Application.DTOs.Media
{
    public class UpdateMediaRequest
    {
        [MaxLength(500, ErrorMessage = "Chú thích không được vượt quá 500 ký tự.")]
        public string? Caption { get; set; }

        [MaxLength(250, ErrorMessage = "Văn bản thay thế không được vượt quá 250 ký tự.")]
        public string? AltText { get; set; }
    }
}
