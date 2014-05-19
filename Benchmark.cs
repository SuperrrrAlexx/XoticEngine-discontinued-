using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ScorpionEngine
{
    public class Benchmark
    {
        static DateTime startTime, endTime;
        static string name;
        static bool started = false;

        public static void Start(string benchmarkName)
        {
            name = benchmarkName;
            GameConsole.Write("Benchmark " + name + " started");
            started = true;
            startTime = DateTime.Now;
        }

        public static void End()
        {
            if (started)
            {
                endTime = DateTime.Now;
                GameConsole.Write("Benchmark " + name + " ended");
                GameConsole.Write("Time: " + (endTime - startTime));
                started = false;
            }
            else
                GameConsole.Error("Can't end benchmark before it started");
        }
    }
}
