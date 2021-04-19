using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebApplication.Data.Models
{
    public class Advertisement
    {
        [BindNever] public int Id { get; set; }

        [Display(Name = "Введите название")]
        [StringLength(50, ErrorMessage = "Не больше 50 символов")]
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        public string Name { get; set; }

        [Display(Name = "Введите развернутое описание")]
        [StringLength(400, ErrorMessage = "Не больше 400 символов")]
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        public string Description { get; set; }

        [Display(Name = "Введите цену")]
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        public int Price { get; set; }

        [BindNever] public DateTime Date { get; set; }
        [BindNever] public int CountViews { get; set; }
        [BindNever] public string Image { get; set; }


        [BindNever] public Category Category { get; set; }
        [BindNever] public IdentityUser IdentityUser { get; set; }
    }
}