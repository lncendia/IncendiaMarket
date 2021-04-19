using System;
using Microsoft.AspNetCore.Identity;

namespace WebApplication.Data.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public Advertisement Advertisement { get; set; }
        public IdentityUser User { get; set; }
        public Comment ParentComment { get; set; }
    }
}