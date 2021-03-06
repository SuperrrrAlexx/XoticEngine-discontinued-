﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XoticEngine
{
    public static class Time
    {
        private static GameTime gameTime;
        private static double gameSpeed = 1.0;

        internal static void Update(GameTime g)
        {
            //Save the game time
            gameTime = g;
        }

        public static GameTime GameTime
        { get { return gameTime; } }
        public static double DeltaTime
        { get { return gameTime.ElapsedGameTime.TotalSeconds * gameSpeed; } }
        public static double RealTime
        { get { return gameTime.ElapsedGameTime.TotalSeconds; } }
        public static double GameSpeed
        { get { return gameSpeed; } set { gameSpeed = value; } }
    }

    public static class TimeF
    {
        public static float DeltaTime
        { get { return (float)Time.DeltaTime; } }
        public static float RealTime
        { get { return (float)Time.RealTime; } }
        public static float GameSpeed
        { get { return (float)Time.GameSpeed; } set { Time.GameSpeed = value; } }
    }
}
