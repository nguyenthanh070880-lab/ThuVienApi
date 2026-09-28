using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.DTO
{
    public class AddAuthorRequestDTO
    {
        [Required(ErrorMessage = "Tên tác giả không được để trống")]
        [MinLength(3, ErrorMessage = "Tên tác giả phải có tối thiểu 3 ký tự")]
        public string FullName { get; set; }
    }
}