using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace HabitatBuddy.Models
{
    public class User
    {
        // Declare private members with get/set methods
        [PrimaryKey]
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }

        // Constructors
        public User() {}
        public User(string username, string password)
        {
            this.username = username;
            this.password = password;
        }


        // For checking user input, trivial for now
        public bool CheckInformation()
        {
            if(!username.Equals("") && !password.Equals(""))
            {
                return true;
            }
            return false;
        }
    }
}
