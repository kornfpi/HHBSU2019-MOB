using System;
using System.Collections.Generic;
using System.Text;

namespace HabitatBuddy.Models
{
    public class MaintenanceItem
    {
        public string maintenanceId { get; set; }
        public string title { get; set; }
        public DateTime dueDate { get; set; }
        public int recurrencePeriod { get; set; }
        public TodoREST.HomeIssue actionPlan { get; set; }
        public string displayTitle { get; set; }

        public MaintenanceItem(string t, int r, TodoREST.HomeIssue p)
        {
            title = t;
            recurrencePeriod = r;
            actionPlan = p;
            dueDate = DateTime.Now;

            resetDueDate();
        }

        /*
         * Recalculates the display title that should be displayed in the maintenance reminder listview
         */
        public void reloadTitle()
        {
            // get days til due
            int days = (int)(dueDate - DateTime.Today).TotalDays;
            if (days < 0)
            {
                days *= -1;
                displayTitle = String.Format("Overdue {0,1} days - {1}", days, this.title);
            }
            else if (days == 0)
            {
                displayTitle = String.Format("Due Today - {0}", this.title);
            }
            else
            {
                displayTitle = String.Format("{0,1} - {1}", this.dueDate.ToShortDateString(), this.title);
            }
        }

        // Resets the due date based on the recurrence period. The new due date is the current date plus recurrence period days.
        public void resetDueDate()
        {
            dueDate = DateTime.Now.AddDays(recurrencePeriod);
            reloadTitle(); // Recalculate the display title since due date has most likely changed
        }
    }
}
