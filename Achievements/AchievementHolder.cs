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
        private static bool enabled = false;
        private static string exception = "Initialize must be called before using the AchievementHolder.";
        //Lists
        private static Dictionary<string, Achievement> achievements;
        private static Queue<Achievement> queue;
        private static Achievement current;
        //Movement
        private static AchievementMove currentMove = AchievementMove.Up;
        private static int moveSpeed = 200;
        private static double showTime = 5;
        private static double showTimeLeft;
        //Achievement default properties
        private static SpriteFont nameFont, descFont;
        private static Color textColor, backColor;

        public static void Initialize(SpriteFont defaultNameFont, SpriteFont defaultDescFont, Color defaultTextColor, Color defaultBackColor)
        {
            //Enable
            enabled = true;

            //Create the lists
            achievements = new Dictionary<string,Achievement>();
            queue = new Queue<Achievement>();

            //Save the default properties
            nameFont = defaultNameFont;
            descFont = defaultDescFont;
            textColor = defaultTextColor;
            backColor = defaultBackColor;
        }

        public static void Add(string name, string description, Texture2D picture)
        {
            //Create an achievement and add it to the list
            Add(new Achievement(name, description, nameFont, descFont, textColor, backColor, picture));
        }
        public static void Add(Achievement a)
        {
            if (!enabled)
                throw new InvalidOperationException(exception);

            //Add the achievement to the list
            achievements.Add(a.Name, a);
        }

        public static void Update()
        {
            if (enabled)
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
        }
        public static void Draw(SpriteBatch s)
        {
            //Draw all achievements from the drawing list
            if (current != null)
                current.Draw(s);
        }

        public static void GetAchievement(string name)
        {
            if (!enabled)
                throw new InvalidOperationException(exception);

            //Add the achievement
            if (!achievements[name].Achieved)
                queue.Enqueue(achievements[name]);

            //Set the achievement as achieved
            achievements[name].Achieved = true;
        }

        private enum AchievementMove
        { Up, Show, Down }

        public static Dictionary<string, Achievement> Achievements
        { get { return achievements; } }
    }
}