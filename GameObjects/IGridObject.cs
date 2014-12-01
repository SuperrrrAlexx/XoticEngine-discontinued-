using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XoticEngine.GameObjects
{
    public interface IGridObject
    {
        void Update();
        void Draw(SpriteBatchHolder spriteBatches, Point gridPosition);

        bool Solid { get; }
    }
}
