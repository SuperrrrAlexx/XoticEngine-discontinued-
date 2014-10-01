using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XoticEngine.GameObjects.MenuItems
{
    public struct Alignment
    {
        private HorizontalAlignment horizontal;
        private VerticalAlignment vertical;

        public Alignment(HorizontalAlignment horizontal, VerticalAlignment vertical)
        {
            this.horizontal = horizontal;
            this.vertical = vertical;
        }

        public HorizontalAlignment Horizontal
        { get { return horizontal; } set { horizontal = value; } }
        public VerticalAlignment Vertical
        { get { return vertical; } set { vertical = value; } }
    }

    
}
