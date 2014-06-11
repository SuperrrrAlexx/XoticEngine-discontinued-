using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XoticEngine
{
    public abstract class PostProcessingEffect
    {
        protected SpriteBatch spriteBatch;
        public abstract void Draw(Texture2D texture, Vector2 pos);
    }
}
