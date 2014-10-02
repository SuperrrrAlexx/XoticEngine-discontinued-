using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XoticEngine.GameObjects
{
    public class SpriteGameObject : GameObject, IXDrawable
    {
        private DrawModes drawMode = DrawModes.AlphaBlend;
        private Texture2D texture;
        private Color color = Color.White;
        private Rectangle? sourceRect = null;
        private SpriteEffects effects = SpriteEffects.None;

        public SpriteGameObject(string name, Texture2D texture, Vector2 position)
            : base(name, position)
        {
            this.texture = texture;
        }
        public SpriteGameObject(string name, Texture2D texture, Vector2 position, Color color)
            : base(name, position)
        {
            this.texture = texture;
            this.color = color;
        }
        public SpriteGameObject(string name, Texture2D texture, Vector2 position, Color color, float rotation, Vector2 origin, float depth)
            : base(name, position, rotation, origin, depth)
        {
            this.texture = texture;
            this.color = color;
        }
        public SpriteGameObject(string name, Texture2D texture, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, float depth)
            : base(name, position, rotation, origin, scale, depth)
        {
            this.texture = texture;
            this.color = color;
        }

        public DrawModes DrawMode
        { get { return drawMode; } set { drawMode = value; } }
        public Texture2D Sprite
        { get { return texture; } set { texture = value; } }
        public Color DrawColor
        { get { return color; } set { color = value; } }
        public Rectangle? SourceRectangle
        { get { return sourceRect; } set { sourceRect = value; } }
        public SpriteEffects Effects
        { get { return effects; } set { effects = value; } }
    }
}
