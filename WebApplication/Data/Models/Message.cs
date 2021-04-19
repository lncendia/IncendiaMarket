using System;
using Microsoft.AspNetCore.Identity;

namespace WebApplication.Data.Models
{
    public class Message
    {
        public int Id { get; set; }
        public IdentityUser From { get; set; }
        public IdentityUser To { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public bool Read { get; set; }
    }
}