using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XoticEngine.Shapes
{
    public class Circle
    {
        Point center;
        int radius;
        Rectangle boundingBox;

        public Circle(Point center, int radius)
        {
            this.center = center;
            this.radius = radius;
            UpdateBoundingBox();
        }

        void UpdateBoundingBox()
        {
            boundingBox = new Rectangle(center.X - radius, center.Y - radius, 2 * radius, 2 * radius);
        }

        public bool Contains(Point point)
        {
            return Vector2.Distance(point.ToVector2(), center.ToVector2()) <= radius;
        }

        public bool Intersects(Circle circle)
        {
            return Vector2.Distance(center.ToVector2(), circle.Center.ToVector2()) <= radius + circle.Radius;
        }

        public bool Intersects(Line line)
        {
            // Translate the space so P1 ends up at the origin:
            Vector2 lineEnd = line.P2.ToVector2() - line.P1.ToVector2(),
                    circleCenter = this.center.ToVector2() - line.P1.ToVector2();
            
            // Project circleCenter onto lineEnd:
            try
            {
                // Calculate the lambda of the projection (of the form p1 + lambda * (p2 - p1)), and restrict it to [0, 1]
                float lambda = MathHelper.Clamp(Vector2.Dot(lineEnd, circleCenter) / lineEnd.LengthSquared(), 0, 1);
                Vector2 closestToLine = lambda * lineEnd - circleCenter;

                return closestToLine.LengthSquared() < this.radius * this.radius;
            }
            catch (DivideByZeroException)
            {
                // The line is actually a point ..
                return circleCenter.LengthSquared() < this.radius * this.radius;
            }
        }

        public Point Center
        { get { return center; } set { center = value; UpdateBoundingBox(); } }
        public int Radius
        { get { return radius; } set { radius = value; UpdateBoundingBox(); } }

        public Rectangle BoundingBox
        { get { return boundingBox; } }
        public int Diameter
        { get { return radius * 2; } set { radius = value / 2; } }
        public int Surface
        { get { return (int)(radius * radius * Math.PI); } }
        public int Circumference
        { get { return (int)(2 * Math.PI * radius); } }
    }
}
