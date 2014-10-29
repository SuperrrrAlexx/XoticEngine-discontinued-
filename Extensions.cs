using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XoticEngine.Components;
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
            s.Draw(Assets.DummyTexture, new Rectangle(l.P1.X, l.P1.Y, (int)direction.Length(), 1), null, color, direction.GetAngle(), Vector2.Zero, SpriteEffects.None, 0);
        }
        public static void DrawPath(this SpriteBatch s, Path p, int pointSize, Color color)
        {
            //Draw each point from the path
            foreach (Vector2 v in p.Points)
                s.Draw(Assets.DummyTexture, v, null, color, 0, new Vector2(0.5f), pointSize, SpriteEffects.None, 0);
        }

        //Point to Vector2
        public static Vector2 ToVector2(this Point p)
        {
            //Return a vector from a point
            return new Vector2(p.X, p.Y);
        }
        public static Point ToPoint(this Vector2 v)
        {
            //Round a vector to a point
            return new Point((int)Math.Round(v.X, MidpointRounding.AwayFromZero), (int)Math.Round(v.Y, MidpointRounding.AwayFromZero));
        }

        //Rectangle size
        public static Point Size(this Rectangle r)
        {
            return new Point(r.Width, r.Height);
        }

        //Angle and direction
        public static float GetAngle(this Vector2 v)
        {
            //Get the angle from a direction vector (after normalizing)
            return MathF.Atan2(v.Y / v.Length(), v.X / v.Length());
        }

        public static Vector2 GetDirection(this double a)
        {
            return ((float)a).GetDirection();
        }
        public static Vector2 GetDirection(this float a)
        {
            //Get the direction from an angle
            return new Vector2(MathF.Cos(a), MathF.Sin(a));
        }

        public static Vector2 Rotate(this Vector2 v, float radians)
        {
            //Rotate the vector
            return new Vector2((float)(Math.Cos(radians) * v.X - Math.Sin(radians) * v.Y), (float)(Math.Sin(radians) * v.X + Math.Cos(radians) * v.Y));
        }

        //Random
        public static float NextFloat(this Random r)
        {
            //Return a random float
            return (float)r.NextDouble();
        }
        public static int NextSign(this Random r)
        {
            //Return -1 or 1
            return r.Next(2) * 2 - 1;
        }
        public static Color NextColor(this Random r)
        {
            //Return a random color
            return new Color(r.NextFloat(), r.NextFloat(), r.NextFloat());
        }

        //Text wrap
        public static string Wrap(this SpriteFont font, string s, int lineWidth)
        {
            string wrapped = String.Empty;
            string line = String.Empty;

            //Get all words
            string[] words = s.Split(' ');

            for (int i = 0; i < words.Length; i++)
            {
                //Check for newline
                if (words[i] == "\n")
                {
                    wrapped += line + "\n";
                    line = String.Empty;
                }
                //Check if the line is longer than the width of the textbox
                else if (font.MeasureString(line + words[i]).X >= lineWidth)
                {
                    wrapped += line + "\n";
                    line = words[i] + " ";
                }
                //Add a word to the line
                else
                    line += words[i] + " ";

                //TODO: Check if the word is longer than the line
            }

            //Return the wrapped text plus the last line
            return wrapped + line;
        }

        //SpriteBatchSettings
        public static void Begin(this SpriteBatch s, SpriteBatchSettings settings)
        {
            //Begin the spritebatch with all the spritebatch settings
            s.Begin(settings.SortMode, settings.BlendState, settings.SamplerState, settings.DepthStencilState, settings.RasterizerState, settings.Effect, settings.TransformMatrix);
        }
    }
}
