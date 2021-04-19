using System.Collections.Generic;

namespace WebApplication.Data.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Category> Categories { get; set; }
    }
}