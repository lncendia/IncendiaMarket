using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication.Data.Interfaces;
using WebApplication.Data.Models;

namespace WebApplication.Data.Repository
{
    public class AdvertisementRepository : IAllAdvertisement
    {
        private readonly Db _db;

        public AdvertisementRepository(Db db)
        {
            _db = db;
            Advertisements = _db.Advertisements;
        }

        public DbSet<Advertisement> Advertisements { get; }

        public bool AddAdvertisement(Advertisement car)
        {
            try
            {
                _db.Add(car);
                _db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Advertisement GetAdvertisement(int id)
        {
            var car = _db.Advertisements.Include(_ => _.Category).Include(_ => _.IdentityUser)
                .FirstOrDefault(_ => _.Id == id);
            return car;
        }

        public Advertisement GetAdvertisementWithAddingView(int id)
        {
            var car = _db.Advertisements.Include(_ => _.Category).Include(_ => _.IdentityUser)
                .FirstOrDefault(_ => _.Id == id);
            if (car == null) return null;
            car.CountViews++;
            _db.SaveChanges();
            return car;
        }

        public List<Comment> GetComments(Advertisement advertisement, int offset)
        {
            return _db.Comments.Where(com => com.Advertisement == advertisement).OrderBy(com => com.Date).Skip(offset)
                .Take(8)
                .Include(_ => _.Advertisement).Include(_ => _.User).Include(_ => _.ParentComment).ToList();
        }

        public List<Comment> AddComment(IdentityUser user, Advertisement advertisement, string text, int parentId)
        {
            var comment = new Comment()
            {
                Advertisement = advertisement,
                Date = DateTime.Now,
                Text = text,
                User = user,
                ParentComment = _db.Comments.FirstOrDefault(id => id.Id == parentId)
            };
            _db.Comments.Add(comment);
            _db.SaveChanges();
            return _db.Comments.Where(com => com.Advertisement == advertisement).OrderBy(com => com.Date)
                .Take(8)
                .Include(_ => _.Advertisement).Include(_ => _.User).Include(_ => _.ParentComment).ToList();
        }

        public bool RemoveComment(int id, IdentityUser user)
        {
            var comment = _db.Comments.Include(com => com.User).FirstOrDefault(com => com.Id == id);
            if (comment == null || user == null) return false;
            if (comment.User != user) return false;
            _db.Comments.Where(com => com.ParentComment == comment).ToList()
                .ForEach(com => com.ParentComment = null);
            _db.Remove(comment);
            _db.SaveChanges();
            return true;
        }
    }
}