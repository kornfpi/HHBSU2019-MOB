using Plugin.LocalNotifications;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HabitatBuddy.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MaintenancePage : ContentPage
	{

        private ObservableCollection<Models.MaintenanceItem> reminders;
		public MaintenancePage (ObservableCollection<Models.MaintenanceItem> r)
		{
			InitializeComponent ();


            // ---------------------------------- Changes Here!
            
            ObservableCollection<Models.MaintenanceItem> remindersNarrowed = new ObservableCollection<Models.MaintenanceItem>();
            foreach (Models.MaintenanceItem newItem in r)
            {
                if (newItem.homecode != null && newItem.homecode.Equals(App.homecode))
                {
                    remindersNarrowed.Add(newItem);
                }
            }

            //---------------------------------------------


            // Update all display titles to show appropriate date
            foreach (Models.MaintenanceItem reminder in remindersNarrowed) // Used to be in r
            {
                reminder.reloadTitle();
            }

            reminders = remindersNarrowed; // Used to be r
            ReminderList.ItemsSource = reminders;
            ViewPlanButton.IsEnabled = false;
            CompleteButton.IsEnabled = false;
        }

        private void Complete_Clicked(object sender, EventArgs e) {
            if(!(ReminderList.SelectedItem is null)) {
                var selected = (Models.MaintenanceItem)ReminderList.SelectedItem;
                if((int)(selected.dueDate - DateTime.Today).TotalDays < 7) {
                    selected.resetDueDate();
                    ReminderList.ItemsSource = null;
                    ReminderList.ItemsSource = reminders;
                } else {

                }
            }
        }

        private void View_Plan_Clicked(object sender, EventArgs e) {
            if (!(ReminderList.SelectedItem is null)) {
                var selected = (Models.MaintenanceItem)ReminderList.SelectedItem;
                Navigation.PushAsync(new HabitatBuddy.Views.ActionPlanPage(selected.actionPlan));
            }
        }

        private void ReminderList_ItemSelected(object sender, SelectedItemChangedEventArgs e) {
            var selected = (Models.MaintenanceItem)ReminderList.SelectedItem;
            ViewPlanButton.IsEnabled = true;
            if ((int)(selected.dueDate - DateTime.Today).TotalDays < 7) {
                CompleteButton.IsEnabled = true;
            } else {
                CompleteButton.IsEnabled = false;
            }
        }
    }
}