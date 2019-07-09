// Interface for dependency service which closes app on iOS and Android

using System;
using System.Collections.Generic;
using System.Text;

namespace HabitatBuddy.Services
{
    public interface ICloseApp
    {
        // Single method for closing app
        void closeApplication();
    }
}
