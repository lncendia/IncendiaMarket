using System.Collections;
using System.Collections.Generic;
using WebApplication.Data.Models;

namespace WebApplication.Data.Interfaces
{
    public interface ICarsCategory
    {
        List<Category> AllCategories { get; }
    }
}