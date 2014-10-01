using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using XoticEngine.GameObjects;

namespace XoticEngine.Input
{
    public static class KeyboardInput
    {
        //State
        private static KeyboardState prevKeyboard, currKeyboard;
        //Events
        public delegate void KeyEvent(object sender, KeyEventArgs k);
        public static event KeyEvent OnKeyPressed;
        public delegate void CharEvent(object sender, CharEventArgs c);
        public static event CharEvent OnCharEntered;
        //A dictionary of keys that can be converted to characters
        private static Dictionary<Keys, char> keychars;
        //A dictionary of keys used to move IMovable
        private static Dictionary<MoveKeys, Keys?[]> moveKeyArrays;

        public static void Initialize()
        {
            //Key characters
            keychars = new Dictionary<Keys, char>();
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

            //Move keys
            moveKeyArrays = new Dictionary<MoveKeys, Keys?[]>();
            //Add all move key configurations
            moveKeyArrays.Add(MoveKeys.Arrows, new Keys?[4] { Keys.Up, Keys.Down, Keys.Right, Keys.Left });
            moveKeyArrays.Add(MoveKeys.WASD, new Keys?[4] { Keys.W, Keys.S, Keys.D, Keys.A });
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
                    OnKeyPressed(null, new KeyEventArgs(key));

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
                        OnCharEntered(null, new CharEventArgs(c));
                }
            }
        }

        //Move IMovable
        public static void MoveOnInput(IMovable movable, MoveKeys keys, float speed)
        {
            MoveOnInput(movable, moveKeyArrays[keys], new Vector2(speed));
        }
        public static void MoveOnInput(IMovable movable, MoveKeys keys, Vector2 speed)
        {
            MoveOnInput(movable, moveKeyArrays[keys], speed);
        }
        public static void MoveOnInput(IMovable movable, Keys?[] keys, float speed)
        {
            MoveOnInput(movable, keys, new Vector2(speed));
        }
        public static void MoveOnInput(IMovable movable, Keys?[] keys, Vector2 speed)
        {
            //Check if the array contains 4 keys
            if (keys.Length != 4)
                throw new ArgumentException("The length of the key array must be 4", "keys");

            //The amount to move the object
            Vector2 moveAmount = Vector2.Zero;

            for (int i = 0; i < 4; i++)
            {
                //Check if the keys is not null and is pressed
                if (keys[i].HasValue && KeyDown(keys[i].Value))
                    switch (i)
                    {
                        //Up, down, right, left
                        case 0:
                            moveAmount.Y -= speed.Y;
                            break;
                        case 1:
                            moveAmount.Y += speed.Y;
                            break;
                        case 2:
                            moveAmount.X += speed.X;
                            break;
                        case 3:
                            moveAmount.X -= speed.X;
                            break;
                    }
            }

            //Move the object
            moveAmount *= (float)Time.DeltaTime;
            movable.Move(moveAmount);
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
