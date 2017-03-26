using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Wintergreen.Settings;

namespace Wintergreen.Input
{
    public static class InputManager
    {
        #region Events

        public delegate void GamePadConnectedEventHandler(object sender, EventArgs e);

        /// <summary>
        /// Event that fires when a GamePad gets connected.
        /// </summary>
        public static event GamePadConnectedEventHandler GamePadConnected;

        public delegate void GamePadDisconnectedEventHandler(object sender, EventArgs e);
        /// <summary>
        /// Event that fires when a GamePad gets disconnected.
        /// </summary>
        public static event GamePadDisconnectedEventHandler GamePadDisconnected;

        #endregion Events

        #region Enums

        public enum MouseButton
        {
            Left,
            Middle,
            Right
        }

        public enum JoystickSide
        {
            Left,
            Right
        }

        public enum JoystickDirection
        {
            None,
            Up,
            Right,
            Down,
            Left,
            RightUp,
            RightDown,
            LeftDown,
            LeftUp
        }

        public enum Toggle
        {
            None,
            Pressed,
            Released
        }

        #endregion Enums

        #region Keyboard States

        private static KeyboardState _previousKeyboardState;
        /// <summary>
        /// The previous frame's keyboard state.
        /// </summary>
        public static KeyboardState PreviousKeyboardState
        {
            get { return _previousKeyboardState; }
        }

        private static KeyboardState _currentKeyboardState;
        /// <summary>
        /// The current frame's keyboard state.
        /// </summary>
        public static KeyboardState CurrentKeyboardState
        {
            get { return _currentKeyboardState; }
        }

        #endregion Keyboard States

        #region Mouse States

        private static MouseState _previousMouseState;
        /// <summary>
        /// The previous frame's mouse state.
        /// </summary>
        public static MouseState PreviousMouseState
        {
            get { return _previousMouseState; }
        }

        private static MouseState _currentMouseState;
        /// <summary>
        /// The current frame's mouse state.
        /// </summary>
        public static MouseState CurrentMouseState
        {
            get { return _currentMouseState; }
        }

        #endregion

        #region Gamepad States

        private static GamePadState _previousGamePadState;
        /// <summary>
        /// The previous frame's GamePadState.
        /// </summary>
        public static GamePadState PreviousGamePadState
        {
            get { return _previousGamePadState; }
        }

        private static GamePadState _currentGamePadState;
        /// <summary>
        /// The current frame's GamePadState.
        /// </summary>
        public static GamePadState CurrentGamePadState
        {
            get { return _currentGamePadState; }
        }

        private static JoystickDirection _previousLeftStickDirection;
        public static JoystickDirection PreviousLeftStickDirection
        {
            get { return _previousLeftStickDirection; }
        }

        private static JoystickDirection _currentLeftStickDirection;
        public static JoystickDirection CurrentLeftStickDirection
        {
            get { return _currentLeftStickDirection; }
        }

        private static JoystickDirection _previousRightStickDirection;
        public static JoystickDirection PreviousRightStickDirection
        {
            get { return _previousRightStickDirection; }
        }

        private static JoystickDirection _currentRightStickDirection;
        public static JoystickDirection CurrentRightStickDirection
        {
            get { return _currentRightStickDirection; }
        }

        #endregion Gamepad States

        internal static void Initialize()
        {
            _currentKeyboardState = Keyboard.GetState();
            _currentMouseState = Mouse.GetState();
            _currentGamePadState = GamePad.GetState(0);
        }


