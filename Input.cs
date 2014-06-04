using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace XoticEngine
{
    public static class Input
    {
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
            return currMouse.LeftButton == ButtonState.Released && prevMouse.LeftButton == ButtonState.Pressed;
        }
        public static bool MiddleClicked()
        {
            return currMouse.MiddleButton == ButtonState.Released && prevMouse.MiddleButton == ButtonState.Pressed;
        }
        public static bool RightClicked()
        {
            return currMouse.LeftButton == ButtonState.Released && prevMouse.LeftButton == ButtonState.Pressed;
        }

        //Mouse buttons pressed
        public static bool LeftDown()
        {
            return currMouse.LeftButton == ButtonState.Pressed;
        }
        public static bool MiddleDown()
        {
            return currMouse.MiddleButton == ButtonState.Pressed;
        }
        public static bool RightDown()
        {
            return currMouse.RightButton == ButtonState.Pressed;
        }

        //Mouse buttons released
        public static bool LeftReleased()
        {
            return currMouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released;
        }
        public static bool MiddleReleased()
        {
            return currMouse.MiddleButton == ButtonState.Pressed && prevMouse.MiddleButton == ButtonState.Released;
        }
        public static bool RightReleased()
        {
            return currMouse.RightButton == ButtonState.Pressed && prevMouse.RightButton == ButtonState.Released;
        }

        //Scroll wheel
        public static bool ScrolledUp()
        {
            return currMouse.ScrollWheelValue > prevMouse.ScrollWheelValue;
        }
        public static bool ScrolledDown()
        {
            return currMouse.ScrollWheelValue < prevMouse.ScrollWheelValue;
        }
        public static bool Scrolled()
        {
            return currMouse.ScrollWheelValue != prevMouse.ScrollWheelValue;
        }

        //Keyboard buttons
        public static bool KeyPressed(Keys k)
        {
            return currKeyboard.IsKeyDown(k) && prevKeyboard.IsKeyUp(k);
        }
        public static bool KeyReleased(Keys k)
        {
            return currKeyboard.IsKeyUp(k) && prevKeyboard.IsKeyDown(k);
        }
        public static bool KeyDown(Keys k)
        {
            return currKeyboard.IsKeyDown(k);
        }
        public static bool AnyKeyPressed()
        {
            return currKeyboard.GetPressedKeys().Length > prevKeyboard.GetPressedKeys().Length;
        }
        public static Keys[] PressedKeys()
        {
            return currKeyboard.GetPressedKeys().Except(prevKeyboard.GetPressedKeys()).ToArray();
        }

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
    }
}
