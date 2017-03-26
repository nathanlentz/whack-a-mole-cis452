using System.Threading;
using System.IO;
using System.Collections.Concurrent;
using Wintergreen.Storage;
using System.Globalization;

namespace Wintergreen
{
    public static class Logger
    {
        #region Log Class
        /// <summary>
        /// A Log class internal to the Logger
        /// </summary>
        private class Log
        {
            private string _text;
            private string _logName;
            private string _location;
            private string _key;

            /// <summary>
            /// Creates a log object to be used by a thread to write to a log file.
            /// </summary>
            /// <param name="text">The text to write to the log file</param>
            /// <param name="logName">The name of the log file</param>
            /// <param name="location">A StorageLocation enum for the log file's location</param>
            public Log(string text, string logName, StorageLocation location)
            {
                _text = UseTimeStamps ? string.Format("{0} - {1}", System.DateTime.Now.ToString("yyyy'-'MM'-'dd HH':'mm':'ss\tzzz"), text) : text;
                _logName = logName;
                _location = UseLogFolder ? FileManager.GetFolderPath(location, "Logs") : FileManager.GetFolderPath(location);
                _key = Path.Combine(_location, _logName);
            }

            /// <summary>
            /// The method that executes on a thread to write to the log's file
            /// </summary>
            public void Write()
            {
                StreamWriter logStreamWriter;
                if (!LogFiles.TryGetValue(_key, out logStreamWriter))
                {
                    FileStream logFileStream = FileManager.OpenFile(_location, _logName + ".log", FileMode.Append, FileAccess.Write, FileShare.Write);
                    logStreamWriter = new StreamWriter(logFileStream);
                    logStreamWriter.AutoFlush = true;
                    LogFiles.TryAdd(_key, logStreamWriter);
                }

                object logLocker;
                if (!LogLockers.TryGetValue(_key, out logLocker))
                {
                    logLocker = new object();
                    LogLockers.TryAdd(_key, logLocker);
                }

                lock (logLocker)
                {
                    logStreamWriter.WriteLine(_text);
                }
            }
        }
        #endregion

        /// <summary>
        /// Whether or not logs will be written to a "Logs" folder in the designated storage location
        /// </summary>
        public static bool UseLogFolder = true;

        /// <summary>
        /// Whether or not logs will be time stamped
        /// </summary>
        public static bool UseTimeStamps = true;

        private static ConcurrentDictionary<string, StreamWriter> _logFiles;
        /// <summary>
        /// A dictionary mapping log file names to the StreamWriter for the file's stream.
        /// </summary>
        private static ConcurrentDictionary<string, StreamWriter> LogFiles
        {
            get
            {
                if (_logFiles == null)
                    return _logFiles = new ConcurrentDictionary<string, StreamWriter>();
                return _logFiles;
            }
        }

        private static ConcurrentDictionary<string, object> _logLockers;
        /// <summary>
        /// A dictionary mapping log file names to the lock object for that file's StreamWriter
        /// </summary>
        private static ConcurrentDictionary<string, object> LogLockers
        {
            get
            {
                if (_logLockers == null)
                    return _logLockers = new ConcurrentDictionary<string, object>();
                return _logLockers;
            }
        }

        /// <summary>
        /// Asynchronously writes the log text to a local log file named $APPNAME$.log
        /// </summary>
        /// <param name="text">The text to write to the log file</param>
        public static void Write(string text)
        {
            Write(text, Game.Name);
        }

        /// <summary>
        /// Asynchronously writes the log text to a given file name in a given location
        /// </summary>
        /// <param name="text">The text to write to the log file</param>
        /// <param name="logName">The name of the log file</param>
        /// <param name="location">A StorageLocation enum for the log file's location</param>
        public static void Write(string text, string logName, StorageLocation location = StorageLocation.Local)
        {
            Log logThread = new Log(text, logName, location);

            Thread writingThread = new Thread(new ThreadStart(logThread.Write));
            writingThread.Start();
        }
    }
}
