using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XoticEngine.Shapes
{
    public class Line
    {
        Point p1, p2;
        Rectangle boundingBox;

        public Line(Point p1, Point p2)
        {
            this.p1 = p1;
            this.p2 = p2;
            UpdateBoundingBox();
        }

        void UpdateBoundingBox()
        {
            //Update the bounding box, with a 1 margin so touching lines are also registered
            boundingBox = new Rectangle(Left.X - 1, Top.Y - 1, Right.X - Left.X + 2, Bottom.Y - Top.Y + 2);
        }

        public bool Intersects(Line line)
        {
            //Check if the bounding boxes intersect
            if (!boundingBox.Intersects(line.BoundingBox))
                return false;

            //Calculate denominator and numerators
            float denominator = ((p2.X - p1.X) * (line.P2.Y - line.P1.Y)) - ((p2.Y - p1.Y) * (line.P2.X - line.P1.X));
            float numerator1 = ((p1.Y - line.P1.Y) * (line.P2.X - line.P1.X)) - ((p1.X - line.P1.X) * (line.P2.Y - line.P1.Y));
            float numerator2 = ((p1.Y - line.P1.Y) * (p2.X - p1.X)) - ((p1.X - line.P1.X) * (p2.Y - p1.Y));

            //Detect coincident lines
            if (denominator == 0) return numerator1 == 0 && numerator2 == 0;

            float r = numerator1 / denominator;
            float s = numerator2 / denominator;

            return (r >= 0 && r <= 1) && (s >= 0 && s <= 1);
        }

        public bool Intersects(Rectangle rectangle)
        {
            //Check if the bounding boxes intersect
            if (!boundingBox.Intersects(rectangle))
                return false;
            //Check if the rectangle contains one of the points
            if (rectangle.Contains(p1) || rectangle.Contains(p2))
                return true;

            //Check if one of the rectangles lines intersects with the line
            return Intersects(new Line(new Point(rectangle.Top, rectangle.Left), new Point(rectangle.Top, rectangle.Right)))
                || Intersects(new Line(new Point(rectangle.Top, rectangle.Right), new Point(rectangle.Bottom, rectangle.Right)))
                || Intersects(new Line(new Point(rectangle.Bottom, rectangle.Right), new Point(rectangle.Bottom, rectangle.Left)))
                || Intersects(new Line(new Point(rectangle.Bottom, rectangle.Left), new Point(rectangle.Top, rectangle.Left)));
        }

        public bool Intersects(Circle circle)
        {
            return circle.Intersects(this);
        }

        public Point P1
        { get { return p1; } set { p1 = value; UpdateBoundingBox(); } }
        public Point P2
        { get { return p2; } set { p2 = value; UpdateBoundingBox(); } }
        public Rectangle BoundingBox
        { get { return boundingBox; } }

        public Point Top
        { get { return p1.Y < p2.Y ? p1 : p2; } }
        public Point Bottom
        { get { return p1.Y >= p2.Y ? p1 : p2; } }
        public Point Left
        { get { return p1.X < p2.X ? p1 : p2; } }
        public Point Right
        { get { return p1.X >= p2.X ? p1 : p2; } }
    }
}
