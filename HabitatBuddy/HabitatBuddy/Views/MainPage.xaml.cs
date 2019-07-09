using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using TodoREST;
using Xamarin.Forms;
using HabitatBuddy;
using Plugin.LocalNotifications;
using HabitatBuddy.Models;

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
        //HomeIssue blank_issue, door_knob_loose, door_off_track, door_handle_broken, sewer, sump, water_heater_no_gas_electric, water_heater_yes_gas_electric, thermostat, hvac_electric, furnace_filter, furnace_lights;
        HomeIssue blank_issue, concrete, siding, downspouts, lawn, flooring, sinks_1, sinks_2, sinks_3, toilets_1, toilets_2,
            toilets_3, bath, sump, closet, doors, drywall;


        public MainPage() {
            InitializeComponent();

            // Create internet connection checker
            App.CheckConnectivity_Timer();

            // Get registration info
            App.GetRegistrationInfo();

            if (App.registeredName.Equals("Unregistered"))
            {
                MainLabel.Text = "Welcome to the Homeowner Buddy (Unregistered)";
            }
            else
            {
                MainLabel.Text = "Welcome to the Homeowner Buddy, " + App.registeredName + "!";
            }

            // Check to see if application is online and alter view accordingly
            if (App.isOnline)
            {
                Lbl_NoInternet.IsVisible = false;


            }
            else
            {
                Lbl_NoInternet.IsVisible = true;
                Lbl_NoInternet.Text = "Internet not connected! Some features may be disabled!";

            }




            // Construct a blank action plan, to be used when no action plan is available.
            blank_issue = new HomeIssue();
            blank_issue.Title = "Blank Action Plan";
            blank_issue.Content = "[Add instructions and video here]";

            // Initialize actionplan references
            concrete = new HomeIssue();
            siding = new HomeIssue();
            siding = new HomeIssue();
            downspouts = new HomeIssue();
            lawn = new HomeIssue();
            flooring = new HomeIssue();
            sinks_1 = new HomeIssue();
            sinks_2 = new HomeIssue();
            sinks_3 = new HomeIssue();
            toilets_1 = new HomeIssue();
            toilets_2 = new HomeIssue();
            toilets_3 = new HomeIssue();
            bath = new HomeIssue();
            sump = new HomeIssue();
            closet = new HomeIssue();
            doors = new HomeIssue();
            drywall = new HomeIssue();

            //door_knob_loose = new HomeIssue();
            //siding = new HomeIssue();
            //door_off_track = new HomeIssue();
            //door_handle_broken = new HomeIssue();
            //sewer = new HomeIssue();
            //sump = new HomeIssue();
            //water_heater_no_gas_electric = new HomeIssue();
            //water_heater_yes_gas_electric = new HomeIssue();
            //thermostat = new HomeIssue();
            //hvac_electric = new HomeIssue();
            //furnace_filter = new HomeIssue();

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
                Navigation.PushAsync(new HabitatBuddy.Views.RegistrationPage());
            }
        }


        /*
        * Listener for "Register App" button
        */
        private void checkConn(object sender, EventArgs e)
        {
            if (!loadingActionPlanContent)
            { //Launch the questionnaire page if content is done loading                
                Navigation.PushAsync(new HabitatBuddy.Views.InternetTestPage());
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
            App.GetRegistrationInfo();

            if (App.registeredName.Equals("Unregistered"))
            {
                MainLabel.Text = "Welcome to the Homeowner Buddy (Unregistered)";
            }
            else
            {
                MainLabel.Text = "Welcome to the Homeowner Buddy, " + App.registeredName + "!";
            }

            // Check to see if application is online and alter view accordingly
            if (App.isOnline)
            {
                Lbl_NoInternet.IsVisible = false;
            }
            else
            {
                Lbl_NoInternet.IsVisible = true;
                Lbl_NoInternet.Text = "Internet not connected! Some features may be disabled!";
            }

            // Get list of all registrations (From remote database)
            ListView registrationList = new ListView();
            Console.WriteLine("Fetching registration info..."); // debug
            registrationList.ItemsSource = await App.HomeRegInfo.RefreshDataAsync();
            Console.WriteLine("Received registration info."); // debug
            HomeRegInfo thisHomeReg = new HomeRegInfo();
            foreach (HomeRegInfo regInfo in registrationList.ItemsSource) {
                Console.WriteLine("---------------------------------------------------------------------------------" + regInfo.homeNumber);
                if (App.homecode.Equals(regInfo.homeNumber))
                {
                    thisHomeReg = regInfo;
                    Console.WriteLine("--------------------------------------------------------------------------------- REGISTERED" );
                }
                else
                {
                    Console.WriteLine("--------------------------------------------------------------------------------- CODE NOT FOUND");
                }
            }
            HomeRegInfo testAddition = new HomeRegInfo();
            testAddition.homeNumber = "12345678910";
            testAddition.registeredTo = "Johnny 2 Bad";
            await App.HomeRegInfo.SaveTodoItemAsync(testAddition, true);



            // pull all issues (action plans) from the database
            ListView issueList = new ListView();
            Console.WriteLine("Fetching Action Plans...");
            issueList.ItemsSource = await App.IssueManager.GetTasksAsync();
            Console.WriteLine("Received Action Plans.");


            // wipe categories and replace with new collection
            categories = new ObservableCollection<Models.Category>();

            // Iterate through all action plans, and link up to decision tree and populate categories
            foreach (HomeIssue issue in issueList.ItemsSource) {
                if (issue.ActionPlanId == 170) {
                    concrete = issue;
                } else if (issue.ActionPlanId == 20) {
                    siding = issue;
                } else if (issue.ActionPlanId == 30) {
                    downspouts = issue;
                } else if (issue.ActionPlanId == 40) {
                    lawn = issue;
                } else if (issue.ActionPlanId == 50) {
                    flooring = issue;
                } else if (issue.ActionPlanId == 60) {
                    sinks_1 = issue;
                } else if (issue.ActionPlanId == 70) {
                    sinks_2 = issue;
                } else if (issue.ActionPlanId == 80) {
                    sinks_3 = issue;
                } else if (issue.ActionPlanId == 90) {
                    toilets_1 = issue;
                } else if (issue.ActionPlanId == 100) {
                    toilets_2 = issue;
                } else if (issue.ActionPlanId == 110) {
                    toilets_3 = issue;
                } else if (issue.ActionPlanId == 120) {
                    bath = issue;
                } else if (issue.ActionPlanId == 130) {
                    sump = issue;
                } else if (issue.ActionPlanId == 140) {
                    closet = issue;
                } else if (issue.ActionPlanId == 150) {
                    doors = issue;
                } else if (issue.ActionPlanId == 160) {
                    drywall = issue;
                }
                //else {
                //    blank_issue = issue;
                //}

                // Create a category for the new issue or place it into appropriate category
                bool newCategory = true;
                foreach (Models.Category c in categories) {
                    // If the issue's category matches a category in the list, add the action plan to that category's issue list
                    if (c.title.Equals(issue.Category)) {
                        if(issue.ActionPlanId >= 20 && issue.ActionPlanId <=170)
                        {
                            newCategory = false;
                            c.issues.Add(issue);
                        }
                        
                    }
                }

                // If category did not yet exist, create it and add the issue to its issue list
                if (newCategory) {
                    if (issue.ActionPlanId >= 20 && issue.ActionPlanId <= 170)
                    {
                        categories.Add(new Models.Category(issue.Category));
                        categories[categories.Count - 1].issues.Add(issue);
                    }
                }
            }



            setCategoryTitles();


            // done loading category content
            loadingCategoryContent = false;
            CategoryButton.IsEnabled = true;

            tree = new Models.DecisionTree("Which area of the home is the problem affecting?");

            //*********************************START EXTERIOR*********************************
            tree.addChild("What Location of the Exterior is the Problem Affecting?", "Exterior", null, "house_outside.png");
            tree.moveToChild(0);
            tree.addChild("What issue is occuring with the Outside Walls of House?", "Outside Walls of House", null, "siding.png");
            tree.moveToChild(0);
            tree.addChild("", "Hole in Siding/Piece of Siding Loose", siding, "siding_broke.png");
            tree.moveToParent();
            tree.addChild("What issue is occuring with the Driveway?", "Driveway", null, "driveway_byhouse.png");
            tree.moveToChild(1);
            tree.addChild("", "Cracks in Concrete 1/4\" or greater ", concrete, "outside_concrete_cracked_wtext.png");
            tree.moveToParent();
            tree.addChild("What issue is occuring with the Corners of Home?", "At Several Outside Corners of Home", null, "downspout.png");
            tree.moveToChild(2);
            tree.addChild("", "Drain Line has Sunk / Seperated from the Downspout", downspouts, "downspout_broken.png");
            tree.moveToParent();

            tree.addChild("What issue is occuring with the Yard?", "Yard", null, "grass.png");
            tree.moveToChild(3);
            tree.addChild("", "Bare Spots in Lawn", lawn, "grass_bare_arrow.png");
            tree.moveToParent();

            tree.addChild("What issue is occuring with the Porch?", "Porch", null, "porch.png");
            tree.moveToChild(4);
            tree.addChild("", "Cracks in Concrete 1/4\" or greater ", concrete, "outside_concrete_cracked_wtext.png");
            tree.moveToParent();

            tree.addChild("What issue is occuring with the Steps?", "Steps", null, "no70.png");
            tree.moveToChild(5);
            tree.addChild("", "Cracks in Concrete 1/4\" or greater ", concrete, "outside_concrete_cracked_wtext.png");
            tree.moveToParent();

            tree.addChild("What issue is occuring with the Sidewalk?", "Sidewalk", null, "outside_concrete.png");
            tree.moveToChild(6);
            tree.addChild("", "Cracks in Concrete 1/4\" or greater ", concrete, "outside_concrete_cracked_wtext.png");
            tree.moveToParent();

            tree.addChild("What issue is occuring with the Basement Wall?", "Basement Wall", null, "basement_wall.png");
            tree.moveToChild(7);
            tree.addChild("", "Cracks in Concrete 1/4\" or greater ", concrete, "outside_concrete_cracked_wtext.png");
            tree.moveToParent();
            //*********************************END EXTERIOR*********************************


            //*********************************START INTERIOR*********************************
            tree.moveToParent();
            tree.addChild("What Location of the Interior is the Problem Affecting?", "Interior", null, "house_inside.png");
            tree.moveToChild(1);
            tree.addChild("What issue is occuring with the Bathroom?", "Bathroom", null, "bathroom450.png");
            tree.moveToChild(0);
            tree.addChild("", "Sink Stopper Doesn't Work", sinks_3, "bathroomsink_stopper.png");
            tree.addChild("", "Toilet Clogged or Won't Flush", toilets_1, "toilet_clogged.png");
            tree.addChild("", "Bathub/Shower Not Draining", bath, "bathtub_overflow.png");
            tree.addChild("", "Sink Drain Pipes Leaking", sinks_1, "kitchensink_leakypipes.png");
            tree.addChild("", "Toilet Leaking at Floor", toilets_2, "toilet_floorwater.png");
            tree.addChild("", "Toilet Water Running Continuously",toilets_3, "toilet_endlessflushing.png");

            tree.moveToParent();
            tree.addChild("What issue is occuring with the Kitchen?", "Kitchen", null, "kitchen.png");
            tree.moveToChild(1);
            tree.addChild("", "Garbage Disposal Not Working", sinks_2, "kitchensink_garbagedisposal.png");
            tree.addChild("", "Sink Drain Pipes Leaking", sinks_1, "kitchensink_leakypipes.png");
            tree.addChild("", "Damaged or Loose Piece of Flooring", flooring, "floor_cracked.png");
            tree.moveToParent();

            tree.addChild("What issue is occuring with the Bedroom?", "Bedroom", null, "bed.png");
            tree.moveToChild(2);
            tree.addChild("", "Wire Shelving or Brackets have come off Wall ", closet, "closet_shelfoffwall.png") ;
            tree.addChild("", "Bypass Doors Off-Track, Won't Slide, or Floor Guide", doors, "closet_doorhelp.png");
            tree.addChild("", "Damaged or Loose Piece of Flooring", flooring, "floor_cracked.png");
            tree.moveToParent();

            tree.addChild("What issue is occuring with the Interior Walls of House?", "Throughout Interior Walls of House", null, "wall.png");
            tree.moveToChild(3);
            tree.addChild("", "Nail or Screw Pops", drywall, "wall_nails.png");
            tree.moveToParent();

            tree.addChild("What issue is occuring with the Laundry/Linen?", "Laundry/Linen Closet", null, "laundry.png");
            tree.moveToChild(4);
            tree.addChild("", "Wire Shelving or Brackets have come off Wall ", closet, "closet_shelfoffwall.png");
            tree.moveToParent();

            tree.addChild("What issue is occuring with the Pantry?", "Pantry", null, "no70.png");
            tree.moveToChild(5);
            tree.addChild("", "Wire Shelving or Brackets have come off Wall ", closet, "closet_shelfoffwall.png");
            tree.moveToParent();

            tree.addChild("What issue is occuring with the Halls?", "Halls", null, "hall.png");
            tree.moveToChild(6);
            tree.addChild("", "Damaged or Loose Piece of Flooring", flooring, "floor_cracked.png");
            tree.moveToParent();

            tree.addChild("What issue is occuring with the Living Room?", "Living Room", null, "sofa.png");
            tree.moveToChild(7);
            tree.addChild("", "Damaged or Loose Piece of Flooring", flooring, "floor_cracked.png");
            tree.moveToParent();

            tree.addChild("What issue is occuring with the Basement?", "Basement", null, "basement.png");
            tree.moveToChild(8);
            tree.addChild("", "Sump Pump not Working / Basement Wet", sump, "sumppump_floorwater.png");
            tree.moveToParent();

            //tree.addChild("What issue is occurring with Test 10", "Test", null, "no70.png");
            //tree.moveToChild(9);
            //tree.addChild("", "Test for Button 10", blank_issue, "no70.png");
            //tree.moveToParent();

            //*********************************END INTERIOR*********************************


            tree.moveToParent();


            //// Populate the decision tree
            //tree = new Models.DecisionTree("Which area of the home is the problem affecting?");
            //tree.addChild("Where is the problem occuring in the kitchen?", "Kitchen", null, "kitchen70.png");
            //tree.moveToChild(0);
            //tree.addChild("", "Refrigerator", blank_issue, "no70.png");
            //tree.addChild("What issue are you experiencing with the sink?", "Sink", null, "no70.png");
            //tree.moveToChild(1);
            //tree.addChild("", "No water", blank_issue, "no70.png");
            //tree.addChild("", "No hot water", blank_issue, "no70.png");
            //tree.addChild("", "Clogged/Water won't drain", blank_issue, "no70.png");
            //tree.addChild("", "Other issue", blank_issue, "no70.png");
            //tree.moveToParent();
            //tree.addChild("", "Stove", blank_issue, "stoveoven.png");
            //tree.addChild("What issue is occuring with the door?", "Door", null, "no70.png");
            //tree.moveToChild(3);
            //tree.addChild("", "Off of Track", door_off_track, "no70.png");
            //tree.addChild("", "Other", blank_issue, "no70.png");
            //tree.moveToParent();
            //tree.moveToParent();
            //tree.addChild("What's the problem in the bathroom?", "Bathroom", null, "bathroom70.png");
            //tree.moveToChild(1);
            //tree.addChild("What issue is occuring with the door?", "Door", null, "no70.png");
            //tree.moveToChild(0);
            //tree.addChild("", "Door Knob Loose", door_knob_loose, "no70.png");
            //tree.addChild("", "Other", blank_issue, "no70.png");
            //tree.moveToParent();
            //tree.addChild("", "Bathroom Sink", blank_issue, "no70.png");
            //tree.addChild("", "Shower", blank_issue, "no70.png");
            //tree.moveToParent();
            //tree.addChild("Where is the problem occuring in the bedroom?", "Bedroom", null, "bed70.png");
            //tree.moveToChild(2);
            //tree.addChild("What issue is occuring with the door?", "Door", null, "no70.png");
            //tree.moveToChild(0);
            //tree.addChild("", "Off of Track", door_off_track, "no70.png");
            //tree.addChild("", "Door Knob Loose", door_knob_loose, "no70.png");
            //tree.addChild("", "Other", blank_issue, "no70.png");
            //tree.moveToParent();
            //tree.moveToParent();
            //tree.addChild("Where is the issue occuring in the living room or entry way?", "Living Room/Entry Way", null, "sofa70.png");
            //tree.moveToChild(3);
            //tree.addChild("What issue is occuring with the front door?", "Front Door", null, "no70.png");
            //tree.moveToChild(0);
            //tree.addChild("", "Handle Has Come Off of Door Knob", door_handle_broken, "no70.png");
            //tree.addChild("", "Door Knob Loose", door_knob_loose, "no70.png");
            //tree.addChild("", "Other", blank_issue, "no70.png");
            //tree.moveToParent();
            //tree.moveToParent();
            //tree.addChild("What problem is occuring in the basement?", "Basement/Laundry", null, "laundry70.png");
            //tree.moveToChild(4);
            //tree.addChild("What issue is occuring with the door?", "Broken/Stuck Door", null, "no70.png");
            //tree.moveToChild(0);
            //tree.addChild("", "Off of Track", door_off_track, "no70.png");
            //tree.addChild("", "Other", blank_issue, "no70.png");
            //tree.moveToParent();
            //tree.addChild("Is the Sump Pump plugged in and working?", "Flooding/Water on Floor", null, "no70.png");
            //tree.moveToChild(1);
            //tree.addChild("", "Yes", sewer, "no70.png");
            //tree.addChild("", "No", sump, "no70.png");
            //tree.addChild("", "Not Sure", sump, "no70.png");
            //tree.moveToParent();
            //tree.moveToParent();
            //tree.addChild("What symptoms is the home experiencing?", "Whole Home", null, "wholehouse70.png");
            //tree.moveToChild(5);
            //tree.addChild("Is your thermostat (living room) lit up, and switched to Heat or A/C?", "No heat or no A/C", null, "no70.png");
            //tree.moveToChild(0);
            //tree.addChild("Is the furnace or A/C unit receiving electricity?", "Yes", null, "no70.png");
            //tree.moveToChild(0);
            //tree.addChild("Is the furnace filter excessively clogged and dirty?", "Yes", null, "no70.png");
            //tree.moveToChild(0);
            //tree.addChild("", "Yes", furnace_filter, "no70.png");
            //tree.addChild("Are the lights on the furnace panel blinking in any sequence other than a slow green or slow red blinking?", "No", null, "no70.png");
            //tree.moveToChild(1);
            //tree.addChild("", "Yes", furnace_lights, "no70.png");
            //tree.addChild("", "No", blank_issue, "no70.png");
            //tree.addChild("", "Not sure", furnace_lights, "no70.png");
            //tree.moveToParent();
            //tree.addChild("", "Not sure", furnace_filter, "no70.png");
            //tree.moveToParent();
            //tree.addChild("", "No", hvac_electric, "no70.png");
            //tree.addChild("", "Not sure", hvac_electric, "no70.png");
            //tree.moveToParent();
            //tree.addChild("", "No", thermostat, "no70.png");
            //tree.addChild("", "Not sure", thermostat, "no70.png");
            //tree.moveToParent();
            //tree.addChild("Is the hot water tank receiving gas and electric?", "No hot water", null, "no70.png");
            //tree.moveToChild(1);
            //tree.addChild("", "Yes", water_heater_yes_gas_electric, "no70.png");
            //tree.addChild("", "No", water_heater_no_gas_electric, "no70.png");
            //tree.addChild("", "Not sure", water_heater_no_gas_electric, "no70.png");
            //tree.moveToParent();
            //tree.moveToParent();
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

        private void setCategoryTitles()
        {
            //Siding = 20 | ID: 0
            foreach(Models.Category cat in categories) {
                String switchCase = cat.title;

                switch (switchCase)
                {
                    case "0":
                        cat.title = "Siding";
                        break;
                    case "1":
                        cat.title = "Concrete";
                        break;
                    case "2":
                        cat.title = "Downspouts";
                        break;
                    case "3":
                        cat.title = "Lawn";
                        break;
                    case "4":
                        cat.title = "Flooring";
                        break;
                    case "5":
                        cat.title = "Sinks";
                        break;
                    case "6":
                        cat.title = "Toilets";
                        break;
                    case "7":
                        cat.title = "Bathtubs & Showers";
                        break;
                    case "8":
                        cat.title = "Sump Pump";
                        break;
                    case "9":
                        cat.title = "Shelves";
                        break;
                    case "10":
                        cat.title = "Doors";
                        break;
                    case "11":
                        cat.title = "Drywall";
                        break;


                    default:
                        Console.WriteLine("Default case");
                        break;
                }
            }
            //Concrete = 10 | ID: 1
            //Downspouts = 30 | ID: 2
            //Lawn = 40 | ID: 3
            //Flooring = 50 | ID: 4
            //Sinks = 60 - 80 | ID: 5
            //Toilets = 90 - 110 | ID: 6
            //Bath = 120 | ID: 7
            //Sump = 130 | ID: 8
            //Closet = 140 | ID: 9
            //Doors = 150 | ID: 10
            //Drywall = 160 | ID: 11

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