using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XoticEngine.GameObjects
{
    public interface IXDrawable
    {
        Texture2D Sprite { get; }
        DrawModes DrawMode { get; }
        Color DrawColor { get; }
        SpriteEffects Effects { get; }
    }
}
