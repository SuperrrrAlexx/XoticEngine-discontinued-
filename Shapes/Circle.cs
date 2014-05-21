using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ScorpionEngine.Shapes
{
    public class Circle
    {
        Point center;
        int radius;

        public Circle(Point center, int radius)
        {
            this.center = center;
            this.radius = radius;
        }

        public Point Center
        { get { return center; } set { center = value; } }
        public int Radius
        { get { return radius; } set { radius = value; } }

        public int Diameter
        { get { return radius * 2; } set { radius = value / 2; } }
        public int Surface
        { get { return (int)(radius * radius * Math.PI); } }
        public int Circumference
        { get { return (int)(2 * Math.PI * radius); } }
    }
}
