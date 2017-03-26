using System;

namespace Wintergreen.Settings
{
    /// <summary>
    /// An attribute which marks a property as being a setting
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class Setting : Attribute
    { }

    /// <summary>
    /// A bool indicating whether the setting is enabled
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class Enabled : Attribute
    {
        private bool value;

        public Enabled(bool enabled)
        {
            this.value = enabled;
        }

        public bool Value
        {
            get { return value; }
            set { this.value = value; }
        }
    }

    /// <summary>
    /// A string indicating the category of the setting
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class Category : Attribute
    {
        private string value;

        public Category(string category)
        {
            this.value = category;
        }

        public string Value
        {
            get { return value; }
        }
    }

    /// <summary>
    /// A string indicating the purpose of the setting
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class Description : Attribute
    {
        private string value;

        public Description(string description)
        {
            this.value = description;
        }

        public string Value
        {
            get { return value; }
        }
    }

    /// <summary>
    /// An object indicating the default value of a setting
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DefaultValue : Attribute
    {
        private object value;

        public DefaultValue(object defaultValue)
        {
            this.value = defaultValue;
        }

        public object Value
        {
            get { return value; }
        }
    }

    /// <summary>
    /// An object indicating the minimum value allowed for a setting
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MinimumValue : Attribute
    {
        private object value;

        public MinimumValue(object minimumValue)
        {
            this.value = minimumValue;
        }

        public object Value
        {
            get { return value; }
        }
    }

    /// <summary>
    /// An object indicating the maximum value allowed for a setting
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MaximumValue : Attribute
    {
        private object value;

        public MaximumValue(object maximumValue)
        {
            this.value = maximumValue;
        }

        public object Value
        {
            get { return value; }
        }
    }
}
