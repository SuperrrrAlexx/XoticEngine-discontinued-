using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XoticEngine
{
    public class Timer
    {
        private double time, timeLeft;
        private bool repeat;
        private bool useRealTime = false;
        private bool enabled = true;
        //Event
        public delegate void TimerTick(object sender);
        public event TimerTick Tick;

        public Timer(double time, bool repeat)
        {
            this.time = time;
            this.repeat = repeat;
        }

        public void Update()
        {
            if (enabled)
            {
                //Update the time
                timeLeft -= useRealTime ? Time.RealTime : Time.DeltaTime;

                //Check if the timer ticks
                if (timeLeft <= 0)
                {
                    if (Tick != null)
                        Tick(this);

                    //Repeat or disable
                    if (repeat)
                        timeLeft = time;
                    else
                        enabled = false;
                }
            }
        }

        public double TickTime
        { get { return time; } set { time = value; } }
        public double TimeLeft
        { get { return timeLeft; } }
        public bool Repeat
        { get { return repeat; } set { repeat = value; } }
        public bool UseRealTime
        { get { return useRealTime; } set { useRealTime = value; } }
        public bool Enabled
        { get { return enabled; } set { enabled = value; } }
    }
}
