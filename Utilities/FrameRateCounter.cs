using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XoticEngine.Utilities
{
    public static class FrameRateCounter
    {
        private static int frames, frameRate, secondsPassed;

        internal static void Update()
        {
            if (secondsPassed != Time.GameTime.TotalGameTime.Seconds)
            {
                secondsPassed = Time.GameTime.TotalGameTime.Seconds;
                frameRate = frames;
                //Reset the amount of frames
                frames = 0;
            }
        }
        internal static void Draw()
        {
            frames++;
        }

        public static int FrameRate
        { get { return frameRate; } }
    }
}
