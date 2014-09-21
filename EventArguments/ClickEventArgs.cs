using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using XoticEngine.Input;

namespace XoticEngine.EventArguments
{
    public class ClickEventArgs
    {
        private MouseButton pressed;

        public ClickEventArgs(MouseButton pressed)
        {
            this.pressed = pressed;
        }

        public MouseButton PressedButton
        { get { return pressed; } }
    }
}
