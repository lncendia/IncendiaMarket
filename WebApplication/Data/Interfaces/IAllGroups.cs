using System.Collections.Generic;
using WebApplication.Data.Models;

namespace WebApplication.Data.Interfaces
{
    public interface IAllGroups
    {
        List<Group> AllGroups { get; }
    }
}