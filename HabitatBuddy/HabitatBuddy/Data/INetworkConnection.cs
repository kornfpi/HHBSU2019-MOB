using System;
using System.Collections.Generic;
using System.Text;

namespace HabitatBuddy.Data
{
    public interface INetworkConnection
    {
        bool IsConnected { get; set; }
        void CheckNetworkConnection();

    }
}
