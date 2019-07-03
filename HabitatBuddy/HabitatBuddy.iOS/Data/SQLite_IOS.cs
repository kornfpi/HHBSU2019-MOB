using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Foundation;
using HabitatBuddy.Data;
using HabitatBuddy.iOS.Data;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLite_IOS))]

namespace HabitatBuddy.iOS.Data
{
    public class SQLite_IOS : SQLConn
    {
        public SQLite_IOS() { }
        public SQLite.SQLiteConnection GetConnection()
        {
            var fileName = "TestDB.db3";
            var documentPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var libraryPath = Path.Combine(documentPath, "..", "Library");
            var path = Path.Combine(libraryPath, fileName);
            var connection = new SQLite.SQLiteConnection(path);
            return connection;
        }
    }
}