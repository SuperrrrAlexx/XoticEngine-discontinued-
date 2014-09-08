using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XoticEngine.Shapes;

namespace XoticEngine.GameObjects
{
    public class SpriteGameObject : GameObject
    {
        private Texture2D sprite;
        private Color color;
        private Vector2 scale;
        private SpriteEffects effects = SpriteEffects.None;

        public SpriteGameObject(string name, Texture2D sprite, Vector2 position)
            : base(name, position)
        {
            this.sprite = sprite;
            this.color = Color.White;
            this.scale = Vector2.One;
        }
        public SpriteGameObject(string name, Texture2D sprite, Vector2 position, Color color)
            : base(name, position)
        {
            this.sprite = sprite;
            this.color = color;
            this.scale = Vector2.One;
        }
        public SpriteGameObject(string name, Texture2D sprite, Vector2 position, Color color, float rotation, Vector2 origin)
            : base(name, position, rotation, origin)
        {
            this.sprite = sprite;
            this.color = color;
            this.scale = Vector2.One;
        }
        public SpriteGameObject(string name, Texture2D sprite, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, float depth)
            : base(name, position, rotation, origin, depth)
        {
            this.sprite = sprite;
            this.color = color;
            this.scale = scale;
        }

        public override void Draw(SpriteBatchHolder spriteBatches)
        {
            //Draw the sprite
            spriteBatches[DrawMode].Draw(sprite, Position, null, color, Rotation, Origin, scale, effects, Depth);

            base.Draw(spriteBatches);
        }

        public Texture2D Sprite
        { get { return sprite; } set { sprite = value; } }
        public Color DrawColor
        { get { return color; } set { color = value; } }
        public SpriteEffects Effects
        { get { return effects; } set { effects = value; } }
        public Vector2 Scale
        { get { return scale; } set { scale = value; } }
    }
}
