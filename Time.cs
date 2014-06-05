using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XoticEngine
{
    public static class Time
    {
        static GameTime gameTime;
        static double gameSpeed = 1.0;

        public static  void Update(GameTime g)
        {
            //Save the game time
            gameTime = g;
        }

        public static GameTime GameTime
        { get { return gameTime; } }
        public static double DeltaTime
        { get { return gameTime.ElapsedGameTime.TotalSeconds * gameSpeed; } }
        public static double GameSpeed
        { get { return gameSpeed; } set { gameSpeed = value; } }
    }
}
