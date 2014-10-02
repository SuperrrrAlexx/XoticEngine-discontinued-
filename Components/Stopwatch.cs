using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XoticEngine.Components
{
    public class Stopwatch
    {
        //A static list of stopwatches that updates them all
        private static List<Stopwatch> stopwatches = new List<Stopwatch>();

        private bool enabled;
        private bool useRealTime;
        private TimeSpan time;

        public Stopwatch(bool enable)
        {
            time = new TimeSpan();

            //Check to enable it
            if (enable)
            {
                stopwatches.Add(this);
                enabled = true;
            }
        }

        internal static void UpdateAll()
        {
            foreach (Stopwatch s in stopwatches)
                    s.Update();
        }
        private void Update()
        {
            //Update the time
            time = time.Add(TimeSpan.FromSeconds(useRealTime ? Time.RealTime : Time.DeltaTime));
        }

        public void Start()
        {
            //Enable, add this to the list
            if (!enabled)
                stopwatches.Add(this);
            enabled = true;
        }
        public void Stop()
        {
            //Disable, remove this from the list
            if (enabled)
                stopwatches.Remove(this);
            enabled = false;
        }
        public void Reset(bool enable)
        {
            //Start or stop the stopwatch
            if (enable)
                Start();
            else
                Stop();

            //Reset the time
            time = new TimeSpan();
        }

        public override string ToString()
        {
            return time.ToString();
        }

        public bool UseRealTime
        { get { return useRealTime; } set { useRealTime = value; } }
        public TimeSpan TimeSpan
        { get { return time; } }
        public bool Enabled
        {
            get { return enabled; }
            set
            {
                //Start or stop depending on the value
                if (value)
                    Start();
                else
                    Stop();
            }
        }
    }
}
