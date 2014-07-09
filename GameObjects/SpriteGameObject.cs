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
        Texture2D sprite;
        Color color;
        Vector2 scale;
        SpriteEffects effects = SpriteEffects.None;

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

        public override void Draw(SpriteBatch gameBatch, SpriteBatch additiveBatch, SpriteBatch guiBatch)
        {
            //Draw the sprite with the correct spritebatch
            switch (DrawType)
            {
                case DrawMode.AlphaBlend:
                    gameBatch.Draw(sprite, Position, null, color, Rotation, Origin, scale, effects, Depth);
                    break;
                case DrawMode.Additive:
                    additiveBatch.Draw(sprite, Position, null, color, Rotation, Origin, scale, effects, Depth);
                    break;
                case DrawMode.Gui:
                    guiBatch.Draw(sprite, Position, null, color, Rotation, Origin, scale, effects, Depth);
                    break;
            }

            base.Draw(gameBatch, additiveBatch, guiBatch);
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
