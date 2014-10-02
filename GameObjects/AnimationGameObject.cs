using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XoticEngine.GameObjects
{
    public class AnimationGameObject : GameObject, IXDrawable
    {
        private Animation animation;
        private Texture2D sprite;
        private Color color = Color.White;
        private SpriteEffects effects = SpriteEffects.None;
        private DrawModes drawMode = DrawModes.AlphaBlend;
        private Rectangle? sourceRect = null;

        public AnimationGameObject(string name, Animation animation, Vector2 position)
            : base(name, position)
        {
            this.animation = animation;
        }
        public AnimationGameObject(string name, Animation animation, Vector2 position, Color color)
            : base(name, position)
        {
            this.animation = animation;
            this.color = color;
        }
        public AnimationGameObject(string name, Animation animation, Vector2 position, Color color, float rotation, Vector2 origin, float depth)
            : base(name, position, rotation, origin, depth)
        {
            this.animation = animation;
            this.color = color;
        }
        public AnimationGameObject(string name, Animation animation, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, float depth)
            : base(name, position, rotation, origin, scale, depth)
        {
            this.animation = animation;
            this.color = color;
        }

        public override void Update()
        {
            //Update the animation and sprite
            animation.Update();
            sprite = animation.CurrentFrame;

            base.Update();
        }

        public Animation Animation
        { get { return animation; } set { animation = value; } }
        public Texture2D Sprite
        { get { return sprite; } set { sprite = value; } }
        public Color DrawColor
        { get { return color; } set { color = value; } }
        public SpriteEffects Effects
        { get { return effects; } set { effects = value; } }
        public DrawModes DrawMode
        { get { return drawMode; } set { drawMode = value; } }
        public Rectangle? SourceRectangle
        { get { return sourceRect; } set { sourceRect = value; } }
    }
}
