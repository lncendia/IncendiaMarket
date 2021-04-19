using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using WebApplication.Data.Models;

namespace WebApplication.ViewModels
{
    public class AdvertisementInfoViewModel
    {
        public Advertisement Advertisement { get; set; }
        public List<Comment> Comments { get; set; }
        public IdentityUser User { get; set; }
    }
}