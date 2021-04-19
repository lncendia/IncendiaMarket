using System.Collections.Generic;
using WebApplication.Data.Models;

namespace WebApplication.ViewModels
{
    public class AdvertisementListViewModel
    {
        public IEnumerable<Advertisement> Advertisements { get; set; }
        public string Layout { get; set; }
    }
}