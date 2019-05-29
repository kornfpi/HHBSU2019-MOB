using System;
using System.Collections.Generic;
using HabitatBuddy;
using Xamarin.Forms;

namespace TodoREST.Views
{
    public partial class OneIssuePage : ContentPage
    {
        bool isNewItem;
        public OneIssuePage(bool isNew = false)
        {
            InitializeComponent();
            isNewItem = isNew;
        }
        async void OnSaveActivated(object sender, EventArgs e)
        {
            var todoItem = (HomeIssue)BindingContext;
            await App.IssueManager.SaveTaskAsync(todoItem, isNewItem);
            await Navigation.PopAsync();
        }

        async void OnDeleteActivated(object sender, EventArgs e)
        {
            var todoItem = (HomeIssue)BindingContext;
            await App.IssueManager.DeleteTaskAsync(todoItem);
            await Navigation.PopAsync();
        }
        private void HomeButton(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new MainPage());
        }
        private void AllIssuButton(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new TodoREST.Views.IssueListPage());
        }
        void OnCancelActivated(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}
