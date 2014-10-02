using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XoticEngine.GameObjects
{
    public class AnimationGameObject : SpriteGameObject
    {
        private Animation animation;

        public AnimationGameObject(string name, Animation animation, Vector2 position)
            : base(name, animation.CurrentFrame, position)
        {
            this.animation = animation;
        }
        public AnimationGameObject(string name, Animation animation, Vector2 position, Color color)
            : base(name, animation.CurrentFrame, position, color)
        {
            this.animation = animation;
        }
        public AnimationGameObject(string name, Animation animation, Vector2 position, Color color, float rotation, Vector2 origin, float depth)
            : base(name, animation.CurrentFrame, position, color, rotation, origin, depth)
        {
            this.animation = animation;
        }
        public AnimationGameObject(string name, Animation animation, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, float depth)
            : base(name, animation.CurrentFrame, position, color, rotation, origin, scale, depth)
        {
            this.animation = animation;
        }

        public override void Update()
        {
            //Update the animation and sprite
            animation.Update();
            Sprite = animation.CurrentFrame;

            base.Update();
        }

        public Animation Animation
        { get { return animation; } set { animation = value; } }
    }
}
