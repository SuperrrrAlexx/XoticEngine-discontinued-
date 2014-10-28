using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XoticEngine.GameObjects;

namespace XoticEngine.ParticleSystem
{
    public class AnimationParticle : Particle
    {
        private Animation animation;

        public AnimationParticle(Animation animation, Color color, double ttl)
            : base(animation.CurrentFrame, color, ttl)
        {
            this.animation = animation;
        }
        public AnimationParticle(Vector2 speed, Animation animation, Color color, double ttl)
            : base(speed, animation.CurrentFrame, color, ttl)
        {
            this.animation = animation;
        }
        public AnimationParticle(Vector2 speed, Vector2 scale, float rotation, float rotationSpeed, Animation animation, Color color, double ttl)
            : base(speed, scale, rotation, rotationSpeed, animation.CurrentFrame, color, ttl)
        {
            this.animation = animation;
        }

        public override void Update()
        {
            //Update the animation and sprite
            animation.Update();
            Texture = animation.CurrentFrame;
            
            base.Update();
        }

        public override Particle Fire()
        {
            Animation a = new Animation(animation.Sheet, animation.FPS, animation.Repeat);
            return new AnimationParticle(Speed, Scale, Rotation, RotationSpeed, a, Color, InitialTimeToLive)
            {
                Origin = this.Origin
            };
        }

        public Animation Animation
        { get { return animation; } set { animation = value; } }
    }
}
