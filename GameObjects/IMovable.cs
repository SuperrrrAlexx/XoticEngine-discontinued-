using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XoticEngine.GameObjects
{
    public interface IMovable
    {
        void Move(Vector2 amount);
    }
}
