using HabitatBuddy.Models;
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

            //// Filter maintenance reminders to match only this application's home code
            //ObservableCollection<Models.MaintenanceItem> remindersNarrowed = new ObservableCollection<Models.MaintenanceItem>();
            //foreach (Models.MaintenanceItem newItem in r)
            //{
            //    if (newItem.homecode != null && newItem.homecode.Equals(App.regHomecode))
            //    {
            //        remindersNarrowed.Add(newItem);
            //    }
            //}

            // Update all display titles to show appropriate date
            foreach (Models.MaintenanceItem reminder in r) 
            {
                reminder.reloadTitle();
            }

            reminders = r; 
            ReminderList.ItemsSource = reminders;
            ViewPlanButton.IsEnabled = false;
            CompleteButton.IsEnabled = false;
        }

        private async void Complete_Clicked(object sender, EventArgs e) {
            if(!(ReminderList.SelectedItem is null)) {
                var selected = (Models.MaintenanceItem)ReminderList.SelectedItem;
                // Save log info
                if (App.isReg)
                {
                    App.getRegistrationInfo();
                    Log newLog = new Log();
                    newLog.HomeNumber = App.regHomecode;
                    newLog.HomeOwner = App.regName;
                    newLog.Action = "Marked \"" + selected.displayTitle + "\" as completed.";
                    newLog.Date = DateTime.Now.ToString();
                    await App.LogManager.SaveTodoItemAsync(newLog, false);
                }
                // Reset timer
                if ((int)(selected.dueDate - DateTime.Today).TotalDays < 7) {
                    selected.resetDueDate();
                    ReminderList.ItemsSource = null;
                    ReminderList.ItemsSource = reminders;
                } else {
                    // Cannot click button
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