using System;
using System.IO;
using System.Diagnostics;

namespace Wintergreen.Storage
{
    internal static class StorageManager
    {
        private readonly static string MY_GAMES = "My Games";

        /// <summary>
        /// Returns the path string for local game data in a "My Games" folder in the user's directory 
        /// </summary>
        // "C:\Users\$CurrentUser$\Documents\My Games\$GameName$" for saving user data on Windows
        // "/home/$CurrentUser$/My Games/$GameName$" for saving user data on Linux
        // "/Users/$CurrentUser$/My Games/$GameName$" for saving user data on OS X
        internal static string LocalDataPath
        {
            get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), MY_GAMES, Game.Name); }
        }

        /// <summary>
        /// Returns the path string for shared game data in a "My Games" folder in the computer's shared directory 
        /// </summary>
        // "C:\ProgramData\My Games\$GameName$" for saving user data on Windows
        // "/usr/share/My Games/$GameName$" for saving user data on Linux
        // "/usr/share/My Games/$GameName$" for saving user data on OS X
        internal static string SharedDataPath
        {
            get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), MY_GAMES, Game.Name); }
        }

        /// <summary>
        /// Returns the path string for roaming game data in a "My Games" folder in the user's roaming directory
        /// </summary>
        // "C:\Users\$CurrentUser$\AppData\Roaming\My Games\$GameName$" for saving user data on Windows
        // "/home/$CurrentUser$/.config/My Games/$GameName$" for saving user data on Linux
        // "/Users/$CurrentUser$/.config/My Games/$GameName$" for saving user data on OS X
        internal static string RoamingDataPath
        {
            get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), MY_GAMES, Game.Name); }
        }
    }
}
