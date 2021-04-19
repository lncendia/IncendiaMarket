using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication.Data.Models;

namespace WebApplication.Data
{
    public sealed class Db:IdentityDbContext
    {
        public Db(DbContextOptions<Db> options):base(options)
        {
        }
        public DbSet<Advertisement> Advertisements { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Comment> Comments  { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}