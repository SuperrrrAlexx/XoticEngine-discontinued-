using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using XoticEngine.Input;

namespace XoticEngine.Input
{
    public class ClickEventArgs
    {
        private readonly MouseButton pressed;
        private readonly Point position;

        public ClickEventArgs(MouseButton pressed, Point position)
        {
            this.pressed = pressed;
            this.position = position;
        }

        public MouseButton PressedButton
        { get { return pressed; } }
        public Point MousePosition
        { get { return position; } }
    }
}
