using Microsoft.AspNetCore.Identity;
using WebApplication.Data.Models;

namespace WebApplication.ViewModels
{
    public class CommentViewModel
    {
        public Comment Comment { get; set; }
        public IdentityUser User { get; set; }
    }
}