        internal static void Update()
        {
            // Update states.
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();

            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

            _previousGamePadState = _currentGamePadState;
            _currentGamePadState = GamePad.GetState(0);

            // Check for GamePad connect
            if (_currentGamePadState.IsConnected && !_previousGamePadState.IsConnected)
                GamePadConnected?.Invoke(null, new EventArgs());


            // Check for GamePad disconnect
            if (!_currentGamePadState.IsConnected && _previousGamePadState.IsConnected)
            {
                GamePadDisconnected?.Invoke(null, new EventArgs());
                // Reset the joystick directions
                _currentLeftStickDirection = JoystickDirection.None;
                _currentRightStickDirection = JoystickDirection.None;
                _previousLeftStickDirection = JoystickDirection.None;
                _previousRightStickDirection = JoystickDirection.None;
            }

            // Don't bother checking joystick directions if it's not connected
            if (!_currentGamePadState.IsConnected)
                return;

            // Update the GamePad's left stick direction if it has one
            if (!GamePad.GetCapabilities(0).HasLeftXThumbStick || !GamePad.GetCapabilities(0).HasLeftYThumbStick)
            {
                _previousLeftStickDirection = _currentLeftStickDirection;
                _currentLeftStickDirection = GetJoystickDirection(_currentGamePadState.ThumbSticks.Left);
            }

            // Update the GamePad's right stick direction if it has one
            if (!GamePad.GetCapabilities(0).HasRightXThumbStick || !GamePad.GetCapabilities(0).HasRightYThumbStick)
            {
                _previousRightStickDirection = _currentRightStickDirection;
                _currentRightStickDirection = GetJoystickDirection(_currentGamePadState.ThumbSticks.Right);
            }
        }

        #region Keyboard Methods

        /// <summary>
        /// Checks whether the specified key was just pressed this frame.
        /// </summary>
        /// <param name="key">The key to check</param>
        /// <returns>Returns true if the key was just pressed</returns>
        public static bool IsNewKeyPress(Keys key)
        {
            return _previousKeyboardState.IsKeyUp(key) && _currentKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Checks whether the specified key was just released this frame.
        /// </summary>
        /// <param name="key">The key to check</param>
        /// <returns>Returns true if the key was just released</returns>
        public static bool IsNewKeyRelease(Keys key)
        {
            return _previousKeyboardState.IsKeyDown(key) && _currentKeyboardState.IsKeyUp(key);
        }

        /// <summary>
        /// Checks whether the specified key is held down.
        /// </summary>
        /// <param name="key">The key to check</param>
        /// <returns>Returns true if the key is held down</returns>
        public static bool IsKeyDown(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Checks whether the specified key's state changed this frame.
        /// </summary>
        /// <param name="key">The key to check</param>
        /// <returns>Returns true if the key's state changed</returns>
        public static bool KeyChanged(Keys key)
        {
            return _previousKeyboardState.IsKeyDown(key) != _currentKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Checks whether the specified key's state changed this frame and returns the type of change.
        /// </summary>
        /// <param name="key">The key to check</param>
        /// <returns>Returns an enum specifying the type of key change</returns>
        public static Toggle KeyChangeType(Keys key)
        {
            bool isDown = _currentKeyboardState.IsKeyDown(key);
            bool wasDown = _previousKeyboardState.IsKeyDown(key);
            if (isDown == wasDown)
                return Toggle.None;
            if (isDown && !wasDown)
                return Toggle.Pressed;
            else
                return Toggle.Released;
        }

        #endregion Keyboard Methods

        #region Mouse Methods

        /// <summary>
        /// Checks whether the specified mouse button was just pressed this frame.
        /// </summary>
        /// <param name="button">The mouse button to check</param>
        /// <returns>Returns true if the mouse button was just pressed</returns>
        public static bool IsNewMousePress(MouseButton button)
        {
            switch(button)
            {
                case MouseButton.Left:
                    return (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released);
                case MouseButton.Right:
                    return (_currentMouseState.RightButton == ButtonState.Pressed && _previousMouseState.RightButton == ButtonState.Released);
                case MouseButton.Middle:
                    return (_currentMouseState.MiddleButton == ButtonState.Pressed && _previousMouseState.MiddleButton == ButtonState.Released);
            }
            throw new Exception("Invalid MouseButton enum.");
        }

        /// <summary>
        /// Checks whether the specified mouse button was just released this frame
        /// </summary>
        /// <param name="button">The mouse button to check</param>
        /// <returns>Returns true if the mouse button was just released</returns>
        public static bool IsNewMouseRelease(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return (_currentMouseState.LeftButton == ButtonState.Released && _previousMouseState.LeftButton == ButtonState.Pressed);
                case MouseButton.Right:
                    return (_currentMouseState.RightButton == ButtonState.Released && _previousMouseState.RightButton == ButtonState.Pressed);
                case MouseButton.Middle:
                    return (_currentMouseState.MiddleButton == ButtonState.Released && _previousMouseState.MiddleButton == ButtonState.Pressed);
            }
            throw new Exception("Invalid MouseButton enum.");
        }

        /// <summary>
        /// Checks whether the specified mouse button is held down.
        /// </summary>
        /// <param name="button">The mouse button to check</param>
        /// <returns>Returns true if the mouse button is held down</returns>
        public static bool IsMouseButtonDown(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return (_currentMouseState.LeftButton == ButtonState.Pressed);
                case MouseButton.Right:
                    return (_currentMouseState.RightButton == ButtonState.Pressed);
                case MouseButton.Middle:
                    return (_currentMouseState.MiddleButton == ButtonState.Pressed);
            }
            throw new Exception("Invalid MouseButton enum.");
        }

        /// <summary>
        /// Checks whether the specified mouse button's state changed this frame
        /// </summary>
        /// <param name="button">The mouse button to check</param>
        /// <returns>Returns true if the mouse button's state changed</returns>
        public static bool MouseButtonChanged(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return (_currentMouseState.LeftButton != _previousMouseState.LeftButton);
                case MouseButton.Right:
                    return (_currentMouseState.RightButton != _previousMouseState.RightButton);
                case MouseButton.Middle:
                    return (_currentMouseState.MiddleButton != _previousMouseState.MiddleButton);
            }
            throw new Exception("Invalid MouseButton enum.");
        }

