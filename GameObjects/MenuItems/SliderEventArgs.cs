using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XoticEngine.GameObjects.MenuItems
{
    public class SliderEventArgs
    {
        private readonly int value;

        public SliderEventArgs(int value)
        {
            this.value = value;
        }

        public int Value
        { get { return value; } }
    }
}
