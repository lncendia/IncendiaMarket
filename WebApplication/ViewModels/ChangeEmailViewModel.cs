using System.ComponentModel.DataAnnotations;

namespace WebApplication.ViewModels
{
    public class ChangeEmailViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Введите новый электронный адрес")]
        [StringLength(400, ErrorMessage = "Не больше 400 символов")]
        public string Email { get; set; }
    }
}