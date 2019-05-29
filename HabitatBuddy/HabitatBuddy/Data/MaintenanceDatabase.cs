using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using HabitatBuddy.Models;

namespace HabitatBuddy.Data
{
    public class MaitenanceDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public MaitenanceDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Maintenance>().Wait();
        }

        public Task<List<Maintenance>> GetIssuesAsync()
        {
            return _database.Table<Maintenance>().ToListAsync();
        }

        public Task<Maintenance> GetIssueAsync(int id)
        {
            return _database.Table<Maintenance>()
                            .Where(i => i.MaintenanceItemID == id)
                            .FirstOrDefaultAsync();
        }

       

       
    }
}
