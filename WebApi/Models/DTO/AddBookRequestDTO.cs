using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.DTO
{
    public class AddBookRequestDTO
    {
        [Required(ErrorMessage = "Title là bắt buộc")]
        [RegularExpression(@"^[^\^$\&\%#@!*()~`+=<>{}\[\]|\\/]+$", ErrorMessage = "Title không được chứa ký tự đặc biệt")]
        public string Title { get; set; }

        public string? Description { get; set; }
        public bool IsRead { get; set; }
        public DateTime? DateRead { get; set; }

        [Range(0, 5, ErrorMessage = "Rate phải nằm trong khoảng từ 0 đến 5")]
        public int? Rate { get; set; }

        public string? Genre { get; set; }
        public string? CoverUrl { get; set; }
        public DateTime DateAdded { get; set; }

        public int PublisherID { get; set; }
        public List<int> AuthorIds { get; set; }
    }
}