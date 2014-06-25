using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XoticEngine.GraphicEffects
{
    public abstract class PostProcessingEffect
    {
        public abstract Texture2D Apply(Texture2D texture, SpriteBatch spriteBatch, Vector2 pos);
    }
}
