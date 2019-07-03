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
    public partial class RegistrationInfoPage : ContentPage
    {

        // Path to local SQLite DB which contains registration info
        private string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "regDB.db3");

        public RegistrationInfoPage()
        {
            InitializeComponent();
            var db = new SQLiteConnection(dbPath);
            
            if (App.isReg == true) {
                var name = db.Table<Registration>().OrderBy(x => x.Name).FirstOrDefault().Name.ToString();
                var houseNum = db.Table<Registration>().OrderBy(x => x.Id).FirstOrDefault().Id.ToString();
                bool verified = db.Table<Registration>().OrderBy(x => x.isConfirmed).FirstOrDefault().isConfirmed;
                displayName.Text = "User Name: " + name;
                displayHouseNumber.Text = "House Number: " + houseNum;
                if (verified)
                {
                    displayVerified.Text = "Home Number: Confirmed";
                }
                else
                {
                    displayVerified.Text = "Home Number: Unconfirmed";
                }
                displayName.IsVisible = true;
                displayHouseNumber.IsVisible = true;
                displayVerified.IsVisible = true;
                query.IsVisible = false;
            }
            else
            {
                query.Text = "App Unregistered!";
            }
            
        }
    }
}