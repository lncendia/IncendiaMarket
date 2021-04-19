using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApplication.Data.Interfaces;
using WebApplication.Data.Models;

namespace WebApplication.Data.Repository
{
    public class GroupRepository : IAllGroups
    {
        private readonly Db _db;

        public GroupRepository(Db db)
        {
            _db = db;
        }

        public List<Group> AllGroups => _db.Groups.Include(_=>_.Categories).ToList();

        public Group GetGroup(Category category)
        {
            return _db.Groups.FirstOrDefault(_ => _.Categories.Contains(category));
        }
    }
}