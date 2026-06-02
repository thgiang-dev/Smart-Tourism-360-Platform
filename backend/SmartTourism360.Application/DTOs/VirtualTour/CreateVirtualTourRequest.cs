using System;
using System.ComponentModel.DataAnnotations;

namespace SmartTourism360.Application.DTOs.VirtualTour
{
    public class CreateVirtualTourRequest
    {
        [Required(ErrorMessage = "Tiêu đề không được trống.")]
        [StringLength(200, ErrorMessage = "Tiêu đề không được vượt quá 200 ký tự.")]
        public string Title { get; set; } = null!;

        [StringLength(1000, ErrorMessage = "Mô tả không được vượt quá 1000 ký tự.")]
        public string? Description { get; set; }

        public Guid? ThumbnailId { get; set; }
    }
}