        /// <summary>
        /// Checks whether the specified mouse button's state changed this frame and returns the type of change.
        /// </summary>
        /// <param name="button">The mouse button to check</param>
        /// <returns>Returns an enum specifying the type of mouse button change</returns>
        public static Toggle MouseButtonChangeType(MouseButton button)
        {
            bool isDown, wasDown;
            switch (button)
            {
                case MouseButton.Left:
                    isDown = _currentMouseState.LeftButton == ButtonState.Pressed;
                    wasDown = _previousMouseState.LeftButton == ButtonState.Pressed;
                    if (isDown == wasDown)
                        return Toggle.None;
                    if (isDown && !wasDown)
                        return Toggle.Pressed;
                    else
                        return Toggle.Released;
                case MouseButton.Right:
                    isDown = _currentMouseState.RightButton == ButtonState.Pressed;
                    wasDown = _previousMouseState.RightButton == ButtonState.Pressed;
                    if (isDown == wasDown)
                        return Toggle.None;
                    if (isDown && !wasDown)
                        return Toggle.Pressed;
                    else
                        return Toggle.Released;
                case MouseButton.Middle:
                    isDown = _currentMouseState.MiddleButton == ButtonState.Pressed;
                    wasDown = _previousMouseState.MiddleButton == ButtonState.Pressed;
                    if (isDown == wasDown)
                        return Toggle.None;
                    if (isDown && !wasDown)
                        return Toggle.Pressed;
                    else
                        return Toggle.Released;
            }
            throw new Exception("Invalid MouseButton enum.");
        }

        /// <summary>
        /// Subtracts the Mouse's current ScrollWheelValue from the ScrollWheelValue of the previous frame
        /// </summary>
        /// <returns></returns>
        public static int ScrollWheelChange()
        {
            return _currentMouseState.ScrollWheelValue - _previousMouseState.ScrollWheelValue;
        }

        #endregion Mouse Methods

        #region GamePad Methods

        #region Buttons

        /// <summary>
        /// Checks whether the specified button was just pressed this frame.
        /// </summary>
        /// <param name="button">The button to check</param>
        /// <returns>Returns true if the button was just pressed</returns>
        public static bool IsNewButtonPress(Buttons button)
        {
            return _previousGamePadState.IsButtonUp(button) && _currentGamePadState.IsButtonDown(button);
        }

        /// <summary>
        /// Checks whether the specified button was just released this frame.
        /// </summary>
        /// <param name="button">The button to check</param>
        /// <returns>Returns true if the button was just released</returns>
        public static bool IsNewButtonRelease(Buttons button)
        {
            return _previousGamePadState.IsButtonUp(button) && _currentGamePadState.IsButtonDown(button);
        }

        /// <summary>
        /// Checks whether the specified button is held down.
        /// </summary>
        /// <param name="button">The button to check</param>
        /// <returns>Returns true if the button is held down</returns>
        public static bool IsButtonDown(Buttons button)
        {
            return _currentGamePadState.IsButtonDown(button);
        }

