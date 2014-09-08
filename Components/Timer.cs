using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XoticEngine.Components
{
    public class Timer
    {
        //A static list of timers that updates them all
        private static List<Timer> timers = new List<Timer>();

        private double time, timeLeft;
        private bool repeat, useRealTime;
        private bool enabled = true;
        //Event
        public delegate void TimerTick(object sender, EventArgs e);
        public event TimerTick Tick;

        public Timer(double time, bool repeat)
        {
            this.time = time;
            this.timeLeft = time;
            this.repeat = repeat;

            timers.Add(this);
        }

        public static void UpdateAll()
        {
            //Update all enabled timers
            foreach (Timer t in timers)
                if (t.Enabled)
                    t.Update();
        }
        private void Update()
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
        {
            get { return enabled; }
            set
            {
                //Add or remove this from the list, but only if the value changed
                if (enabled && !value)
                    timers.Remove(this);
                else if (!enabled && value)
                    timers.Add(this);

                enabled = value;
            }
        }
    }
}
