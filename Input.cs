using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ScorpionEngine
{
    public static class Input
    {
        #region Fields
        //Keyboard
        static KeyboardState prevKeyboard;
        static KeyboardState currKeyboard;
        //Mouse
        static MouseState prevMouse;
        static MouseState currMouse;
        //Events
        public delegate void KeyEvent(Keys k);
        public static event KeyEvent OnKeyPressed;
        public delegate void CharEvent(char c);
        public static event CharEvent OnCharEntered;
        //A dictionary of keys that can be converted to characters
        static Dictionary<Keys, char> keychars = new Dictionary<Keys, char>();
        #endregion

        #region Methods
        public static void Initialize()
        {
            //Add all the keys that can be converted to chars
            keychars.Add(Keys.Space, ' ');
            //OEM keys that *should* be the same on every region keyboard
            keychars.Add(Keys.OemComma, ',');
            keychars.Add(Keys.OemMinus, '-');
            keychars.Add(Keys.OemPeriod, '.');
            keychars.Add(Keys.OemPlus, '+');
            //Numpad keys
            keychars.Add(Keys.Divide, '/');
            keychars.Add(Keys.Multiply, '*');
            keychars.Add(Keys.Subtract, '-');
            keychars.Add(Keys.Add, '+');
            keychars.Add(Keys.Decimal, '.');
        }

        public static void Update()
        {
            //Update the keyboard and mouse states
            prevKeyboard = currKeyboard;
            currKeyboard = Keyboard.GetState();
            prevMouse = currMouse;
            currMouse = Mouse.GetState();

            CheckKeyEvents();
        }
        static void CheckKeyEvents()
        {
            //Get the pressed keys
            Keys[] keys = PressedKeys();

            //Trigger an event for each pressed key
            for (int i = 0; i < keys.Count(); i++)
            {
                Keys key = keys[i];
                //Key pressed event
                if (OnKeyPressed != null)
                    OnKeyPressed(key);

                if (OnCharEntered != null)
                {
                    string s = null;
                    char c = ' ';

                    //Check if the key is a char
                    //If the key is a letter
                    if ((key >= Keys.A && key <= Keys.Z))
                    {
                        s = key.ToString();
                        //Capital letter or not
                        if (!KeyDown(Keys.LeftShift) && !KeyDown(Keys.RightShift))
                            s = s.ToLower();
                        c = s[0];
                    }
                    //If the key is a number
                    else if (key >= Keys.D0 && key <= Keys.D9)
                    {
                        s = key.ToString();
                        c = s[1];
                    }
                    //If the key is a numpad number
                    else if (key >= Keys.NumPad0 && key <= Keys.NumPad9)
                    {
                        s = key.ToString();
                        c = s[6];
                    }
                    else if (keychars.ContainsKey(key))
                    {
                        s = keychars[key].ToString();
                        c = keychars[key];
                    }

                    //Trigger the event
                    if (s != null)
                        OnCharEntered(c);
                }
            }
        }

        //Mouse clicks
        public static bool LeftClicked()
        {
            if (currMouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released)
                return true;
            return false;
        }
        public static bool RightClicked()
        {
            if (currMouse.RightButton == ButtonState.Pressed && prevMouse.RightButton == ButtonState.Released)
                return true;
            return false;
        }
        public static bool MiddleClicked()
        {
            if (currMouse.MiddleButton == ButtonState.Pressed && prevMouse.MiddleButton == ButtonState.Released)
                return true;
            return false;
        }

        //Mouse buttons pressed
        public static bool LeftMousePressed()
        {
            if (currMouse.LeftButton == ButtonState.Pressed)
                return true;
            return false;
        }
        public static bool RightMousePressed()
        {
            if (currMouse.RightButton == ButtonState.Pressed)
                return true;
            return false;
        }
        public static bool MiddleMousePressed()
        {
            if (currMouse.MiddleButton == ButtonState.Pressed)
                return true;
            return false;
        }

        //Scroll wheel
        public static bool ScrolledUp()
        {
            if (currMouse.ScrollWheelValue > prevMouse.ScrollWheelValue)
                return true;
            return false;
        }
        public static bool ScrolledDown()
        {
            if (currMouse.ScrollWheelValue < prevMouse.ScrollWheelValue)
                return true;
            return false;
        }
        public static bool Scrolled()
        {
            if (currMouse.ScrollWheelValue != prevMouse.ScrollWheelValue)
                return true;
            return false;
        }

        //Keyboard buttons
        public static bool KeyPressed(Keys k)
        {
            if (currKeyboard.IsKeyDown(k) && prevKeyboard.IsKeyUp(k))
                return true;
            return false;
        }
        public static bool KeyReleased(Keys k)
        {
            if (currKeyboard.IsKeyUp(k) && prevKeyboard.IsKeyDown(k))
                return true;
            return false;
        }
        public static bool KeyDown(Keys k)
        {
            if (currKeyboard.IsKeyDown(k))
                return true;
            return false;
        }
        public static bool AnyKeyPressed()
        {
            if (currKeyboard.GetPressedKeys().Length > prevKeyboard.GetPressedKeys().Length)
                return true;
            return false;
        }
        public static Keys[] PressedKeys()
        {
            return currKeyboard.GetPressedKeys().Except(prevKeyboard.GetPressedKeys()).ToArray();
        }
        #endregion

        #region Properties
        //Keyboard state
        public static KeyboardState PreviousKeyboard
        { get { return prevKeyboard; } }
        public static KeyboardState CurrentKeyboard
        { get { return currKeyboard; } }
        //Mouse state
        public static MouseState PreviousMouse
        { get { return prevMouse; } }
        public static MouseState CurrentMouse
        { get { return currMouse; } }
        //Mouse position
        public static Point MousePosition
        { get { return new Point(currMouse.X, currMouse.Y); } }
        #endregion
    }
}
