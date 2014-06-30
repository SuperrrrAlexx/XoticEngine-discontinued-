using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XoticEngine.Achievements
{
    public static class AchievementHolder
    {
        //Lists
        static Dictionary<string, Achievement> achievements = new Dictionary<string,Achievement>();
        static Queue<Achievement> queue = new Queue<Achievement>();
        static Achievement current;
        //Movement
        static AchievementMove currentMove = AchievementMove.Up;
        static int moveSpeed = 200;
        static double showTime = 5;
        static double showTimeLeft;

        public static void Add(Achievement a)
        {
            //Add the achievement to the list
            achievements.Add(a.Name, a);
        }

        public static void Update()
        {
            if (current != null)
            {
                switch (currentMove)
                {
                    case AchievementMove.Up:
                        //Move up
                        current.Position -= new Vector2(0, (float)(moveSpeed * Time.RealTime));

                        //Check the position
                        if (current.Position.Y <= Graphics.Viewport.Height - current.BoundingBox.Height)
                        {
                            //Set the position
                            current.Position = new Vector2(current.Position.X, Graphics.Viewport.Height - current.BoundingBox.Height);
                            //Show the achievement
                            currentMove = AchievementMove.Show;
                        }
                        break;
                    case AchievementMove.Show:
                        //Check and reduce the time
                        if (showTimeLeft <= 0)
                            currentMove = AchievementMove.Down;
                        showTimeLeft -= Time.RealTime;
                        break;
                    case AchievementMove.Down:
                        //Move down
                        current.Position += new Vector2(0, (float)(moveSpeed * Time.RealTime));

                        //Check the position
                        if (current.Position.Y >= Graphics.Viewport.Height)
                            current = null;
                        break;
                }
            }
            else if (queue.Count() > 0)
            {
                //Get the new achievement, set the time
                current = queue.Dequeue();
                showTimeLeft = showTime;
                currentMove = AchievementMove.Up;
            }
        }
        public static void Draw(SpriteBatch s)
        {
            //Draw all achievements from the drawing list
            if (current != null)
                current.Draw(s);
        }

        public static void GetAchievement(string name)
        {
            //Add the achievement
            if (!achievements[name].Achieved)
                queue.Enqueue(achievements[name]);

            //Set the achievement as achieved
            achievements[name].Achieved = true;
        }

        enum AchievementMove
        { Up, Show, Down }
    }
}