using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HabitatBuddy.Models;

namespace TodoREST
{
    public class MaintenanceManager
    {
        IMaintenance restService;

        public MaintenanceManager(IMaintenance service)
        {
            restService = service;
        }

        public Task<List<Maintenance>> GetTasksAsync()
        {
            return restService.RefreshDataAsync();
        }



       
    }
}

