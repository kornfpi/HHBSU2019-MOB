using System;
namespace HabitatBuddy.Models
{
    public class Maintenance
    {
        public int MaintenanceItemID { get; set; }
        public string Name { get; set; }
        // number of days in between reminders
        public int RecurrencePeriod { get; set; }

        public int ActionPlanId { get; set; }


        //-------------------- Change here
        public string homecode { get; set; }

        // -----------------------

    }
}
