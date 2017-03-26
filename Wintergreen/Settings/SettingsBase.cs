using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using Wintergreen.Storage;
using Wintergreen.ExtensionMethods;

namespace Wintergreen.Settings
{
    public class SettingsBase
    {

        #region File Properties

        private string _location;
        /// <summary>
        /// The storage location of the settings file.
        /// </summary>
        public string Location
        {
            get { return _location; }
        }

        private string _name;
        /// <summary>
        /// The name of the .ini file, e.g. "Settings" for Settings.ini
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// The FileStream for the settings file.
        /// </summary>
        private FileStream _fileStream { get; set; }

        private bool _needsWriting = false;
        /// <summary>
        /// A boolean value for whether or not the settings file needs to be rewritten.
        /// </summary>
        public bool NeedsWriting
        {
            get { return _needsWriting; }
        }

        #endregion

        /// <summary>
        /// Base Constructor for Settings Classes
        /// </summary>
        /// <param name="location">The storage location of the settings file</param>
        /// <param name="fileName">The name of the .ini file, e.g. "Settings" for Settings.ini</param>
        public SettingsBase(StorageLocation location, string fileName)
        {
            _location = FileManager.GetFolderPath(location);
            _name = fileName;
        }

        /// <summary>
        /// Loads the settings file from storage.
        /// </summary>
        /// <param name="writing">True for read access, false for write access</param>
        /// <returns>Returns false if the file cannot be loaded properly.</returns>
        private bool Load(bool writing = false)
        {
            _fileStream = FileManager.OpenFile(Location, Name + ".ini", writing ? FileMode.Create : FileMode.OpenOrCreate, writing ? FileAccess.Write : FileAccess.Read, writing ? FileShare.Write : FileShare.Read);

            // Error opening the file. Default Settings will have to be used instead.
            // Return false so the settings manager can handle the problem.
            if (_fileStream == null)
                return false;

            // If you need to write but can't or need to read but can't, there is a problem.
            if ((writing && !_fileStream.CanWrite) || (!writing && !_fileStream.CanRead))
            {
                _fileStream.Close();
                return false;
            }

            return true;
        }


        /// <summary>
        /// Reads the settings file to apply user settings.
        /// </summary>
        /// <returns>Returns false if the file cannot be loaded properly.</returns>
        public bool Read()
        {
            // Load the file before reading
            if (!Load())
                return false;

            // If the file is new, the settings needs to be written to it.
            if (_fileStream.Length == 0)
            {
                _needsWriting = true;
                _fileStream.Close();
                return true;
            }

            // Read through the settings file
            using (StreamReader fileReader = new StreamReader(_fileStream))
            {
                while (!fileReader.EndOfStream)
                {
                    string currentLine = fileReader.ReadLine();

                    if (currentLine.IsNullOrWhiteSpace())
                        continue;

                    // Current line is a comment or category tag --> ignore it.
                    if (currentLine.StartsWith(";") || currentLine.StartsWith("["))
                        continue;

                    // Split the setting into name and value.
                    string[] settingArray = currentLine.Split('=');

                    // Invalid setting if length is wrong.
                    if (settingArray.Length != 2)
                        continue;

                    string settingName = settingArray[0].Trim();
                    PropertyInfo setting = this.GetType().GetProperty(settingName);

                    // This setting name is invalid or disabled
                    if (setting == null || setting.GetCustomAttribute(typeof(Setting)) == null || !IsEnabled(setting))
                    {
                        // Flag settings as need writing so this invalid setting will be removed.
                        Logger.Write(string.Format("Setting name {0} is not a valid setting.", settingName));
                        _needsWriting = true;
                        continue;
                    }

                    object settingValue;
                    string settingValueString = settingArray[1].Trim();
                    try
                    {
                        settingValue = TypeDescriptor.GetConverter(setting.PropertyType).ConvertFromString(settingValueString);
                    }
                    catch
                    {
                        // Unable to convert --> invalid setting value (e.g. user wrote a string in the ini file where a number should be)
                        // Flag settings as need writing so this invalid value will be replaced with the default.
                        Logger.Write(string.Format("Value of {0} is invalid for the setting: {1}.", settingValueString, settingName));
                        _needsWriting = true;
                        continue;
                    }

                    // Get the default value for the setting if it exists (it should).
                    object defaultValue = null;
                    DefaultValue defaultValueAttribute = setting.GetCustomAttribute(typeof(DefaultValue)) as DefaultValue;
                    if (defaultValueAttribute == null)
                        throw new Exception(string.Format("The setting {0} does not have a default value specified"));

                    defaultValue = defaultValueAttribute.Value;

                    // If the setting is the default value, we can just move on to the next setting
                    if (settingValue.Equals(defaultValue))
                        continue;

                    // Check if setting is below the minimum. Fix and continue if it is
                    if (!IsValidSetting(setting, settingValue as IComparable, true))
                        continue;

                    // Check if setting is above the maximum. Fix and continue if it is
                    if (!IsValidSetting(setting, settingValue as IComparable, false))
                        continue;

                    // If the value from the file is not the default and is not invalid, it should reset the default value
                    UpdateSetting(setting, settingValue);
                }
            }
            // Done reading the file, so close it.
            _fileStream.Close();
            return true;
        }

