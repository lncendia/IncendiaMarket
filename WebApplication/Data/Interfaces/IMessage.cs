using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication.Data.Models;

namespace WebApplication.Data.Interfaces
{
    public interface IMessage
    {
        bool Send(IdentityUser from, IdentityUser to, string message);
        List<Message> Get(IdentityUser from, IdentityUser to, int offset);
        bool Delete(IdentityUser user, int id);
        DbSet<Message> Messages { get; }
    }
}