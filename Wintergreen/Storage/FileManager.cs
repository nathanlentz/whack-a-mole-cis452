using System;
using System.IO;

namespace Wintergreen.Storage
{
    #region Enums
    public enum StorageLocation
    {
        Local = 1,
        Shared = 2,
        Roaming = 3
    }
    #endregion

    #region Events
    // Unused event. I thought maybe it would be useful, but this may later be deleted.
    public delegate void LoadErrorEventHandler(object sender, LoadErrorEventArgs e);

    public class LoadErrorEventArgs : EventArgs
    {
        string FileLocation;
        string FileName;

        public LoadErrorEventArgs(string fileLocation, string fileName)
        {
            this.FileLocation = fileLocation;
            this.FileName = fileName;
        }
    }
    #endregion

    public static class FileManager
    {
        /// <summary>
        /// Opens and return a FileStream for a file given a StorageLocation enum, a file name, and various enums about the file.
        /// The directory for the file will be created if it does not exist.
        /// </summary>
        /// <param name="location">A StorageLocation enum </param>
        /// <param name="fileName">The name of the file to be written, including an extension</param>
        /// <param name="openType">A FileMode enum indicating how the file should be opened</param>
        /// <param name="accessType">A FileAccess enum indicating the read/write permissions</param>
        /// <param name="shareType">A FileShare enum indicating how the file will be used, similar to FileAccess</param>
        /// <returns>Return a FileStream for reading or writing to the desired file.</returns>
        public static FileStream OpenFile(StorageLocation location, string fileName, FileMode openType = FileMode.OpenOrCreate, FileAccess accessType = FileAccess.Read, FileShare shareType = FileShare.Read)
        {
            string folderPath = GetFolderPath(location);
            return OpenFile(folderPath, fileName, openType, accessType, shareType);
        }

        /// <summary>
        /// Opens and return a FileStream for a file given a location string, a file name, and various enums about the file.
        /// The directory for the file will be created if it does not exist.
        /// </summary>
        /// <param name="location">A string indicating the file path</param>
        /// <param name="fileName">The name of the file to be written, including an extension</param>
        /// <param name="openType">A FileMode enum indicating how the file should be opened</param>
        /// <param name="accessType">A FileAccess enum indicating the read/write permissions</param>
        /// <param name="shareType">A FileShare enum indicating how the file will be used, similar to FileAccess</param>
        /// <returns>Return a FileStream for reading or writing to the desired file.</returns>
        public static FileStream OpenFile(string location, string fileName, FileMode openType = FileMode.OpenOrCreate, FileAccess accessType = FileAccess.Read, FileShare shareType = FileShare.Read)
        {
            // Creates all of the directories in the file path unless they already exist.
            Directory.CreateDirectory(location);

            string filePath = Path.Combine(location, fileName);
            FileStream file;
            try
            {
                file = new FileStream(filePath, openType, accessType, shareType);
                return file;
            }
            catch
            {
                // If there are any problems opening or creating the file, just return nothing.
                // This will probably be replaced later with better error handling to let the user
                // know what the problem might be.
                return null;
            }
        }

        /// <summary>
        /// Converts a StorageLocation enum to the corresponding path string
        /// </summary>
        /// <param name="location">A StorageLocation enum for the desired location</param>
        /// <returns>A string for the path to the corresponding folder</returns>
        public static string GetFolderPath(StorageLocation location)
        {
            switch (location)
            {
                case StorageLocation.Local:
                    return StorageManager.LocalDataPath;
                case StorageLocation.Roaming:
                    return StorageManager.RoamingDataPath;
                case StorageLocation.Shared:
                    return StorageManager.SharedDataPath;
            }
            throw new Exception("Invalid Storage Location.");
        }

        /// <summary>
        /// Converts a StorageLocation enum and an internal folder name to the corresponding path string
        /// </summary>
        /// <param name="location">A StorageLocation enum for the desired location</param>
        /// <param name="folderName">A folder within the storage location</param>
        /// <returns>A string for the path to the corresponding folder</returns>
        public static string GetFolderPath(StorageLocation location, string folderName)
        {
            return Path.Combine(GetFolderPath(location), folderName);
        }

        /// <summary>
        /// Converts a StorageLocation enum and the names of nested folderrs to the corresponding path string
        /// </summary>
        /// <param name="location">A StorageLocation enum for the desired location</param>
        /// <param name="folder1">A folder within the storage location</param>
        /// <param name="folder2">A folder within the first folder</param>
        /// <returns>A string for the path to the corresponding folder</returns>
        public static string GetFolderPath(StorageLocation location, string folder1, string folder2)
        {
            return Path.Combine(GetFolderPath(location), folder1, folder2);
        }

        /// <summary>
        /// Converts a StorageLocation enum and the names of nested folders to the corresponding path string
        /// </summary>
        /// <param name="location">A StorageLocation enum for the desired location</param>
        /// <param name="folder1">A folder within the storage location</param>
        /// <param name="folder2">A folder within the first folder</param>
        /// <param name="folder3">A folder within the second folder</param>
        /// <returns>A string for the path to the corresponding folder</returns>
        public static string GetFolderPath(StorageLocation location, string folder1, string folder2, string folder3)
        {
            return Path.Combine(GetFolderPath(location), folder1, folder2, folder3);
        }

        /// <summary>
        /// Converts a StorageLocation enum and the names of nested folders to the corresponding path string
        /// </summary>
        /// <param name="location">A StorageLocation enum for the desired location</param>
        /// <param name="folderNames">Names of nested folders within the storage location</param>
        /// <returns>A string for the path to the corresponding folder</returns>
        public static string GetFolderPath(StorageLocation location, params string[] folderNames)
        {
            string folderPath = GetFolderPath(location);

            if (folderNames == null)
                return folderPath;

            string[] folderArray = new string[folderNames.Length + 1];
            folderArray[0] = folderPath;

            for (int i = 1; i <= folderNames.Length; i++)
                folderArray[i] = folderNames[i - 1];

            return Path.Combine(folderArray);
        }
    }
}
