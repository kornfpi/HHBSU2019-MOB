// Class which makes the proper call to close application
// This call CurrentThread.Abort() may be obsolete!

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Foundation;
using HabitatBuddy.iOS.Services;
using HabitatBuddy.Services;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(CloseApp))]
namespace HabitatBuddy.iOS.Services
{
    public class CloseApp : ICloseApp
    {
        public void closeApplication()
        {
            Thread.CurrentThread.Abort();
        }
    }
}