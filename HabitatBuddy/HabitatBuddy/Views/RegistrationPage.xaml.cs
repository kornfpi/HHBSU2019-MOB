// This view handles user app registration, and is the first to be placed on the view stack on startup.

using HabitatBuddy.Models;
using HabitatBuddy.Services;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HabitatBuddy.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : ContentPage
    {

        // For checking if this page is on startup or called from main
        private static bool appStarted;

        // For catching the recovered database address
        private static string recievedAddress;

        // For regular internet connectivity view modifications
        private static Timer netViewTimer;

        // Path to local SQLite DB which contains registration info
        private string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "regDB.db3");

        public RegistrationPage(Boolean status = false) {

            // Component must be initialized
            InitializeComponent();

            appStarted = status; // For checking on start or app running

            // Start timer to update views based on connectivity
            initializeNetViewTimer();

            // Check registration of app
            App.getRegistrationInfo();
            if (App.isReg == false) // Unregistered App
            {
                // Display registration prompt
                Device.BeginInvokeOnMainThread(() => {
                    Reg_Prompt.IsVisible = true;
                    Reg_Prompt.Text = "Unregistered App, Please Register!";
                    Lbl_HomeCode.IsVisible = true;
                    Entry_HomeCode.IsVisible = true;
                    Lbl_User_First_Name.IsVisible = true;
                    Entry_User_First_Name.IsVisible = true;
                    Btn_Register.IsVisible = true;
                    Btn_Continue.IsVisible = true;
                    Btn_Clear.IsVisible = false; // No reg to clear
                    Btn_Show.IsVisible = false; // No reg to display
                });
                // Check to see if application is online and alter view accordingly
                setNetViews();
            }
            else // App already registered, pop registration
            {
                if (!appStarted)
                {
                    Navigation.PushAsync(new HabitatBuddy.MainPage());
                }
                else
                {
                    setNetViews();
                    setRegViews();
                }
            }
        }

        // What happens to page on appearing
        protected override void OnAppearing()
        {
            setNetViews();
            if(appStarted) setRegViews();
        }

        // This method checks App class to see if app is online, and changes views accordingly
        public void setNetViews()
        {
            if (App.isOnline)
            {
                Device.BeginInvokeOnMainThread(() => {

                    Lbl_NoInternet.IsVisible = false;
                    Lbl_HomeCode.IsVisible = true;
                    Lbl_User_First_Name.IsVisible = true;
                    Entry_User_First_Name.IsVisible = true;
                    Lbl_User_Last_Name.IsVisible = true;
                    Entry_User_Last_Name.IsVisible = true;
                    Entry_HomeCode.IsVisible = true;
                    Btn_Register.IsVisible = true;
                });
            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Lbl_NoInternet.IsVisible = true;
                    Lbl_NoInternet.Text = "Internet not connected, some features may be disabled!";
                    Lbl_HomeCode.IsVisible = false;
                    Lbl_User_First_Name.IsVisible = false;
                    Entry_User_First_Name.IsVisible = false;
                    Lbl_User_Last_Name.IsVisible = false;
                    Entry_User_Last_Name.IsVisible = false;
                    Entry_HomeCode.IsVisible = false;
                    Btn_Register.IsVisible = false;
                });
            }
        }

        // If app already registered, don't display extra fields
        public void setRegViews()
        {
            if (App.isReg)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Lbl_HomeCode.IsVisible = false;
                    Lbl_User_First_Name.IsVisible = false;
                    Entry_User_First_Name.IsVisible = false;
                    Lbl_User_Last_Name.IsVisible = false;
                    Entry_User_Last_Name.IsVisible = false;
                    Entry_HomeCode.IsVisible = false;
                    Btn_Register.IsVisible = false;
                    Btn_Continue.IsVisible = false;
                    Btn_Clear.IsVisible = true;
                });
            }
            else
            {
                // Display registration prompt
                Device.BeginInvokeOnMainThread(() => {
                    Reg_Prompt.IsVisible = true;
                    Reg_Prompt.Text = "Unregistered App, Please Register!";
                    Lbl_HomeCode.IsVisible = true;
                    Entry_HomeCode.IsVisible = true;
                    Lbl_User_First_Name.IsVisible = true;
                    Entry_User_First_Name.IsVisible = true;
                    Btn_Register.IsVisible = true;
                    Btn_Continue.IsVisible = true;
                    Btn_Clear.IsVisible = false; // No reg to clear
                    Btn_Show.IsVisible = false; // No reg to display
                });
            }
        }

        // Method will connect to SQLite DB and save registration info
        async void clearRegInfo(Object sender, EventArgs e)
        {
            checkHomeNum(App.regHomecode, true); // Clears registeredTo in database
            var db = new SQLiteConnection(dbPath);
            db.DeleteAll<Registration>(); // Delete old registration
            db.CreateTable<Registration>();
            await DisplayAlert("Success!", "User information deleted, App will now shut down.", "Ok");
            // Close app
            Device.BeginInvokeOnMainThread(() =>
            {
                var closer = DependencyService.Get<ICloseApp>();
                closer?.closeApplication();
            });
        }

        // Method will connect to SQLite DB and retrieve registration info
        async void saveRegInfo(Object sender, EventArgs e)
        {
            // Check data entry
            bool entriesExist = true;
            String[] entries = { Entry_HomeCode.Text, Entry_User_First_Name.Text, Entry_User_Last_Name.Text };
            foreach (String s in entries)
            {
                if (s == null || s.Equals(""))
                {
                    entriesExist = false;
                    break;
                }
            }

            // Check that entered home number works
            bool regExists = await checkHomeNum(Entry_HomeCode.Text);

            if (entriesExist && regExists)
            {
                var db = new SQLiteConnection(dbPath);
                db.CreateTable<Registration>();
                Registration newRegistration = new Registration()
                {
                    Id = Entry_HomeCode.Text,
                    Name = Entry_User_First_Name.Text,
                    isConfirmed = true,
                    Address = recievedAddress
                };
                db.Insert(newRegistration);
                await DisplayAlert("Success!", newRegistration.Name + "'s home information saved!", "Ok");
                await Navigation.PopAsync(); // Return to previous window
                App.getRegistrationInfo(); // Refresh registration info
                await Navigation.PushAsync(new HabitatBuddy.MainPage()); // Go to main app
            }
            else
            {
                await DisplayAlert("Registration Failure!", "Please ensure name and house number are entered correctly!", "Ok");
            }
        }

        /*
         * Listener for "Display Registration Info" button
         */
        private void dispRegInfo(object sender, EventArgs e)
        {
            Navigation.PushAsync(new HabitatBuddy.Views.RegistrationInfoPage());
        }

        /*
         * Listener for "Continue Without Registration" button
         */
        private void continue_noreg(object sender, EventArgs e)
        {
            Navigation.PushAsync(new HabitatBuddy.MainPage());
        }

        // Creates timer which regularly sets views based on internet connection
        private void initializeNetViewTimer()
        {
            if (netViewTimer == null)
            {
                // Create timer which calls connection checking method regularly
                netViewTimer = new Timer((e) =>
                {
                    setNetViews();
                }, null, 10, (int)TimeSpan.FromSeconds(3).TotalMilliseconds);
            }
        }

        // Check home number entry against web server to ensure authenticity
        private async Task<bool> checkHomeNum(string entry, Boolean isClear = false)
        {
            // Get list of all registrations (From remote database)
            ListView registrationList = new ListView();
            registrationList.ItemsSource = await App.HomeRegInfo.RefreshDataAsync();
            bool regExists = false;
            while (!regExists)
            {
                if (entry == (null)) break;
                foreach (HomeRegInfo regInfo in registrationList.ItemsSource)
                {
                    if (entry.Equals(regInfo.homeNumber))
                    {
                        if (regInfo.registeredTo != null && !regInfo.registeredTo.Equals(" ") && isClear == false) break;
                        regExists = true;
                        string currentName = regInfo.registeredTo;
                        regInfo.registeredTo = isClear ? null : Entry_User_First_Name.Text + " " + Entry_User_Last_Name.Text;
                        await App.HomeRegInfo.SaveTodoItemAsync(regInfo, false);
                        recievedAddress = regInfo.streetAddress;

                        // Save log info
                        Log newLog = new Log();
                        newLog.HomeNumber = regInfo.homeNumber;
                        newLog.HomeOwner = isClear? currentName : Entry_User_First_Name.Text + " " + Entry_User_Last_Name.Text;
                        newLog.Action = isClear? "Removed application registration to HomeNumber." : "Registered application to HomeNumber.";
                        newLog.Date = DateTime.Now.ToString();
                        await App.LogManager.SaveTodoItemAsync(newLog, false);
                    }
                }
                break;
            }
            return regExists;
        }

    }

}