using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XoticEngine
{
    public class Path
    {
        Vector2[] path;
        int prev, next;
        Vector2 position;
        float speed, distance;
        bool repeat;

        public Path(Vector2[] points, float speed, bool repeat, bool smooth, int smoothness)
        {
            //Check if there are at least 2 points
            if (points.Count() < 2)
                throw new ArgumentException("The path must contain at least 2 points.");

            this.speed = speed;
            this.repeat = repeat;
            this.path = smooth ? Smoothen(points, smoothness) : points;
            prev = 0;
            next = 1;
            distance = 0;
        }

        Vector2[] Smoothen(Vector2[] points, int smoothness)
        {
            List<Vector2> newPath = new List<Vector2>();

            //Go through all points
            for (int i = 0; i < points.Count(); i++)
            {
                float step = (float)smoothness / Vector2.Distance(VectorNumber(points, i), VectorNumber(points, i + 1));
                for (float t = 0; t <= 1; t += step)
                    newPath.Add(Vector2.CatmullRom(VectorNumber(points, i - 1), VectorNumber(points, i), VectorNumber(points, i + 1), VectorNumber(points, i + 2), t));
            }

            return newPath.ToArray();
        }
        Vector2 VectorNumber(Vector2[] vectorList, int n)
        {
            //Make sure n falls within range of the array
            if (repeat)
            {
                //Wrap n around
                while (n < 0)
                    n += vectorList.Count();
                n %= vectorList.Count();
            }
            else
                //Set n to the first or last element
                n = n < 0 ? 0 : (n < vectorList.Count() ? n : vectorList.Count() - 1);

            return vectorList[n];
        }

        public void Update()
        {
            //Check if the path has ended or is repeating
            if (next < path.Count() || repeat)
            {
                //Set the indices to within range
                prev %= path.Count();
                next %= path.Count();

                //Get the amount to move between the previous and next point, move
                distance += speed * (float)Time.DeltaTime / (path[next] - path[prev]).Length();
                position = Vector2.Lerp(path[prev], path[next], MathHelper.Clamp(distance, 0, 1));

                //Change to the next point
                if (distance >= 1)
                {
                    distance--;
                    prev++;
                    next++;
                }
            }
        }

        public Vector2[] Points
        { get { return path; } }
        public Vector2 Position
        { get { return position; } }
        public bool Repeat
        { get { return repeat; } set { repeat = value; } }
        public float Speed
        { get { return speed; } set { speed = value; } }
    }
}
