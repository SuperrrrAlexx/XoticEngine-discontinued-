using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XoticEngine.EventArguments
{
    public class SliderEventArgs
    {
        private int value;

        public SliderEventArgs(int value)
        {
            this.value = value;
        }

        public int Value
        { get { return value; } }
    }
}
