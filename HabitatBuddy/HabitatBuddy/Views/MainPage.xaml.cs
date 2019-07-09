﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using TodoREST;
using Xamarin.Forms;
using HabitatBuddy;
using Plugin.LocalNotifications;
using HabitatBuddy.Models;
using Xamarin.Forms.Xaml;

namespace HabitatBuddy {
    public partial class MainPage : ContentPage {

        /* 
         * Flags. Set to true when content is loading, set false when content is ready to display. 
         * Enables/Disables buttons on main page.
         */
        private bool loadingActionPlanContent = true;
        private bool loadingCategoryContent = true;
        private bool loadingReminderContent = true;


        // Decision tree object
        private Models.DecisionTree tree;

        // Collection of maintenance reminders, passed to a maintenance page instance
        private ObservableCollection<Models.MaintenanceItem> reminders;

        // Collection of category objects, passed to a category page instance
        private ObservableCollection<Models.Category> categories;

        // Hard coded home issue references, to populate the decision tree
        HomeIssue blank_issue, door_knob_loose, door_off_track, door_handle_broken, sewer, sump, water_heater_no_gas_electric, water_heater_yes_gas_electric, thermostat, hvac_electric, furnace_filter, furnace_lights;

        public MainPage() {

            InitializeComponent();

            // Get registration info
            App.getRegistrationInfo();

            if (App.isReg == false)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    MainLabel.Text = "Welcome to the Homeowner Buddy";
                    Address.Text = "Address: (unregistered)";
                });
            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    MainLabel.Text = "Welcome to the Homeowner Buddy, " + App.registeredName + "!";
                    Address.Text = "Address: " + App.regAddress;
                });
            }

            // Check to see if application is online and alter view accordingly
            if (App.isOnline)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Lbl_NoInternet.IsVisible = false;
                });

            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Lbl_NoInternet.IsVisible = true;
                    Lbl_NoInternet.Text = "Internet not connected! Some features may be disabled!";
                });
            }




            // Construct a blank action plan, to be used when no action plan is available.
            blank_issue = new HomeIssue();
            blank_issue.Title = "Blank Action Plan";
            blank_issue.Content = "[Add instructions and video here]";

            // Initialize actionplan references
            door_knob_loose = new HomeIssue();
            door_off_track = new HomeIssue();
            door_handle_broken = new HomeIssue();
            sewer = new HomeIssue();
            sump = new HomeIssue();
            water_heater_no_gas_electric = new HomeIssue();
            water_heater_yes_gas_electric = new HomeIssue();
            thermostat = new HomeIssue();
            hvac_electric = new HomeIssue();
            furnace_filter = new HomeIssue();

            // Initialize tree to default value, will be replaced when content is loaded
            tree = new Models.DecisionTree("In which area of the home is the problem occuring?");

            // Initialize category collection to default value, will be replaced when content is loaded
            categories = new ObservableCollection<Models.Category>();
        }
      
        /*
         * Listener for "Ask The Homeowner Buddy" button
         */
        private void Diagnose_Button_Clicked(object sender, EventArgs e) {
            if (!loadingActionPlanContent) { //Launch the questionnaire page if content is done loading                
                Navigation.PushAsync(new HabitatBuddy.Views.QuestionnairePage(tree));
            }
        }

        /*
         * Listener for "Register App" button
        */
        private void appRegistered(object sender, EventArgs e)
        {
            if (!loadingActionPlanContent)
            { //Launch the questionnaire page if content is done loading                
                Navigation.PushAsync(new HabitatBuddy.Views.RegistrationPage(true));
            }
        }



        /*
         * Listener for category view button
         */
        private void Category_Button_Clicked(object sender, EventArgs e) {
            if (!loadingCategoryContent) { //Launch the category page if content is done loading
                Navigation.PushAsync(new HabitatBuddy.Views.CategoryPage(categories));
            }
        }

        /*
         * Listener for check all isues button (disable this in final release, this is for debugging use)
         */
        private void AllIssueButton(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TodoREST.Views.IssueListPage());
        }

        /*
         * Listener for maintnenace reminder button
         */
        private void Reminder_Button_Clicked(object sender, EventArgs e) {
            if (!loadingReminderContent) { //Launch the maintenance page if content is done loading                
                Navigation.PushAsync(new HabitatBuddy.Views.MaintenancePage(reminders));
            }

        }



        // Called when main page appears, reloads all content from the database in case of any changes
        protected async override void OnAppearing() {
            base.OnAppearing();
            LoadingLabel.IsVisible = true;
            MainLabel.IsVisible = false;
            // reset flags to true since content is now loading
            loadingActionPlanContent = true;
            loadingCategoryContent = true;
            loadingReminderContent = true;
            AskButton.IsEnabled = false;
            CategoryButton.IsEnabled = false;
            ReminderButton.IsEnabled = false;

            // Get registration info and set display (locally)
            App.getRegistrationInfo();
            if (App.isReg == false)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    MainLabel.Text = "Welcome to the Homeowner Buddy";
                    Address.Text = "Address: (unregistered)";
                });
            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    MainLabel.Text = "Welcome to the Homeowner Buddy, " + App.regName + "!";
                    Address.Text = "Address: " + App.regAddress;
                });
            }

            // Check to see if application is online and alter view accordingly
            if (App.isOnline)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Lbl_NoInternet.IsVisible = false;
                });

            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Lbl_NoInternet.IsVisible = true;
                    Lbl_NoInternet.Text = "Internet not connected! Some features may be disabled!";
                });
            }

            // pull all issues (action plans) from the database
            ListView issueList = new ListView();
            Console.WriteLine("Fetching Action Plans...");
            issueList.ItemsSource = await App.IssueManager.GetTasksAsync();
            Console.WriteLine("Received Action Plans.");

            // wipe categories and replace with new collection
            categories = new ObservableCollection<Models.Category>();

            // Iterate through all action plans, and link up to decision tree and populate categories
            foreach (HomeIssue issue in issueList.ItemsSource) {
                if (issue.ActionPlanId == 10) {
                    door_knob_loose = issue;
                } else if (issue.ActionPlanId == 20) {
                    door_off_track = issue;
                } else if (issue.ActionPlanId == 30) {
                    door_handle_broken = issue;
                } else if (issue.ActionPlanId == 40) {
                    sewer = issue;
                } else if (issue.ActionPlanId == 50) {
                    sump = issue;
                } else if (issue.ActionPlanId == 60) {
                    water_heater_no_gas_electric = issue;
                } else if (issue.ActionPlanId == 70) {
                    water_heater_yes_gas_electric = issue;
                } else if (issue.ActionPlanId == 80) {
                    thermostat = issue;
                } else if (issue.ActionPlanId == 90) {
                    hvac_electric = issue;
                } else if (issue.ActionPlanId == 100) {
                    furnace_filter = issue;
                } else if (issue.ActionPlanId == 110) {
                    furnace_lights = issue;
                }

                // Create a category for the new issue or place it into appropriate category
                bool newCategory = true;
                foreach (Models.Category c in categories) {
                    // If the issue's category matches a category in the list, add the action plan to that category's issue list
                    if (c.title.Equals(issue.Category)) {
                        newCategory = false;
                        c.issues.Add(issue);
                    }
                }

                // If category did not yet exist, create it and add the issue to its issue list
                if (newCategory) {
                    categories.Add(new Models.Category(issue.Category));
                    categories[categories.Count - 1].issues.Add(issue);
                }
            }

            // done loading category content
            loadingCategoryContent = false;
            CategoryButton.IsEnabled = true;

            // Populate the decision tree
            tree = new Models.DecisionTree("Which area of the home is the problem affecting?");
            tree.addChild("Where is the problem occuring in the kitchen?", "Kitchen", null, "kitchen70.png");
            tree.moveToChild(0);
            tree.addChild("", "Refrigerator", blank_issue, "no70.png");
            tree.addChild("What issue are you experiencing with the sink?", "Sink", null, "no70.png");
            tree.moveToChild(1);
            tree.addChild("", "No water", blank_issue, "no70.png");
            tree.addChild("", "No hot water", blank_issue, "no70.png");
            tree.addChild("", "Clogged/Water won't drain", blank_issue, "no70.png");
            tree.addChild("", "Other issue", blank_issue, "no70.png");
            tree.moveToParent();
            tree.addChild("", "Stove", blank_issue, "stoveoven.png");
            tree.addChild("What issue is occuring with the door?", "Door", null, "no70.png");
            tree.moveToChild(3);
            tree.addChild("", "Off of Track", door_off_track, "no70.png");
            tree.addChild("", "Other", blank_issue, "no70.png");
            tree.moveToParent();
            tree.moveToParent();
            tree.addChild("What's the problem in the bathroom?", "Bathroom", null, "bathroom70.png");
            tree.moveToChild(1);
            tree.addChild("What issue is occuring with the door?", "Door", null, "no70.png");
            tree.moveToChild(0);
            tree.addChild("", "Door Knob Loose", door_knob_loose, "no70.png");
            tree.addChild("", "Other", blank_issue, "no70.png");
            tree.moveToParent();
            tree.addChild("", "Bathroom Sink", blank_issue, "no70.png");
            tree.addChild("", "Shower", blank_issue, "no70.png");
            tree.moveToParent();
            tree.addChild("Where is the problem occuring in the bedroom?", "Bedroom", null, "bed70.png");
            tree.moveToChild(2);
            tree.addChild("What issue is occuring with the door?", "Door", null, "no70.png");
            tree.moveToChild(0);
            tree.addChild("", "Off of Track", door_off_track, "no70.png");
            tree.addChild("", "Door Knob Loose", door_knob_loose, "no70.png");
            tree.addChild("", "Other", blank_issue, "no70.png");
            tree.moveToParent();
            tree.moveToParent();
            tree.addChild("Where is the issue occuring in the living room or entry way?", "Living Room/Entry Way", null, "sofa70.png");
            tree.moveToChild(3);
            tree.addChild("What issue is occuring with the front door?", "Front Door", null, "no70.png");
            tree.moveToChild(0);
            tree.addChild("", "Handle Has Come Off of Door Knob", door_handle_broken, "no70.png");
            tree.addChild("", "Door Knob Loose", door_knob_loose, "no70.png");
            tree.addChild("", "Other", blank_issue, "no70.png");
            tree.moveToParent();
            tree.moveToParent();
            tree.addChild("What problem is occuring in the basement?", "Basement/Laundry", null, "laundry70.png");
            tree.moveToChild(4);
            tree.addChild("What issue is occuring with the door?", "Broken/Stuck Door", null, "no70.png");
            tree.moveToChild(0);
            tree.addChild("", "Off of Track", door_off_track, "no70.png");
            tree.addChild("", "Other", blank_issue, "no70.png");
            tree.moveToParent();
            tree.addChild("Is the Sump Pump plugged in and working?", "Flooding/Water on Floor", null, "no70.png");
            tree.moveToChild(1);
            tree.addChild("", "Yes", sewer, "no70.png");
            tree.addChild("", "No", sump, "no70.png");
            tree.addChild("", "Not Sure", sump, "no70.png");
            tree.moveToParent();
            tree.moveToParent();
            tree.addChild("What symptoms is the home experiencing?", "Whole Home", null, "wholehouse70.png");
            tree.moveToChild(5);
            tree.addChild("Is your thermostat (living room) lit up, and switched to Heat or A/C?", "No heat or no A/C", null, "no70.png");
            tree.moveToChild(0);
            tree.addChild("Is the furnace or A/C unit receiving electricity?", "Yes", null, "no70.png");
            tree.moveToChild(0);
            tree.addChild("Is the furnace filter excessively clogged and dirty?", "Yes", null, "no70.png");
            tree.moveToChild(0);
            tree.addChild("", "Yes", furnace_filter, "no70.png");
            tree.addChild("Are the lights on the furnace panel blinking in any sequence other than a slow green or slow red blinking?", "No", null, "no70.png");
            tree.moveToChild(1);
            tree.addChild("", "Yes", furnace_lights, "no70.png");
            tree.addChild("", "No", blank_issue, "no70.png");
            tree.addChild("", "Not sure", furnace_lights, "no70.png");
            tree.moveToParent();
            tree.addChild("", "Not sure", furnace_filter, "no70.png");
            tree.moveToParent();
            tree.addChild("", "No", hvac_electric, "no70.png");
            tree.addChild("", "Not sure", hvac_electric, "no70.png");
            tree.moveToParent();
            tree.addChild("", "No", thermostat, "no70.png");
            tree.addChild("", "Not sure", thermostat, "no70.png");
            tree.moveToParent();
            tree.addChild("Is the hot water tank receiving gas and electric?", "No hot water", null, "no70.png");
            tree.moveToChild(1);
            tree.addChild("", "Yes", water_heater_yes_gas_electric, "no70.png");
            tree.addChild("", "No", water_heater_no_gas_electric, "no70.png");
            tree.addChild("", "Not sure", water_heater_no_gas_electric, "no70.png");
            tree.moveToParent();
            tree.moveToParent();
            loadingActionPlanContent = false; //Set flag to false because tree is constructed, questionnaire page can now be launched
            AskButton.IsEnabled = true;

            // Load maintenance reminder content from the database
            ListView reminderList = new ListView();
            Console.WriteLine("Fetching Maintenance Reminders...");
            reminderList.ItemsSource = await App.mManager.GetTasksAsync();
            Console.WriteLine("Received Maintenance Reminders.");

            // wipe existing reminders and replace with empty collection
            reminders = new ObservableCollection<Models.MaintenanceItem>();



            int j = 3;//for TESTING DATES
            // Populate list of maintenance reminders
            foreach (Models.Maintenance reminder in reminderList.ItemsSource) {
                HomeIssue plan = blank_issue; //Use blank action plan in case no plan with matching ID is found.
                //Find the action plan that matches the reminder's action plan ID
                foreach (HomeIssue issue in issueList.ItemsSource) {
                    if (issue.ActionPlanId == reminder.ActionPlanId) {
                        plan = issue;
                    }
                }
                Models.MaintenanceItem newReminder = new Models.MaintenanceItem(reminder.Name, reminder.RecurrencePeriod, plan, reminder.homecode);
                newReminder.dueDate = new DateTime(2019, 4, 16 - j); //FOR TESTING DATES
                j -= 3;  //FOR TESTING DATES
                reminders.Add(newReminder);
            }

            // Done loading maintenance reminders
            loadingReminderContent = false;
            ReminderButton.IsEnabled = true;
            LoadingLabel.IsVisible = false;
            MainLabel.IsVisible = true;

            // Schedule push notifications for maintenance items that are due soon
            sendReminderPushNotifs();
        }
        public void sendReminderPushNotifs() {
            int id = 1;
            if(!loadingReminderContent) {
                foreach (Models.MaintenanceItem reminder in reminders) {
                    int days = (int)(reminder.dueDate - DateTime.Today).TotalDays;
                    if (days <= 7) { // Only give reminders for overdue maintenance, or due within 7 days
                        if (days < 0) {
                            days *= -1;
                            if (days == 1) { // use the word "day" or "days"
                                CrossLocalNotifications.Current.Show("Overdue - " + reminder.title, days + " day overdue. Complete as soon as possible!", id, DateTime.Now.AddSeconds(id));
                            } else {
                                CrossLocalNotifications.Current.Show("Overdue - " + reminder.title, days + " days overdue. Complete as soon as possible!", id, DateTime.Now.AddSeconds(id));
                            }
                        } else if (days == 0) {
                            CrossLocalNotifications.Current.Show("Reminder - " + reminder.title, "Complete maintenance activity today!", id, DateTime.Now.AddSeconds(id));
                        } else {
                            if (days == 1) { // use the word "day" or "days"
                                CrossLocalNotifications.Current.Show("Reminder - " + reminder.title, "Complete maintenance activity in " + days + " day.", id, DateTime.Now.AddSeconds(id));
                            } else {
                                CrossLocalNotifications.Current.Show("Reminder - " + reminder.title, "Complete maintenance activity in " + days + " days.", id, DateTime.Now.AddSeconds(id));
                            }
                        }
                    }
                    id++;
                }
            }
        }
    }
}