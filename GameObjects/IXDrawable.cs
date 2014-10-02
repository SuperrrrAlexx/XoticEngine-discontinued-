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
        DrawModes DrawMode { get; }
        Texture2D Sprite { get; }
        Vector2 Position { get; }
        Rectangle? SourceRectangle { get; }
        Color DrawColor { get; }
        float Rotation { get; }
        Vector2 Origin { get; }
        Vector2 Scale { get; }
        SpriteEffects Effects { get; }
        float Depth { get; }
    }
}
