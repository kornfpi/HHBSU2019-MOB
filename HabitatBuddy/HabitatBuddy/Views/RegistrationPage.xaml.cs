using HabitatBuddy.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HabitatBuddy.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : ContentPage
    {

        // Path to local SQLite DB which contains registration info
        private string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "regDB.db3");

        public RegistrationPage() {         
            InitializeComponent();

            // Check to see if application is online and alter view accordingly
            if (App.isOnline)
            {
                Lbl_NoInternet.IsVisible = false;
                Lbl_HomeCode.IsVisible = true;
                Lbl_Username.IsVisible = true;
                Entry_Username.IsVisible = true;
                Entry_HomeCode.IsVisible = true;
                Btn_Register.IsVisible = true;
               
            }
            else
            {
                Lbl_NoInternet.IsVisible = true;
                Lbl_NoInternet.Text = "Internet not connected! Some features may be disabled!";
                Lbl_HomeCode.IsVisible = false;
                Lbl_Username.IsVisible = false;
                Entry_Username.IsVisible = false;
                Entry_HomeCode.IsVisible = false;
                Btn_Register.IsVisible = false;
            }


        }


        // Method will connect to SQLite DB and save registration info
        async void viewRegInfo(Object sender, EventArgs e)
        {

            var db = new SQLiteConnection(dbPath);
            db.DeleteAll<Registration>(); // Delete old registration
            db.CreateTable<Registration>();

            Registration newRegistration = new Registration()
            {
                Id = Entry_HomeCode.Text,
                Name = Entry_Username.Text,
                isConfirmed = false
            };

            db.Insert(newRegistration);
            await DisplayAlert("Success!", newRegistration.Name + "'s home information saved!", "Ok");
            // Navigation.PopAsync(); // Return to previous window

        }

        // Method will connect to SQLite DB and save registration info
        async void clearRegInfo(Object sender, EventArgs e)
        {

            var db = new SQLiteConnection(dbPath);
            db.DeleteAll<Registration>(); // Delete old registration
            db.CreateTable<Registration>();
            await DisplayAlert("Success!", "User information deleted!", "Ok");
            await Navigation.PopAsync(); // Return to previous window
            App.registeredName = "(Unregistered)";
            App.isReg = false;

        }

        // Method will connect to SQLite DB and retrieve registration info
        async void saveRegInfo(Object sender, EventArgs e)
        {
            if(!Entry_Username.Text.Equals("") && !Entry_HomeCode.Text.Equals("") && !Entry_Username.Text.Equals(null) && !Entry_HomeCode.Text.Equals(null))
            {
                var db = new SQLiteConnection(dbPath);
                db.CreateTable<Registration>();

                Registration newRegistration = new Registration()
                {
                    Id = Entry_HomeCode.Text,
                    Name = Entry_Username.Text,
                    isConfirmed = false
                };

                db.Insert(newRegistration);
                await DisplayAlert("Success!", newRegistration.Name + "'s home information saved!", "Ok");
                App.homecode = newRegistration.Id;
                //await Navigation.PopAsync(); // Return to previous window
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


    }


}