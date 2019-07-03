using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace HabitatBuddy.Models
{
    public class Registration
    {
        [PrimaryKey]
        public string Id { get; set; } // House Number
        public string Name { get; set; } // User name 
        public bool isConfirmed { get; set; } // House number checked against database
    }
}
