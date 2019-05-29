using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TodoREST;

namespace HabitatBuddy.Models
{
    public class Category
    {
        public string title { get; set; } // Title of the category
        public ObservableCollection<HomeIssue> issues { get; set; } // List of issues to be displayed as a sublist of the category
        public bool visible { get; set; } // Sets whether the sublist is currently expanded in the category view
        public int sublistHeight { get; set; } // The required height of the sublist in the category view

        public Category(string t) {
            issues = new ObservableCollection<HomeIssue>(); 
            title = t;
            visible = false;
        }

        /*
         * Used to toggle the sublist in the category view
         */
        public void toggleVisibility() {
            visible = !visible;
        }

        /*
         * Recalculate the sublist height based off of the number of issues
         */
        public void calcSublistHeight() {
            sublistHeight = 10 + 33 * issues.Count;
        }
    }
}
