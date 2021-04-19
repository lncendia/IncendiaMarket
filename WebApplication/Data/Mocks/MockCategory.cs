using System.Collections.Generic;
using WebApplication.Data.Interfaces;
using WebApplication.Data.Models;

namespace WebApplication.Data.Mocks
{
    public class MockCategory:ICarsCategory
    {
        public IEnumerable<Category> AllCategories =>
            new List<Category>
            {
                new Category(){CategoryName = "Электромобили",Description = "У тебя денег не хватит."},
                new Category(){CategoryName = "Бензиновые",Description = "Привет Гретте Тунберг."}
            };
    }
}