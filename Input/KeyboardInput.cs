using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace XoticEngine.Input
{
    public static class KeyboardInput
    {
        //State
        static KeyboardState prevKeyboard, currKeyboard;
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
            //Update the states
            prevKeyboard = currKeyboard;
            currKeyboard = Keyboard.GetState();

            //Check for key events
            CheckKeyEvents();
        }
        private static void CheckKeyEvents()
        {
            //Get the pressed keys
            Keys[] keys = PressedKeys().Except(prevKeyboard.GetPressedKeys()).ToArray();

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

        //Keys
        public static bool KeyPressed(Keys k)
        {
            return currKeyboard.IsKeyDown(k) && prevKeyboard.IsKeyUp(k);
        }
        public static bool KeyDown(Keys k)
        {
            return currKeyboard.IsKeyDown(k);
        }
        public static bool KeyReleased(Keys k)
        {
            return currKeyboard.IsKeyUp(k) && prevKeyboard.IsKeyDown(k);
        }

        //All pressed keys
        public static bool AnyKeyPressed()
        {
            return currKeyboard.GetPressedKeys().Length > prevKeyboard.GetPressedKeys().Length;
        }
        public static Keys[] PressedKeys()
        {
            return currKeyboard.GetPressedKeys() ;
        }

        //Keyboard state
        public static KeyboardState PreviousState
        { get { return prevKeyboard; } }
        public static KeyboardState CurrentState
        { get { return currKeyboard; } }
    }
}