        /// <summary>
        /// Checks whether the specified button's state changed this frame.
        /// </summary>
        /// <param name="button">The button to check</param>
        /// <returns>Returns true if the button's state changed</returns>
        public static bool ButtonChanged(Buttons button)
        {
            return _previousGamePadState.IsButtonUp(button) && _currentGamePadState.IsButtonDown(button);
        }

        /// <summary>
        /// Checks whether the specified button's state changed this frame and returns the type of change.
        /// </summary>
        /// <param name="button">The button to check</param>
        /// <returns>Returns an enum specifying the type of button change</returns>
        public static Toggle ButtonChangeType(Buttons button)
        {
            bool isDown = _currentGamePadState.IsButtonDown(button);
            bool wasDown = _previousGamePadState.IsButtonDown(button);
            if (isDown == wasDown)
                return Toggle.None;
            if (isDown && !wasDown)
                return Toggle.Pressed;
            else
                return Toggle.Released;
        }

        #endregion Buttons

        #region Joysticks

        /// <summary>
        /// The current direction of the specified joystick.
        /// </summary>
        /// <param name="side">The side of the joystick to check</param>
        /// <returns>Returns an enum specifying the direction of the joystick</returns>
        public static JoystickDirection CurrentJoyStickDirection(JoystickSide side)
        {
            if (side == JoystickSide.Left)
            {
                // If there is no left joystick on the controller, return no direction
                if (!GamePad.GetCapabilities(0).HasLeftXThumbStick || !GamePad.GetCapabilities(0).HasLeftYThumbStick)
                    return JoystickDirection.None;

                return GetJoystickDirection(_currentGamePadState.ThumbSticks.Left);
            }
            else // right joystick
            {
                // If there is no right joystick on the controller, return no direction
                if (!GamePad.GetCapabilities(0).HasRightXThumbStick || !GamePad.GetCapabilities(0).HasRightYThumbStick)
                    return JoystickDirection.None;

                return GetJoystickDirection(_currentGamePadState.ThumbSticks.Right);
            }
        }
        
        // Radian Angle Constants
        // All of these calculations are done at compile time
        private const float _22_5degrees = (float)(22.5f * Math.PI / 180);
        private const float _67_5degrees = (float)(67.5f * Math.PI / 180);
        private const float _112_5degrees = (float)(112.5f * Math.PI / 180);
        private const float _157_5degrees = (float)(157.5f * Math.PI / 180);
        private const float _202_5degrees = (float)(202.5f * Math.PI / 180);
        private const float _248_5degrees = (float)(248.5f * Math.PI / 180);
        private const float _292_5degrees = (float)(292.5f * Math.PI / 180);
        private const float _338_5degrees = (float)(338.5f * Math.PI / 180);

        private static JoystickDirection GetJoystickDirection(Vector2 stick)
        {
            // If the vector's magnitude is within the deadzone, return no direction
            float length = stick.Length();
            if (length < EngineSettings.JoystickDeadzone)
                return JoystickDirection.None;

            // Gets the angle of the Joystick in Radians
            float angle = (float)Math.Atan2(stick.Y, stick.X);
            if (angle < 0)
                angle += (float)(2 * Math.PI);

            if (angle < _22_5degrees || angle >= _338_5degrees)
                return JoystickDirection.Right;
            if (angle < _67_5degrees)
                return JoystickDirection.RightUp;
            if (angle < _112_5degrees)
                return JoystickDirection.Up;
            if (angle < _157_5degrees)
                return JoystickDirection.LeftUp;
            if (angle < _202_5degrees)
                return JoystickDirection.Left;
            if (angle < _248_5degrees)
                return JoystickDirection.LeftDown;
            if (angle < _292_5degrees)
                return JoystickDirection.Down;
            if (angle < _338_5degrees)
                return JoystickDirection.RightDown;

            // pretty sure this shouldn't happen if the math is right.
            // TODO: real error handling here if necessary? Needs testing first
            throw new Exception("Invalid Joystick Direction");
        }

        #endregion Joysticks

        #endregion GamePad Methods
    }
}
