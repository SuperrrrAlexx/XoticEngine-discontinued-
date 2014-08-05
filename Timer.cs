using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XoticEngine
{
    public class Timer
    {
        //A static list of timers that updates them all
        private static List<Timer> timers;

        private double time, timeLeft;
        private bool repeat;
        private bool useRealTime = false;
        private bool enabled = true;
        //Event
        public delegate void TimerTick(object sender, EventArgs e);
        public event TimerTick Tick;

        static Timer()
        {
            timers = new List<Timer>();
        }
        public Timer(double time, bool repeat, bool autoUpdate = true)
        {
            this.time = time;
            this.timeLeft = time;
            this.repeat = repeat;

            //Add it to the static list of timers to auto update it
            if (autoUpdate)
                timers.Add(this);
        }

        public static void UpdateAll()
        {
            //Update all enabled timers
            foreach (Timer t in timers)
                if (t.Enabled)
                    t.Update();
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
                        Tick(this, EventArgs.Empty);

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
