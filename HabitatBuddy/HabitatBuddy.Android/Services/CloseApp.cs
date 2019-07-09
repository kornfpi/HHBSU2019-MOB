// Class which makes the proper call to close application

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using HabitatBuddy.Droid.Services;
using HabitatBuddy.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(CloseApp))]
namespace HabitatBuddy.Droid.Services
{
    public class CloseApp : ICloseApp
    {
        public void closeApplication()
        {
            var activity = (Activity)Forms.Context;
            activity.FinishAffinity();
        }
    }
}