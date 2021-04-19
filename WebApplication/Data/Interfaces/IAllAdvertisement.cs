using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication.Data.Models;

namespace WebApplication.Data.Interfaces
{
    public interface IAllAdvertisement
    {
        DbSet<Advertisement> Advertisements { get; }
        bool AddAdvertisement(Advertisement car);
        Advertisement GetAdvertisement(int id);
        Advertisement GetAdvertisementWithAddingView(int id);
        List<Comment> GetComments(Advertisement advertisement, int offset);
        List<Comment> AddComment(IdentityUser user, Advertisement advertisement, string text, int parentId);
        bool RemoveComment(int id, IdentityUser user);
    }
}