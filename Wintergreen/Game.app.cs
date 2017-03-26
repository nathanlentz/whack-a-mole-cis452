using System;
using System.Diagnostics;
using System.IO;

namespace Wintergreen
{
    public partial class Game
    {
        /// <summary>
        /// Retrieves the name of the Game's executable file without the extension
        /// </summary>
        private static string _Name;
        public static string Name
        {
            get
            {
                if (_Name != null)
                    return _Name;
                return _Name = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName).Split('.')[0];
            }
        }
    }
}
