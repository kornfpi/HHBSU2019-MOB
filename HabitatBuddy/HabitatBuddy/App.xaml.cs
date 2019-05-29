using System;
using TodoREST;
using TodoREST.Views;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace HabitatBuddy {
    public partial class App : Application {
        public static HomeIssueManager IssueManager { get; private set; }
        public static MaintenanceManager mManager { get; private set; }
        

        public App() {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
            MainPage.Title = "Homeowner Buddy";
            IssueManager = new HomeIssueManager(new IssueService());
            mManager = new MaintenanceManager(new MaintenanceService());
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
    }
}