        /// <summary>
        /// Checks to see if a setting is below its minimum or over its maximum
        /// </summary>
        /// <param name="setting">A PropertyInfo object for the setting</param>
        /// <param name="settingValue">The value for the setting read from the file</param>
        /// <param name="minimumComparison">A bool for whether the minimum or maximum is being checked</param>
        private bool IsValidSetting(PropertyInfo setting, IComparable settingValue, bool minimumComparison)
        {
            // Get minimum/maximum attribute if it exists
            object minMaxAttribute = minimumComparison ?
                setting.GetCustomAttribute(typeof(MinimumValue))
                : setting.GetCustomAttribute(typeof(MaximumValue));

            // If it is null, this settings doesn't have max/min constraints, so it is valid
            if (minMaxAttribute == null)
                return true;

            // Get minimum/maximum value for the setting
            IComparable minMaxValue = minimumComparison ? (minMaxAttribute as MinimumValue).Value as IComparable : (minMaxAttribute as MaximumValue).Value as IComparable;

            if (minMaxValue != null)
            {
                bool outsideValidRange = false;
                try
                {
                    // See if the value is below the minimum for a minimum comparison
                    // or above the maximum for a  maximum comparison.
                    int compare = minMaxValue.CompareTo(settingValue);
                    outsideValidRange = minimumComparison ? compare > 0 : compare < 0;
                }
                catch (Exception e)
                {
                    // An exception here means the setting was created with invalid types.
                    throw e;
                }
                if (outsideValidRange)
                {
                    // If the setting was below the minimum, raise it to the minimum
                    // If the setting was above the maximum, lower to the maximum
                    UpdateSetting(setting, minMaxValue);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Updates a setting with the specified value
        /// </summary>
        /// <param name="setting">A PropertyInfo object for the setting</param>
        /// <param name="value">The new value for the setting</param>
        private void UpdateSetting(PropertyInfo setting, object value)
        {
            string fieldName = setting.Name.Insert(0, "_");
            
            FieldInfo settingField = this.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.IgnoreCase);

            // If the field was not found, the setting naming convention was ignored
            if (settingField == null)
                throw new Exception("Improperly named setting for " + setting.Name);

            if (settingField.FieldType != setting.PropertyType)
                throw new Exception("Setting field type does not match setting property type for " + setting.Name);

            settingField.SetValue(this, value);

            // A setting value has been changed, so the settings file will need updating.
            _needsWriting = true;
        }

        /// <summary>
        /// Write the current settings to the settings file.
        /// </summary>
        /// <returns>Returns false if the file cannot be loaded properly. Returns true if the file is up to date.</returns>
        public bool Write()
        {
            // If the settings file is already up to date, return true
            if (!NeedsWriting)
                return true;

            // Load the FileStream for writing the settings file.
            if (!Load(true))
                return false;

            // File will no longer need writing
            _needsWriting = false;

            // Get all of the settings that have a category and sort them by category
            IEnumerable <PropertyInfo> categorizedSettings = this.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Static).Where(p => p.IsDefined(typeof(Category))).OrderBy(p => (p.GetCustomAttribute(typeof(Category)) as Category).Value);

            // Get all the other settings (the ones without categories)
            IEnumerable<PropertyInfo> otherSettings = this.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Static).Where(p => !p.IsDefined(typeof(Category)));

            // Append the other settings to the categorized settings
            PropertyInfo[] sortedSettings;
            sortedSettings = categorizedSettings.Concat(otherSettings).ToArray();

            // If there are no settings to write, the file is up to date.
            if (sortedSettings.Length == 0)
            {
                _fileStream.Close();
                return true;
            }

            string currentCategoryName = string.Empty;
            Category currentCategory = sortedSettings.First().GetCustomAttribute(typeof(Category)) as Category;

            if (currentCategory != null)
                currentCategoryName = currentCategory.Value;

            using (StreamWriter fileWriter = new StreamWriter(_fileStream))
            {
                fileWriter.WriteLine(string.Format("[{0}]", currentCategoryName));
                fileWriter.WriteLine();
                // Loop through the settings writing them to the file.
                foreach (PropertyInfo pi in sortedSettings)
                {
                    // Skip this setting if it's disabled.
                    if (!IsEnabled(pi))
                        continue;

                    // Get the category of the next setting. This will be other if it has no category.
                    string nextCategoryName = "Other";
                    Category nextCategory = pi.GetCustomAttribute(typeof(Category)) as Category;
                    if (nextCategory != null)
                        nextCategoryName = nextCategory.Value;

                    // If this category is different from the previous category, a new category tag is written,
                    // and the current category is updated
                    if (nextCategoryName != currentCategoryName)
                    {
                        // Write a new line before every new category
                        fileWriter.WriteLine();

                        currentCategoryName = nextCategoryName;
                        fileWriter.WriteLine(string.Format("[{0}]", currentCategoryName));
                        fileWriter.WriteLine();
                    }

                    // If the setting has a description, write it as a value. Really, every setting should have a description
                    Description settingDescription = pi.GetCustomAttribute(typeof(Description)) as Description;
                    if (settingDescription != null)
                        fileWriter.WriteLine(string.Format("; {0}", settingDescription.Value));

                    // Write what the setting's default value is
                    DefaultValue defaultValueAttribute = pi.GetCustomAttribute(typeof(DefaultValue)) as DefaultValue;
                    if (defaultValueAttribute == null)
                        throw new Exception(string.Format("The setting {0} does not have a default value specified"));
                    fileWriter.Write(string.Format("; Default Value: {0}", defaultValueAttribute.Value));

                    // Write what the setting's minimum value is
                    MinimumValue minimumValueAttribute = pi.GetCustomAttribute(typeof(MinimumValue)) as MinimumValue;
                    if (minimumValueAttribute != null)
                        fileWriter.Write(string.Format(", Minimum Value: {0}", minimumValueAttribute.Value));

                    // Write what the setting's minimum value is
                    MaximumValue maximumValueAttribute = pi.GetCustomAttribute(typeof(MaximumValue)) as MaximumValue;
                    if (maximumValueAttribute != null)
                        fileWriter.Write(string.Format(", Maximum Value: {0}", maximumValueAttribute.Value));

                    // Write setting on line separate from description.
                    fileWriter.WriteLine();

                    // Write the setting to the file.
                    fileWriter.WriteLine(string.Format("{0} = {1}", pi.Name, pi.GetValue(this)));
                    fileWriter.WriteLine();
                }
            }
            // Done reading the file, so close it.
            _fileStream.Close();
            return true;
        }

        /// <summary>
        /// Enables or Disables a Setting from being editable by the user.
        /// </summary>
        /// <param name="settingPropertyInfo">The property info of the setting to enable or disable</param>
        /// <param name="enabled">True to enable, false to disable</param>
        public static void EnableSetting(PropertyInfo settingPropertyInfo, bool enabled)
        {
            Enabled enabledAttribute = settingPropertyInfo.GetCustomAttribute(typeof(Enabled)) as Enabled;
            enabledAttribute.Value = enabled;
        }

        /// <summary>
        /// Checks whether a setting is enabled or not.
        /// A setting without an Enabled attribute is by default enabled.
        /// </summary>
        /// <param name="settingPropertyInfo">The propery info of the setting to check.</param>
        /// <returns></returns>
        private bool IsEnabled(PropertyInfo settingPropertyInfo)
        {
            Enabled enabledAttribute = settingPropertyInfo.GetCustomAttribute(typeof(Enabled)) as Enabled;
            return enabledAttribute == null || enabledAttribute.Value;
        }
    }
}
