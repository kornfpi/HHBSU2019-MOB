using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HabitatBuddy.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryPage : ContentPage {

        ObservableCollection<Models.Category> categories;

        public CategoryPage(ObservableCollection<Models.Category> c) {
            InitializeComponent();
            categories = c;
            /*categories = new ObservableCollection<Models.Category>();

            categories.Add(new Models.Category("Kitchen"));
            categories[0].issues.Add(new TodoREST.HomeIssue());
            categories[0].issues.Add(new TodoREST.HomeIssue());
            categories[0].issues[0].Title = "kitchen sink";
            categories[0].issues[1].Title = "kitchen door";
            categories[0].calcSublistHeight();
            categories.Add(new Models.Category("Bathroom"));
            categories[1].issues.Add(new TodoREST.HomeIssue());
            categories[1].issues.Add(new TodoREST.HomeIssue());
            categories[1].issues[0].Title = "Bathroom sink";
            categories[1].issues[1].Title = "Bathroom door";
            categories[1].calcSublistHeight();
            categories.Add(new Models.Category("Basement"));
            categories[2].issues.Add(new TodoREST.HomeIssue());
            categories[2].issues.Add(new TodoREST.HomeIssue());
            categories[2].issues.Add(new TodoREST.HomeIssue());
            categories[2].issues[0].Title = "Sump pump";
            categories[2].issues[1].Title = "Flooding";
            categories[2].issues[2].Title = "Other";
            categories[2].calcSublistHeight();
            */

            // Recalculate required heights of sublist listviews since data may have changed
            foreach(Models.Category x in categories) {
                x.calcSublistHeight();
            }

            // Populate the list view with the categories
            categoryList.ItemsSource = categories;
        }

        private void CategoryList_ItemTapped(object sender, ItemTappedEventArgs e) {
            Models.Category selected = (Models.Category)categoryList.SelectedItem;
            selected.toggleVisibility();
            categoryList.ItemsSource = null;
            categoryList.ItemsSource = categories;
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e) {
            var view = (ListView)sender;
            var selected = (TodoREST.HomeIssue)view.SelectedItem;
            Navigation.PushAsync(new HabitatBuddy.Views.ActionPlanPage(selected));
        }
    }
}