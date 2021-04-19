using System.ComponentModel.DataAnnotations;

namespace WebApplication.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Такой никнейм уже используется")]
        [StringLength(20, ErrorMessage = "Не больше 20 символов")]
        [Display(Name = "Введите имя пользователя")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Такой email уже используется")]
        [StringLength(400, ErrorMessage = "Не больше 400 символов")]
        [Display(Name = "Введите ваш электронный адрес")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [DataType(DataType.Password)]
        [Display(Name = "Введите пароль")]
        [StringLength(50, ErrorMessage = "Не больше 50 символов")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [StringLength(50, ErrorMessage = "Не больше 50 символов")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }
    }
}