using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XoticEngine.GameObjects.MenuItems
{
    public class Label : MenuItem
    {
        //Text
        protected string text;
        private SpriteFont font;
        private Vector2 textPos;
        private Alignment alignment;
        //Colors
        private Color textColor, backColor;
        //Background
        private Texture2D backTex;

        public Label(string name, Rectangle backRect, float depth, string text, SpriteFont font, Color textColor, Color backColor)
            : base(name, backRect, 0, Vector2.Zero, depth)
        {
            this.text = text;
            this.font = font;
            this.textColor = textColor;
            this.backColor = backColor;
            this.backTex = Assets.DummyTexture;

            alignment = new MenuItems.Alignment(HorizontalAlignment.Center, VerticalAlignment.Center);
            CalculateTextPosition();
        }
        public Label(string name, Rectangle backRect, float depth, string text, SpriteFont font, Color textColor, Texture2D background, Color backColor)
            : base(name, backRect, 0, Vector2.Zero, depth)
        {
            this.text = text;
            this.font = font;
            this.textColor = textColor;
            this.backColor = backColor;
            this.backTex = background;

            alignment = new MenuItems.Alignment(HorizontalAlignment.Center, VerticalAlignment.Center);
            CalculateTextPosition();
        }

        private void CalculateTextPosition()
        {
            //Horizontal alignment
            switch (alignment.Horizontal)
            {
                case HorizontalAlignment.Left:
                    textPos.X = ClickRectangle.Left;
                    break;
                case HorizontalAlignment.Center:
                    textPos.X = ClickRectangle.Center.X - font.MeasureString(text).X / 2;
                    break;
                case HorizontalAlignment.Right:
                    textPos.X = ClickRectangle.Right - font.MeasureString(text).X;
                    break;
            }

            //Vertical alignment
            switch (alignment.Vertical)
            {
                case VerticalAlignment.Top:
                    textPos.Y = ClickRectangle.Top;
                    break;
                case VerticalAlignment.Center:
                    textPos.Y = ClickRectangle.Center.Y - font.MeasureString(text).Y / 2;
                    break;
                case VerticalAlignment.Bottom:
                    textPos.Y = ClickRectangle.Bottom - font.MeasureString(text).Y;
                    break;
            }
        }

        public override void Draw(SpriteBatchHolder spriteBatches)
        {
            //Draw the background
            spriteBatches[DrawModes.Gui].Draw(backTex, ClickRectangle, null, backColor, 0, Vector2.Zero, SpriteEffects.None, MathHelper.Clamp(Depth + float.Epsilon, 0, 1));
            //Draw the text
            spriteBatches[DrawModes.Gui].DrawString(font, text, textPos, textColor, 0, Vector2.Zero, 1, SpriteEffects.None, Depth);
            
            base.Draw(spriteBatches);
        }

        public override Vector2 RelativePosition
        {
            get { return base.RelativePosition; }
            set
            {
                base.RelativePosition = value;
                CalculateTextPosition();
            }
        }
        //Text
        protected Vector2 TextPosition
        { get { return textPos; } }
        public virtual string Text
        {
            get { return text; }
            set
            {
                text = value;
                CalculateTextPosition();
            }
        }
        public SpriteFont Font
        {
            get { return font; }
            set
            {
                font = value;
                CalculateTextPosition();
            }
        }
        public Alignment Alignment
        {
            get { return alignment; }
            set
            {
                alignment = value;
                CalculateTextPosition();
            }
        }
        //Colors
        public Color TextColor
        { get { return textColor; } set { textColor = value; } }
        public Color BackColor
        { get { return backColor; } set { backColor = value; } }
        public Texture2D BackTexture
        { get { return backTex; } set { backTex = value; } }
    }
}
