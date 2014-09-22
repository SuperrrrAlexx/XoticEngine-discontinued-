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
        private SpriteEffects effects = SpriteEffects.None;

        public SpriteGameObject(string name, Texture2D sprite, Vector2 position)
            : base(name, position)
        {
            this.sprite = sprite;
            this.color = Color.White;
        }
        public SpriteGameObject(string name, Texture2D sprite, Vector2 position, Color color)
            : base(name, position)
        {
            this.sprite = sprite;
            this.color = color;
        }
        public SpriteGameObject(string name, Texture2D sprite, Vector2 position, Color color, float rotation, Vector2 origin, float depth)
            : base(name, position, rotation, origin, depth)
        {
            this.sprite = sprite;
            this.color = color;
        }
        public SpriteGameObject(string name, Texture2D sprite, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, float depth)
            : base(name, position, rotation, origin, scale, depth)
        {
            this.sprite = sprite;
            this.color = color;
        }

        public override void Draw(SpriteBatchHolder spriteBatches)
        {
            //Draw the sprite
            spriteBatches[DrawMode].Draw(sprite, Position, null, color, Rotation, Origin, Scale, effects, Depth);

            base.Draw(spriteBatches);
        }

        public Texture2D Sprite
        { get { return sprite; } set { sprite = value; } }
        public Color DrawColor
        { get { return color; } set { color = value; } }
        public SpriteEffects Effects
        { get { return effects; } set { effects = value; } }
    }
}
