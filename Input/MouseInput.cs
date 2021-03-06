﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace XoticEngine.Input
{
    public static partial class InputManager
    {
        public static class Mouse
        {
            //State
            private static MouseState prevMouse, currMouse;

            internal static void Update()
            {
                //Update the states
                prevMouse = currMouse;
                currMouse = Microsoft.Xna.Framework.Input.Mouse.GetState();

                //Check the events
                if (OnClick != null)
                    CheckEvents();
            }
            private static void CheckEvents()
            {
                //Check if a mouse button was pressed
                if (LeftPressed())
                    OnClick(null, new ClickEventArgs(MouseButton.Left, Position));
                if (MiddlePressed())
                    OnClick(null, new ClickEventArgs(MouseButton.Middle, Position));
                if (RightPressed())
                    OnClick(null, new ClickEventArgs(MouseButton.Right, Position));
            }

            //Clicks
            public static bool LeftPressed()
            {
                return currMouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released;
            }
            public static bool MiddlePressed()
            {
                return currMouse.MiddleButton == ButtonState.Pressed && prevMouse.MiddleButton == ButtonState.Released;
            }
            public static bool RightPressed()
            {
                return currMouse.RightButton == ButtonState.Pressed && prevMouse.RightButton == ButtonState.Released;
            }

            //Button pressed
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

            //Button released
            public static bool LeftReleased()
            {
                return currMouse.LeftButton == ButtonState.Released && prevMouse.LeftButton == ButtonState.Pressed;
            }
            public static bool MiddleReleased()
            {
                return currMouse.MiddleButton == ButtonState.Released && prevMouse.MiddleButton == ButtonState.Pressed;
            }
            public static bool RightReleased()
            {
                return currMouse.RightButton == ButtonState.Released && prevMouse.RightButton == ButtonState.Pressed;
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

            //Mouse state
            public static MouseState PreviousState
            { get { return prevMouse; } }
            public static MouseState CurrentState
            { get { return currMouse; } }
            //Mouse position
            public static Point Position
            { get { return new Point(currMouse.X, currMouse.Y); } }
        }
    }
}