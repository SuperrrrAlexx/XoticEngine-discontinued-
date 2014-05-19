using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ScorpionEngine
{
    public class Benchmark
    {
        static string sName;
        static DateTime sStartTime, sEndTime;
        static bool sStarted = false;

        string name;
        DateTime startTime, endTime;
        bool started = false;

        public Benchmark(string name)
        {
            this.name = name;
        }

        public static void StartStatic(string benchmarkName)
        {
            if (!sStarted)
            {
                sName = benchmarkName;
                GameConsole.Write("Benchmark " + sName + " started");
                sStarted = true;
                sStartTime = DateTime.Now;
            }
            else
                GameConsole.Error("Can't start a static benchmark before the previous on ended, recommend using instances.");
        }

        public void Start()
        {
            if (!started)
            {
                GameConsole.Write("Benchmark " + name + " started");
                started = true;
                startTime = DateTime.Now;
            }
            else
                GameConsole.Error("Can't start a benchmark before it on ended.");
        }

        public static void EndStatic()
        {
            if (sStarted)
            {
                sEndTime = DateTime.Now;
                GameConsole.Write("Benchmark " + sName + " ended");
                GameConsole.Write("Time: " + (sEndTime - sStartTime));
                sStarted = false;
            }
            else
                GameConsole.Error("Can't end benchmark before it started.");
        }

        public void End()
        {
            if (started)
            {
                endTime = DateTime.Now;
                GameConsole.Write("Benchmark " + name + " ended");
                GameConsole.Write("Time: " + (endTime - startTime));
                started = false;
            }
            else
                GameConsole.Error("Can't end benchmark before it started.");
        }

        public DateTime StartTime
        { get { return startTime; } }
        public DateTime EndTime
        { get { return endTime; } }
    }
}
