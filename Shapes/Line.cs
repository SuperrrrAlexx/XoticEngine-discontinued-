using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ScorpionEngine.Shapes
{
    public class Line
    {
        Point p1, p2;

        public Line(Point p1, Point p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }

        public Point P1
        { get { return p1; } set { p1 = value; } }
        public Point P2
        { get { return p2; } set { p2 = value; } }

        public Point Top
        { get { return p1.Y < p2.Y ? p1 : p2; } }
        public Point Bottom
        { get { return p1.Y > p2.Y ? p1 : p2; } }
        public Point Left
        { get { return p1.X < p2.X ? p1 : p2; } }
        public Point Right
        { get { return p1.X > p2.X ? p1 : p2; } }
    }
}
