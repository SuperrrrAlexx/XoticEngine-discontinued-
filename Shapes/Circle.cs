using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XoticEngine.Shapes
{
    public class Circle
    {
        private Point center;
        private int radius;
        private Rectangle boundingBox;

        public Circle(Point center, int radius)
        {
            this.center = center;
            this.radius = radius;
            UpdateBoundingBox();
        }

        private void UpdateBoundingBox()
        {
            boundingBox = new Rectangle(center.X - radius, center.Y - radius, 2 * radius, 2 * radius);
        }

        public bool Contains(Point point)
        {
            return Vector2.Distance(point.ToVector2(), center.ToVector2()) <= radius;
        }

        //Intersections
        public bool Intersects(Circle circle)
        {
            //Check if the distance is smaller than the two radiuses combined
            return Vector2.Distance(center.ToVector2(), circle.Center.ToVector2()) <= radius + circle.Radius;
        }
        public bool Intersects(Rectangle rect)
        {
            //Get the smallest distance to a side
            Vector2 closest = new Vector2(MathHelper.Clamp(center.X, rect.Left, rect.Right), MathHelper.Clamp(center.Y, rect.Top, rect.Bottom));

            return (center.ToVector2() - closest).LengthSquared() < radius * radius;
        }
        public bool Intersects(Line line)
        {
            return line.Intersects(this);
        }

        public Point Center
        {
            get { return center; }
            set
            {
                center = value;
                UpdateBoundingBox();
            }
        }
        public int Radius
        {
            get { return radius; }
            set
            {
                radius = value;
                UpdateBoundingBox();
            }
        }
        public int Diameter
        {
            get { return radius * 2; }
            set
            {
                radius = value / 2;
                UpdateBoundingBox();
            }
        }
        public Rectangle BoundingBox
        { get { return boundingBox; } }
        public int Surface
        { get { return (int)(radius * radius * Math.PI); } }
        public int Circumference
        { get { return (int)(MathHelper.TwoPi * radius); } }
    }
}
