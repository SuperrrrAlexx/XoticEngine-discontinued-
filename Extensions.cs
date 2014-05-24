using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ScorpionEngine.Shapes;

namespace ScorpionEngine
{
    public static class Extensions
    {
        //Draw a line
        public static void DrawLine(this SpriteBatch s, Line l, Color color)
        {
            //Calculate the direction and angle
            Vector2 p1 = new Vector2(l.P1.X, l.P1.Y);
            Vector2 p2 = new Vector2(l.P2.X, l.P2.Y);
            Vector2 direction = p2 - p1;
            float angle = (float)Math.Atan2(direction.Y, direction.X);
            //Calculate the distance
            float length = Vector2.Distance(p1, p2);

            //Draw a rotated rectangle with height 1 (a line)
            s.Draw(Assets.Get<Texture2D>("DummyTexture"), new Rectangle((int)p1.X, (int)p1.Y, (int)length, 1), null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
        }

        //Point and Vector2
        public static Vector2 ToVector2(this Point p)
        {
            return new Vector2(p.X, p.Y);
        }

        public static Point ToPoint(this Vector2 v)
        {
            return new Point((int)(v.X + 0.5f), (int)(v.Y + 0.5f));
        }

        //Random
        public static float NextFloat(this Random r)
        {
            return (float)r.NextDouble();
        }

        public static int NextParity(this Random r)
        {
            return r.Next(2) * 2 - 1;
        }
    }
}
