using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XoticEngine.Utilities
{
    public class Benchmark
    {
        private static string _name;
        private static DateTime _startTime, _endTime;
        private static bool _started = false;

        private string name;
        private DateTime startTime, endTime;
        private bool started = false;

        public Benchmark(string name)
        {
            this.name = name;
        }

        public static void StartStatic(string benchmarkName)
        {
            if (!_started)
            {
                _name = benchmarkName;
                GameConsole.Write("Benchmark " + _name + " started");
                _started = true;
                _startTime = DateTime.Now;
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
                GameConsole.Error("Can't start a benchmark multiple times.");
        }

        public static void EndStatic()
        {
            if (_started)
            {
                _endTime = DateTime.Now;
                GameConsole.Write("Benchmark " + _name + " ended");
                GameConsole.Write("Time: " + (_endTime - _startTime));
                _started = false;
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
        public TimeSpan Length
        { get { return endTime - startTime; } }
    }
}
