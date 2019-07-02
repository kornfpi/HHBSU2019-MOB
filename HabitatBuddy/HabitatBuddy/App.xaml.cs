using HabitatBuddy.Data;
using HabitatBuddy.Models;
using Plugin.Connectivity;
using SQLite;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TodoREST;
using TodoREST.Views;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace HabitatBuddy {
    public partial class App : Application {
        public static HomeIssueManager IssueManager { get; private set; }
        public static MaintenanceManager mManager { get; private set; }

        // For registration
        public static HomeRegInfoService HomeRegInfo { get; set; }


        static TokenDatabaseController tokenDatabase;
        static UserDatabaseController userDatabase;
        static RestService restService;
        private static Label labelScreen;
        private static bool hasInternet;
        private static Page currentpage;
        private static Timer timer;
        private static Timer newer_timer;
        private static bool noInterShow; // Toggle display of internet availablility
        public static string registeredName;


        public static string message = "Test Message";
        public static bool isOnline = false;

        public static bool isReg;
        public static string homecode;




        public App() {
            InitializeComponent();

            // Remove these for final release
            isReg = false;
            registeredName = "Unregistered";
            homecode = "8675310";
            // End remove

            noInterShow = false; // Set to display internet availability

            MainPage = new NavigationPage(new MainPage());
            MainPage.Title = "Homeowner Buddy";
            IssueManager = new HomeIssueManager(new IssueService());
            mManager = new MaintenanceManager(new MaintenanceService());
            HomeRegInfo = new HomeRegInfoService();


            //MainPage = new NavigationPage(new IssueListPage());
        }

        protected override void OnStart() {
            // Handle when your app starts
        }

        protected override void OnSleep() {
            // Handle when your app sleeps
        }

        protected override void OnResume() {
            // Handle when your app resumes
        }

        public static UserDatabaseController UserDatabase
        {
            get
            {
                if(userDatabase == null)
                {
                    userDatabase = new UserDatabaseController();
                }
                return userDatabase;
            }
        }

        public static TokenDatabaseController TokenDatabase
        {
            get
            {
                if (tokenDatabase == null)
                {
                    tokenDatabase = new TokenDatabaseController();
                }
                return tokenDatabase;
            }
        }

        public static RestService RestService
        {
            get
            {
                if(restService == null)
                {
                    restService = new RestService();
                }
                return restService;
            }
        }


        //--------- Internet Connection

        public static void StartCheckIfInternet(Label label, Page page)
        {
            labelScreen = label;
            label.Text = Models.Constants.NoInternetText;
            label.IsVisible = false;
            hasInternet = true;
            currentpage = page;
            if(timer == null)
            {
                // Changes here to CheckIfInternet type call (originally CheckIfInternetOvertime()
                // Lambda function not originally async
                timer = new Timer((e) =>
                {
                   CheckIfInternetOvertime();
                }, null, 10, (int)TimeSpan.FromSeconds(3).TotalMilliseconds);
            }
        }

        private static void CheckIfInternetOvertime()
        {
            var networkConnection = DependencyService.Get<INetworkConnection>();
            networkConnection.CheckNetworkConnection();
            if (networkConnection.IsConnected)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (hasInternet)
                    {
                        if (!noInterShow)
                        {
                            hasInternet = false;
                            labelScreen.IsVisible = true;
                            await ShowDisplayAlert();
                        }
                    }
                });
            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    hasInternet = true;
                    labelScreen.IsVisible = false;
                });
            }
        }

        public static bool CheckIfInternet()
        {
            var networkConnection = DependencyService.Get<INetworkConnection>();
            networkConnection.CheckNetworkConnection();
            return networkConnection.IsConnected;
        }

        public static async Task<bool> CheckIfInternetAlert()
        {
            var networkConnection = DependencyService.Get<INetworkConnection>();
            networkConnection.CheckNetworkConnection();
            if (networkConnection.IsConnected)
            {
                if (!noInterShow)
                {
                    await ShowDisplayAlert();
                }
                return false;
            }
            return true;
        }






        // Creates timer which regularly checks connectivity and sets private members to reflect connection status
        public static void CheckConnectivity_Timer()
        {
            if (newer_timer == null)
            {
                // Create timer which calls connection checking method regularly
                newer_timer = new Timer((e) =>
                {
                   CheckConnectivity_Simple();
                }, null, 10, (int)TimeSpan.FromSeconds(3).TotalMilliseconds);
            }
        }

        // Method checks connection status and updates private members as needed
        // Displays connection status when offline as async label
        public static void CheckConnectivity_Simple()
        {
            // Check connection status using Xam.Connection NuGet
            var isConnected = CrossConnectivity.Current.IsConnected;
            if (isConnected == true)
            {
                isOnline = true;
            }
            else
            {
                isOnline = false;
            }
        }

        private static async Task ShowDisplayAlert()
        {
            noInterShow = false;
            await currentpage.DisplayAlert("Internet", "Device has no internet, please reconnect", "Oke");
            noInterShow = false;
        }

        // Get registration info and display name
        public static void GetRegistrationInfo()
        {
            
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "regDB.db3");
            var db = new SQLiteConnection(dbPath);
            try
            {
                var name = db.Table<Registration>().OrderBy(x => x.Name).FirstOrDefault().Name.ToString();
                registeredName = name;
                isReg = true;
            }
            catch
            {
                // Do Nothing
            }

        }

    }



}
