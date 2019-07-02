using System;
using System.Collections.Generic;
using System.Text;



namespace HabitatBuddy.Models
{

    public class HomeRegInfo
    {
        public string homeNumber { get; set; }
        public string streetAddress { get; set; }
        public int? zip { get; set; }
        public int? types { get; set; }
        public string registeredTo { get; set; }
    }

}