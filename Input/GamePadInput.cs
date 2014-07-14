﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace XoticEngine.Input
{
    public static class GamePadInput
    {
        //State
        static GamePadState[] prevGamePad, currGamePad;

        public static void Initialize()
        {
            //Create the gamepad state arrays
            prevGamePad = new GamePadState[4];
            currGamePad = new GamePadState[4];
        }

        public static void Update()
        {
            //Update the states
            for (int i = 0; i < 4; i++)
            {
                prevGamePad[i] = currGamePad[i];
                currGamePad[i] = GamePad.GetState(GetPlayerIndex(i + 1));
            }
        }

        public static PlayerIndex GetPlayerIndex(int playerIndex)
        {
            //Get a PlayerIndex from an int
            switch (playerIndex)
            {
                case 1:
                    return PlayerIndex.One;
                case 2:
                    return PlayerIndex.Two;
                case 3:
                    return PlayerIndex.Three;
                case 4:
                    return PlayerIndex.Four;
                default:
                    throw new IndexOutOfRangeException("Player index must be between 1 and 4.");
            }
        }

        //Buttons
        public static bool ButtonPressed(Buttons b, int playerIndex)
        {
            return currGamePad[playerIndex - 1].IsButtonDown(b) && prevGamePad[playerIndex - 1].IsButtonUp(b);
        }
        public static bool ButtonDown(Buttons b, int playerIndex)
        {
            return currGamePad[playerIndex - 1].IsButtonDown(b);
        }
        public static bool ButtonReleased(Buttons b, int playerIndex)
        {
            return currGamePad[playerIndex - 1].IsButtonUp(b) && prevGamePad[playerIndex - 1].IsButtonDown(b);
        }

        //Analog sticks
        public static Vector2 LeftStick(int playerIndex)
        {
            return currGamePad[playerIndex - 1].ThumbSticks.Left;
        }
        public static Vector2 RightStick(int playerIndex)
        {
            return currGamePad[playerIndex - 1].ThumbSticks.Right;
        }

        //Triggers
        public static float LeftTrigger(int playerIndex)
        {
            return currGamePad[playerIndex - 1].Triggers.Left;
        }
        public static float RightTrigger(int playerIndex)
        {
            return currGamePad[playerIndex - 1].Triggers.Right;
        }

        //GamePad state
        public static GamePadState[] PreviousState
        { get { return prevGamePad; } }
        public static GamePadState[] CurrentState
        { get { return currGamePad; } }
        //Connected gamepads
        public static int ConnectedGamePads
        {
            get
            {
                //Check the gamepads, return when one is not connected
                for (int i = 0; i < 4; i++)
                    if (!currGamePad[i].IsConnected)
                        return i;
                return 4;
            }
        }
    }
}
