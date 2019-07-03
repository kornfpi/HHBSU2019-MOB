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
	public partial class ActionPlanPage : ContentPage
	{
		public ActionPlanPage (TodoREST.HomeIssue actionPlan)
		{
			InitializeComponent ();



            //Models.Issue issue = new Models.Issue();
            //issue = App.Database.GetIssueAsync(int.Parse(actionPlanID)).Result;
            //TitleText.Text = actionPlan.Title;
            //ContentText.Text = actionPlan.Content;
            //string videoSource = actionPlan.VideoLink;

            var htmlSource = new HtmlWebViewSource();
            htmlSource.Html = string.Format(@"<html><body>
                                  <h1>{0}</h1>
                                  <p>{1}</p>

                                  </body></html>", actionPlan.Title, actionPlan.Content);
            VideoView.Source = htmlSource;
        }

        void webviewNavigating(object sender, WebNavigatingEventArgs e) {
            labelLoading.IsVisible = true;
        }

        void webviewNavigated(object sender, WebNavigatedEventArgs e) {
            labelLoading.IsVisible = false;
        }
    }
}