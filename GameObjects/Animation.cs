using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace XoticEngine.GameObjects
{
    public class Animation : GameObject
    {
        SpriteSheet sheet;
        double fps;
        bool repeat;

        public Animation(string name, SpriteSheet sheet, double fps, bool repeat)
            : base(name)
        {
            this.sheet = sheet;
            this.fps = fps;
            this.repeat = repeat;
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }
    }
}
