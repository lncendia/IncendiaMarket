using System.ComponentModel.DataAnnotations;

namespace WebApplication.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [Display(Name = "Имя пользователя")]
        [StringLength(20, ErrorMessage = "Не больше 20 символов")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [StringLength(50, ErrorMessage = "Не больше 50 символов")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить меня")] public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}