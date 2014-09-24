using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace XoticEngine.Input
{
    public class KeyEventArgs : EventArgs
    {
        private readonly Keys key;

        public KeyEventArgs(Keys key)
        {
            this.key = key;
        }

        public override string ToString()
        {
            return key.ToString();
        }
        
        public Keys Key
        { get { return key; } }
    }
}
