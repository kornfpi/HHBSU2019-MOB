// Main program which runs first on app startup

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
using Xamarin.Essentials;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace HabitatBuddy {
    public partial class App : Application {

        // To track issues
        public static HomeIssueManager IssueManager { get; private set; }

        // To track maintenance items
        public static MaintenanceManager mManager { get; private set; }

        // To track home registration info
        public static HomeRegInfoService HomeRegInfo { get; private set; }
        public static bool isReg { get; private set; }
        public static string regHomecode { get; private set; }
        public static string regAddress { get; private set; }
        public static string regName { get; private set; }

        // For regular internet connectivity testing
        private static Timer netCheckTimer;
        public static bool isOnline { get; private set; }

        // Other private fields
        private static Page currentpage;
        public static string registeredName;
        public static string message = "Test Message";
        // End other private fields

        // Main initialization of application
        public App() {

            // Component must be initialized
            InitializeComponent();

            // Initialize services
            IssueManager = new HomeIssueManager(new IssueService());
            mManager = new MaintenanceManager(new MaintenanceService());
            HomeRegInfo = new HomeRegInfoService();

            // Start internet connectivity checking
            initializeNetTimer();
            checkNetConnectivity();

            // Get application registration info
            getRegistrationInfo();

            // Launch registration page
            MainPage = new NavigationPage(new Views.RegistrationPage()); // Main app starts with registration prompt
            MainPage.Title = "Homeowner Buddy";
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

        // Creates timer which regularly calls upon checkNetConnectivity method
        public static void initializeNetTimer()
        {
            if (netCheckTimer == null)
            {
                // Create timer which calls connection checking method regularly
                netCheckTimer = new Timer((e) =>
                {
                    checkNetConnectivity();
                }, null, 10, (int)TimeSpan.FromSeconds(3).TotalMilliseconds);
            }
        }

        // Method checks connection status and updates private members as needed
        public static void checkNetConnectivity()
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                isOnline = true;
            }
            else
            {
                isOnline = false;
                showInternetAlert();
            }
            Console.WriteLine("-----------------------------------------------------------------------------" + isOnline);
        }

        // Alert which appears when internet is not connected
        public static async Task showInternetAlert()
        {
            await currentpage.DisplayAlert("Internet", "Device has no internet, please reconnect!", "Ok");
        }

        // Method which checks if registration table exists in SQLite database
        public static bool TableExists<T>(SQLiteConnection connection) {
            const string cmdText = "SELECT name FROM sqlite_master WHERE type='table' AND name=?";
            var cmd = connection.CreateCommand(cmdText, typeof(T).Name);
            return cmd.ExecuteScalar<string>() != null;
        }

        // Get registration info from local SQLite DB
        public static void getRegistrationInfo()
        {
            // Connect to db and get registraion entry
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "regDB.db3");
            var db = new SQLiteConnection(dbPath);


            //var regEntry = db.Table<Registration>().FirstOrDefault();
            bool checkTable = TableExists<Registration>(db);
            var regEntry = checkTable ? db.Table<Registration>().FirstOrDefault() : null;

            // Set local variables
            if (regEntry != null) // Registration info exists
            { 
                isReg = true;
                regHomecode = regEntry.Id;
                regName = regEntry.Name;
                regAddress = regEntry.Address;
            }
            else // No registration info
            {
                isReg = false;
                regHomecode = "unregistered";
                regName = "unregistered";
                regAddress = "unregistered";
            }
        }

    }

}
