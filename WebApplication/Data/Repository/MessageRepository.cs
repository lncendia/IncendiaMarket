using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication.Data.Interfaces;
using WebApplication.Data.Models;

namespace WebApplication.Data.Repository
{
    public class MessageRepository : IMessage
    {
        private readonly Db _db;
        public DbSet<Message> Messages { get; }

        public MessageRepository(Db db)
        {
            _db = db;
            Messages = db.Messages;
        }

        public bool Send(IdentityUser from, IdentityUser to, string message)
        {
            if (from == null || to == null || string.IsNullOrEmpty(message)) return false;
            _db.Add(new Message {From = from, To = to, Text = message, Date = DateTime.Now});
            _db.SaveChanges();
            return true;
        }

        public List<Message> Get(IdentityUser from, IdentityUser to, int offset)
        {
            if (from == null || to == null) return null;
            return _db.Messages.Include(mes => mes.From).Include(mes => mes.To)
                .Where(mes => mes.From == from || mes.To == to).Where(mes => mes.From == to || mes.To == from)
                .OrderBy(mes => mes.Date).Skip(offset).Take(10).ToList();
        }

        public bool Delete(IdentityUser user, int id)
        {
            var message = _db.Messages.Where(mes => mes.From == user).Where(mes => mes.Id == id).ToList();
            _db.Remove(message);
            _db.SaveChanges();
            return true;
        }
    }
}