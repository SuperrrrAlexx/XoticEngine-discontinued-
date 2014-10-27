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
        //Lists
        private static Dictionary<string, IAchievement> achievements;
        private static Queue<IAchievement> queue;
        private static IAchievement current;

        public static void Initialize()
        {
            //Enable
            enabled = true;

            //Create the lists
            achievements = new Dictionary<string, IAchievement>();
            queue = new Queue<IAchievement>();
        }

        public static void Add(IAchievement a)
        {
            if (!enabled)
                throw new InvalidOperationException("Initialize must be called before using the AchievementHolder.");

            //Add the achievement to the list
            achievements.Add(a.Name, a);
        }

        internal static void Update()
        {
            if (enabled)
            {
                if (current != null)
                {
                    //Update the current achievement
                    current.Update();

                    //Check if the achievement is done
                    if (current.Done)
                        current = null;
                }
                //Check if the queue contains achievements
                else if (queue.Count > 0)
                    current = queue.Dequeue();
            }
        }
        internal static void Draw(SpriteBatchHolder s)
        {
            //Draw the current achievement
            if (current != null)
                current.Draw(s);
        }

        public static void GetAchievement(string name)
        {
            if (!enabled)
                throw new InvalidOperationException("Initialize must be called before using the AchievementHolder.");

            //Add the achievement
            if (!achievements[name].Achieved)
                queue.Enqueue(achievements[name]);

            //Set the achievement as achieved
            achievements[name].Achieved = true;
        }

        public static Dictionary<string, IAchievement> Achievements
        { get { return achievements; } }
    }
}