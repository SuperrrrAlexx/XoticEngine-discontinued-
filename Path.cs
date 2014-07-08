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
        bool ended = false;

        public Path(Vector2[] points, float speed, bool repeat)
        {
            //Check if there are at least 2 points
            if (points.Count() < 2)
                throw new ArgumentException("The path must contain at least 2 points.");

            this.speed = speed;
            this.repeat = repeat;
            this.path = points;
            prev = 0;
            next = 1;
            distance = 0;
        }
        public Path(Vector2[] points, float speed, bool repeat, int smoothness)
        {
            //Check if there are at least 2 points
            if (points.Count() < 2)
                throw new ArgumentException("The path must contain at least 2 points.");

            this.speed = speed;
            this.repeat = repeat;
            this.path = CatmullRom(points, smoothness);
            prev = 0;
            next = 1;
            distance = 0;
        }

        Vector2[] CatmullRom(Vector2[] points, int smoothness)
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
            //Check if the path hasn't ended or is repeating
            if (!ended || repeat)
            {
                //Get how much to move
                float move = speed * (float)Time.DeltaTime;

                //Move to the next point
                while (move >= Vector2.Distance(path[prev], path[next]) && !ended)
                {
                    move -= Vector2.Distance(path[prev], path[next]);
                    NextPoint();
                }

                //Get the lerp amount, make sure it is <= 1
                distance += move / Vector2.Distance(path[prev], path[next]);
                while (distance > 1 && !ended)
                {
                    distance--;
                    NextPoint();
                }

                //Check if the path has ended
                if (ended)
                    distance = 0;

                //Calculate the position
                position = Vector2.Lerp(path[prev], path[next], distance);
            }
        }
        void NextPoint()
        {
            //Add 1 to both points, wrapping them back to the beginning
            prev = (prev + 1) % path.Count();
            next = (next + 1) % path.Count();

            //Check if the path has ended
            if (next == 0 && !repeat)
                ended = true;
        }

        public Vector2[] Points
        { get { return path; } }
        public Vector2 Position
        { get { return position; } }
        public bool Repeat
        { get { return repeat; } set { repeat = value; } }
        public float Speed
        { get { return speed; } set { speed = value; } }
        public bool Ended
        { get { return ended; } }
    }
}
