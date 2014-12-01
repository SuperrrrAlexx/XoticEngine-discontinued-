using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XoticEngine.GameObjects
{
    public class GridSpriteObject : IGridObject, IXDrawable
    {
        private bool solid;

        public GridSpriteObject(bool solid, Texture2D sprite)
        {
            this.solid = solid;
            DefaultProperties();
            this.Sprite = sprite;
        }
        public GridSpriteObject(bool solid, Texture2D sprite, Color color)
        {
            this.solid = solid;
            DefaultProperties();
            this.Sprite = sprite;
            this.DrawColor = color;
        }
        public GridSpriteObject(bool solid, Texture2D sprite, Color color, float rotation, Vector2 origin, float depth)
        {
            this.solid = solid;
            DefaultProperties();
            this.Sprite = sprite;
            this.DrawColor = color;
            this.Rotation = rotation;
            this.Origin = origin;
            this.Depth = depth;
        }
        public GridSpriteObject(bool solid, Texture2D sprite, Color color, float rotation, Vector2 origin, Vector2 scale, float depth)
        {
            this.solid = solid;
            DefaultProperties();
            this.Sprite = sprite;
            this.DrawColor = color;
            this.Rotation = rotation;
            this.Origin = origin;
            this.Scale = scale;
            this.Depth = depth;
        }

        private void DefaultProperties()
        {
            //Set all properties to a default value
            DrawMode = DrawModes.AlphaBlend;
            Position = Vector2.Zero;
            DrawColor = Color.White;
            Rotation = 0;
            Origin = Vector2.Zero;
            Scale = Vector2.One;
            Effects = SpriteEffects.None;
            Depth = 1;
        }

        public virtual void Update()
        {
        }
        public virtual void Draw(SpriteBatchHolder spriteBatches, Point gridPosition)
        {
            Position = gridPosition.ToVector2();
            spriteBatches[DrawMode].Draw(this);
        }

        public bool Solid
        { get { return solid; } set { solid = value; } }

        //Drawing
        public DrawModes DrawMode
        { get; set; }
        public Texture2D Sprite
        { get; set; }
        public Vector2 Position
        { get; set; }
        public Rectangle? SourceRectangle
        { get; set; }
        public Color DrawColor
        { get; set; }
        public float Rotation
        { get; set; }
        public Vector2 Origin
        { get; set; }
        public Vector2 Scale
        { get; set; }
        public SpriteEffects Effects
        { get; set; }
        public float Depth
        { get; set; }
    }
}
