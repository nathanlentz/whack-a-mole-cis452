using System;
using Wintergreen.Settings;
using Wintergreen.Storage;

namespace Wintergreen
{
    public class EngineSettings : SettingsBase
    {
        public EngineSettings(StorageLocation location, string fileName) : base(location, fileName)
        {
            // Handled by base constructor
        }
        
        private static bool _allowUserResizing = true;
        [Setting]
        [Enabled(true)]
        [Category("Display")]
        [Description("Whether or not the display window can be resized by the user.")]
        [DefaultValue(true)]
        public static bool AllowUserResizing
        {
            get { return _allowUserResizing; }
            set { _allowUserResizing = value; }
        }

        private static bool _isMouseVisible = true;
        [Setting]
        [Enabled(true)]
        [Category("Display")]
        [Description("Whether or not the mouse will appear in the game's window.")]
        [DefaultValue(true)]
        public static bool IsMouseVisible
        {
            get { return _isMouseVisible; }
            set { _isMouseVisible = value; }
        }

        private static int _displayHeight = 480;
        [Setting]
        [Enabled(true)]
        [Category("Display")]
        [Description("The height in pixels of the game's window.")]
        [DefaultValue(480)]
        [MinimumValue(360)]
        public static int DisplayHeight
        {
            get { return _displayHeight; }
            set { _displayHeight = value; }
        }

        private static int _displayWidth = 640;
        [Setting]
        [Enabled(true)]
        [Category("Display")]
        [Description("The width in pixels of the game's window.")]
        [DefaultValue(640)]
        [MinimumValue(640)]
        public static int DisplayWidth
        {
            get { return _displayWidth; }
            set { _displayWidth = value; }
        }

        private static bool _pauseWhenInactive = true;
        [Setting]
        [Enabled(true)]
        [Category("Display")]
        [Description("If the game's window is not in focus, the game will pause.")]
        [DefaultValue(true)]
        public static bool PauseWhenInactive
        {
            get { return _pauseWhenInactive; }
            set { _pauseWhenInactive = value; }
        }

        private static bool _showFPS = true;
        [Setting]
        [Enabled(true)]
        [Category("Display")]
        [Description("Show the game's frames per second in the corner of the winow.")]
        [DefaultValue(true)]
        public static bool ShowFPS
        {
            get { return _showFPS; }
            set { _showFPS = value; }
        }

        private static float _masterVolume = 1.0f;
        [Setting]
        [Enabled(true)]
        [Category("Sound")]
        [Description("A factor by which all other sounds are multiplied on a scale of 0.0 (0% volume) to 1.0 (100% volume)")]
        [DefaultValue(1.0f)]
        [MinimumValue(0.0f)]
        [MaximumValue(1.0f)]
        public static float MasterVolume
        {
            get { return _masterVolume; }
            set { _masterVolume = value; }
        }

        private static float _joystickDeadzone = 0.3f;
        [Setting]
        [Enabled(true)]
        [Category("Input")]
        [Description("A number between 0 an 1 indicating how sensitive joysticks are at picking up input (1 is least sensitive)")]
        [DefaultValue(0.3f)]
        [MinimumValue(0.0f)]
        [MaximumValue(1.0f)]
        public static float JoystickDeadzone
        {
            get { return _joystickDeadzone; }
            set { _joystickDeadzone = value; }
        }

        private static bool _timeBasedUpdating = false;
        [Setting]
        [Enabled(true)]
        [Category("Input")]
        [Description("If true, the game updates based on how much time has passed between frames. If false, the game updates a fixed amount between frames.")]
        [DefaultValue(false)]
        public static bool TimeBasedUpdating
        {
            get { return _timeBasedUpdating; }
            set { _timeBasedUpdating = value; }
        }
    }
}
