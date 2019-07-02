using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HabitatBuddy.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();

            // Create connection check timer in App.cs
            //App.CheckConnectivity_Timer();

            // Init();

            // Check to see if application is online and alter view accordingly
            if (App.isOnline)
            {
                Lbl_NoInternet.Text = "Internet Connected!";
                Lbl_Password.IsVisible = true;
                Lbl_Username.IsVisible = true;
                Entry_Username.IsVisible = true;
                Entry_Password.IsVisible = true;
                Btn_SignIn.IsVisible = true;
               
            }
            else
            {
                Lbl_NoInternet.Text = "Internet not connected. Please connect & refresh this page.";
                Lbl_Password.IsVisible = false;
                Lbl_Username.IsVisible = false;
                Entry_Username.IsVisible = false;
                Entry_Password.IsVisible = false;
                Btn_SignIn.IsVisible = false;
            }


        }

        void Init()
        {
            App.StartCheckIfInternet(Lbl_NoInternet, this);
        }

        async void SignInProcedure(Object sender, EventArgs e)
        {
            Models.User user = new Models.User(Entry_Username.Text, Entry_Password.Text);
            if (user.CheckInformation())
            {
                await DisplayAlert("Login", "Login Success", "Ok");
                //var result = await App.RestService.Login(user);
                //if (result.access_token != null)
                //{
                   // App.UserDatabase.SaveUser(user);
                //}
            }
            else
            {
                await DisplayAlert("Login", "Login Failure (Blank Field(s))", "Ok");
            }
        }
    }
}