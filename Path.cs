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

        public Path(Vector2[] points, float speed, bool repeat)
        {
            //Check if there are at least 2 points
            if (points.Count() < 2)
                throw new ArgumentException("The path must contain at least 2 points.");

            this.path = points;
            this.speed = speed;
            this.repeat = repeat;
            prev = 0;
            next = 1;
            distance = 0;
        }

        public void Update()
        {
            //Check if the path has ended or is repeating
            if (prev < path.Count() || repeat)
            {
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

        public Vector2 Position
        { get { return position; } }
        public bool Repeat
        { get { return repeat; } set { repeat = value; } }
        public float Speed
        { get { return speed; } set { speed = value; } }
    }
}
