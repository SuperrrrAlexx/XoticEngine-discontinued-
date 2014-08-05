using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace XoticEngine.EventArguments
{
    public class CharEventArgs : EventArgs
    {
        private readonly char character;

        public CharEventArgs(char character)
        {
            this.character = character;
        }

        public override string ToString()
        {
            return character.ToString();
        }

        public char Character
        { get { return character; } }
    }
}
