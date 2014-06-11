using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XoticEngine
{
    public interface IPostProcessing
    {
        void Draw(Texture2D texture, SpriteBatch s, Vector2 pos);
    }
}
