using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HabitatBuddy.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InternetTestPage : ContentPage
    {

        // Timer to check for internet connection
        private static Timer timer;


        public InternetTestPage()
        {
            InitializeComponent();
            App.CheckConnectivity_Timer();
            if (App.isOnline)
            {
                ConnectivityLabel.IsVisible = false;
                StatusAlertOff.IsVisible = false;
                StatusAlertOn.IsVisible = true;
            }
            else
            {
                ConnectivityLabel.IsVisible = false;
                StatusAlertOff.IsVisible = true;
                StatusAlertOn.IsVisible = false;
            }



        }

        public static void CheckConnectivity_Timer(Label label, Page page)
        {
            //labelScreen = label;
            //label.Text = Models.Constants.NoInternetText;
            label.IsVisible = true;
            //hasInternet = true;
            //currentpage = page;
            if (timer == null)
            {
                // Changes here to CheckIfInternet type call (originally CheckIfInternetOvertime()
                // Lambda function not originally async
                timer = new Timer((e) =>
                {
                   CheckConnectivity(label);
                }, null, 10, (int)TimeSpan.FromSeconds(3).TotalMilliseconds);
            }
        }

        public static void CheckConnectivity(Label label)
        {
            var isConnected = CrossConnectivity.Current.IsConnected;
            if(isConnected == true)
            {
                App.isOnline = true;
            }
            else
            {
                App.isOnline = false;
            }
        }


    }
}