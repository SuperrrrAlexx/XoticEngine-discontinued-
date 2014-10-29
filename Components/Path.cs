using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XoticEngine.Components
{
    public class Path
    {
        private Vector2[] path;
        private int prev, next;
        private Vector2 position;
        private float speed, distance;
        private bool repeat;
        private bool ended = false;

        public Path(Vector2[] points, float speed, bool repeat)
        {
            //Check if there are at least 2 points
            if (points.Length < 2)
                throw new ArgumentException("The path must contain at least 2 points.");

            this.speed = speed;
            this.repeat = repeat;
            this.path = RemoveEqualConsecutive(points);
            prev = 0;
            next = 1;
            distance = 0;
        }
        public Path(Vector2[] points, float speed, bool repeat, int smoothness)
        {
            //Check if there are at least 2 points
            if (points.Length < 2)
                throw new ArgumentException("The path must contain at least 2 points.");

            this.speed = speed;
            this.repeat = repeat;
            this.path = RemoveEqualConsecutive(CatmullRom(points, smoothness, repeat));
            prev = 0;
            next = 1;
            distance = 0;
        }

        public static Vector2[] CatmullRom(Vector2[] points, int smoothness, bool repeat)
        {
            List<Vector2> newPath = new List<Vector2>();

            //Go through all points
            for (int i = 0; i < points.Length; i++)
            {
                //Calculate the step
                float step = (float)smoothness / Vector2.Distance(VectorNumber(points, i, repeat), VectorNumber(points, i + 1, repeat));
                //Add smoothed points
                for (float t = 0; t <= 1; t += step)
                    newPath.Add(Vector2.CatmullRom(VectorNumber(points, i - 1, repeat), VectorNumber(points, i, repeat), VectorNumber(points, i + 1, repeat), VectorNumber(points, i + 2, repeat), t));
            }

            return newPath.ToArray();
        }
        private static Vector2 VectorNumber(Vector2[] vectorList, int n, bool repeat)
        {
            //Make sure n falls within range of the array
            if (repeat)
            {
                //Wrap n around
                while (n < 0)
                    n += vectorList.Length;
                n %= vectorList.Length;
            }
            else
                //Set n to the first or last element
                n = n < 0 ? 0 : (n < vectorList.Length ? n : vectorList.Length - 1);

            return vectorList[n];
        }
        private Vector2[] RemoveEqualConsecutive(Vector2[] points)
        {
            List<Vector2> newPoints = new List<Vector2>(points);

            //Check all points for an equal point after this one and remove it
            for (int i = points.Length - 1; i >= 0; i--)
                if (points[i] == points[(i + 1) % points.Length])
                    newPoints.RemoveAt(i);

            return newPoints.ToArray();
        }

        public void Update()
        {
            //Check if the path hasn't ended or is repeating
            if (!ended || repeat)
            {
                //Get how much to move
                float move = speed * TimeF.DeltaTime;

                while (Vector2.Distance(position, path[next]) <= move && !ended)
                {
                    move -= Vector2.Distance(position, path[next]);
                    NextPoint();
                    distance = 0;
                }

                distance += move / Vector2.Distance(path[prev], path[next]);

                //Check if the path has ended
                if (ended)
                    distance = 0;

                //Calculate the position
                position = Vector2.Lerp(path[prev], path[next], distance);
            }
        }
        private void NextPoint()
        {
            //Add 1 to both points, wrapping them back to the beginning
            prev = (prev + 1) % path.Length;
            next = (next + 1) % path.Length;

            //Check if the path has ended
            if (next == 0 && !repeat)
                ended = true;
        }

        public void Reset()
        {
            prev = 0;
            next = 1;
            distance = 0;
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
