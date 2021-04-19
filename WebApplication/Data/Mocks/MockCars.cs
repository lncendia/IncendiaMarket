using System.Collections.Generic;
using System.Linq;
using WebApplication.Data.Interfaces;
using WebApplication.Data.Models;

namespace WebApplication.Data.Mocks
{
    public class MockCars:IAllCars
    {
        private readonly ICarsCategory _category = new MockCategory();

        public IEnumerable<Car> Cars =>
            new List<Car>()
            {
                new()
                {
                    Name = "ВАЗ 21099", LongDescription = "жопа", Price = 2000, Available = true, Image = "/img/99.jpg",
                    IsFavorite = false, ShortDescription = "жоп", Category = _category.AllCategories.First()
                },
                new()
                {
                    Name = "ВАЗ 2105", LongDescription = "фальшивка", Price = 1000, Available = true, Image = "/img/5.jpg",
                    IsFavorite = false, ShortDescription = "жоп", Category = _category.AllCategories.First()
                },
                new()
                {
                    Name = "Богдан", LongDescription = "не автобус", Price = 65000, Available = true, Image = "/img/Bogdan.jpg",
                    IsFavorite = false, ShortDescription = "жоп", Category = _category.AllCategories.Last()
                }, 
                new()
                {
                    Name = "Олег Кипелов", LongDescription = "машина еще та", Price = 5, Available = true, Image = "/img/Oleg.jpg",
                    IsFavorite = false, ShortDescription = "жоп", Category = _category.AllCategories.First()
                }
            };

        public IEnumerable<Car> GetFavoriteCars { get; set; }
        public Car GetObjectCar(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}