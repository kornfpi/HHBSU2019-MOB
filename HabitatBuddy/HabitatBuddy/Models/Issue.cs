using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace HabitatBuddy.Models {
    public class Issue {

        //[PrimaryKey, AutoIncrement]
        public int IssueID { get; set; }
        public string Title { get; set; }
        //public Category? Category { get; set; }
        //public Location? Location { get; set; }
        //public ICollection<IssuePartSystem> IssuePartSystems { get; } = new List<IssuePartSystem>();
        public string Content { get; set; }
        public string VideoLink { get; set; }

        public Issue() {
            IssueID = 0;
            Title = "Default Title";
            Content = "Default Content";
            VideoLink = "http://";
        }
        // Vendors
        //Service Providers
    }
}
