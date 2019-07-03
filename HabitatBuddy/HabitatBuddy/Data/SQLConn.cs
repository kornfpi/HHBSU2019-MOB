using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace HabitatBuddy.Data
{
    public interface SQLConn
    {
        SQLiteConnection GetConnection();
    }
}
