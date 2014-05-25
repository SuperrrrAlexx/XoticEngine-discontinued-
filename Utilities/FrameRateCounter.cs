using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScorpionEngine.Utilities
{
    public static class FrameRateCounter
    {
        static int frames = 0, frameRate = 0, secondsPassed = 0;

        public static void Update()
        {
            if (secondsPassed != SE.Time.GameTime.TotalGameTime.Seconds)
            {
                secondsPassed = SE.Time.GameTime.TotalGameTime.Seconds;
                frameRate = frames;
                //Reset the amount of frames
                frames = 0;
            }
        }

        public static void Draw()
        {
            frames++;
        }

        public static int FrameRate
        { get { return frameRate; } }
    }
}
