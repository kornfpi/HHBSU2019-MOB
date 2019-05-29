using System.Collections.Generic;
using System.Threading.Tasks;
using HabitatBuddy.Models;

namespace TodoREST
{
    public interface IMaintenance
    {
        Task<List<Maintenance>> RefreshDataAsync();


    }
}
