using System;
using System.Collections.Generic;
using HabitatBuddy;
using Xamarin.Forms;

namespace TodoREST.Views
{
    public partial class IssueListPage : ContentPage
    {
        public IssueListPage()
        {
            InitializeComponent();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();

           

            listView.ItemsSource = await App.IssueManager.GetTasksAsync();
          


        }
        void OnAddItemClicked(object sender, EventArgs e)
        {
            var todoItem = new HomeIssue()
            {
                IssueId = Guid.NewGuid().ToString()
            };
            var todoPage = new OneIssuePage(true);
            todoPage.BindingContext = todoItem;
            Navigation.PushAsync(todoPage);
        }
        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var issue = e.SelectedItem as HomeIssue;
            var oneIssuePage = new OneIssuePage();
            oneIssuePage.BindingContext = issue;
            Navigation.PushModalAsync(oneIssuePage);
        }
        void HomeButton(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new MainPage());
        }
    }
}
