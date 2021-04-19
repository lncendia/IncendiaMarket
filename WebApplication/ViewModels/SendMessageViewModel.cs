using System.ComponentModel.DataAnnotations;

namespace WebApplication.ViewModels
{
    public class SendMessageViewModel
    {
        [Display(Name = "Введите развернутое описание")]
        [StringLength(400, ErrorMessage = "Не больше 400 символов")]
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        public string Text { get; set; }

        [Required(ErrorMessage = "Поле не должно быть пустым")]
        public int To { get; set; }
    }
}