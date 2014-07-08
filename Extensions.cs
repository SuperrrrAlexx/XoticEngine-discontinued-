using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XoticEngine.Shapes;

namespace XoticEngine
{
    public static class Extensions
    {
        //Drawing
        public static void DrawLine(this SpriteBatch s, Line l, Color color)
        {
            //Calculate the direction
            Vector2 direction = l.P2.ToVector2() - l.P1.ToVector2();

            //Draw a rotated rectangle with height 1 (a line)
            s.Draw(Assets.Get<Texture2D>("DummyTexture"), new Rectangle(l.P1.X, l.P1.Y, (int)direction.Length(), 1), null, color, direction.GetAngle(), Vector2.Zero, SpriteEffects.None, 0);
        }
        public static void DrawPath(this SpriteBatch s, Path p, int pointSize, Color color)
        {
            //Draw each point from the path
            foreach (Vector2 v in p.Points)
                s.Draw(Assets.Get<Texture2D>("DummyTexture"), v, null, color, 0, new Vector2(0.5f), pointSize, SpriteEffects.None, 0);
        }

        //Point and Vector2
        public static Vector2 ToVector2(this Point p)
        {
            return new Vector2(p.X, p.Y);
        }
        public static Point ToPoint(this Vector2 v)
        {
            return new Point((int)Math.Round(v.X, MidpointRounding.AwayFromZero), (int)Math.Round(v.Y, MidpointRounding.AwayFromZero));
        }

        public static float GetAngle(this Vector2 v)
        {
            return (float)Math.Atan2(v.Y / v.Length(), v.X / v.Length());
        }

        public static Vector2 GetDirection(this double a)
        {
            return new Vector2((float)Math.Cos(a), (float)Math.Sin(a));
        }
        public static Vector2 GetDirection(this float a)
        {
            return ((double)a).GetDirection();
        }

        public static Vector2 Rotate(this Vector2 v, float radians)
        {
            return new Vector2((float)(Math.Cos(radians) * v.X - Math.Sin(radians) * v.Y), (float)(Math.Sin(radians) * v.X + Math.Cos(radians) * v.Y));
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
