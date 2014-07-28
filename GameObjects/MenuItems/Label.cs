﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XoticEngine.GameObjects.MenuItems
{
    public class Label : GameObject
    {
        //Text
        private string text;
        private SpriteFont font;
        private Vector2 textPos;
        private Alignment alignment;
        //Colors
        private Color textColor, backColor;
        //Background
        private Rectangle backRect;
        private Texture2D backTex;

        public Label(string name, Rectangle backRect, float depth, string text, SpriteFont font, Color textColor, Color backColor)
            : base(name, backRect.Location.ToVector2(), 0, Vector2.Zero, depth)
        {
            this.backRect = backRect;
            this.text = text;
            this.font = font;
            this.textColor = textColor;
            this.backColor = backColor;
            this.backTex = Assets.DummyTexture;

            alignment = new MenuItems.Alignment(HorizontalAlignment.Center, VerticalAlignment.Center);
            CalculateTextPosition();
        }
        public Label(string name, Rectangle backRect, float depth, string text, SpriteFont font, Color textColor, Texture2D background, Color backColor)
            : base(name, backRect.Location.ToVector2(), 0, Vector2.Zero, depth)
        {
            this.backRect = backRect;
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
                    textPos.X = backRect.Left;
                    break;
                case HorizontalAlignment.Center:
                    textPos.X = backRect.Center.X - font.MeasureString(text).X / 2;
                    break;
                case HorizontalAlignment.Right:
                    textPos.X = backRect.Right - font.MeasureString(text).X;
                    break;
            }

            //Vertical alignment
            switch (alignment.Vertical)
            {
                case VerticalAlignment.Top:
                    textPos.Y = backRect.Top;
                    break;
                case VerticalAlignment.Center:
                    textPos.Y = backRect.Center.Y - font.MeasureString(text).Y / 2;
                    break;
                case VerticalAlignment.Bottom:
                    textPos.Y = backRect.Bottom - font.MeasureString(text).Y;
                    break;
            }
        }

        public override void Draw(SpriteBatch gameBatch, SpriteBatch additiveBatch, SpriteBatch guiBatch)
        {
            //Draw the background
            guiBatch.Draw(backTex, backRect, null, backColor, 0, Vector2.Zero, SpriteEffects.None, MathHelper.Clamp(Depth + float.Epsilon, 0, 1));
            //Draw the text
            guiBatch.DrawString(font, text, textPos, textColor, 0, Vector2.Zero, 1, SpriteEffects.None, Depth);
            
            base.Draw(gameBatch, additiveBatch, guiBatch);
        }

        public new Vector2 RelativePosition
        {
            get { return base.RelativePosition; }
            set
            {
                base.RelativePosition = value;
                backRect.Location = Position.ToPoint();
                CalculateTextPosition();
            }
        }
        //Text
        protected Vector2 TextPosition
        { get { return textPos; } }
        public string Text
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
        //Background
        public Rectangle BackRectangle
        {
            get { return backRect; }
            set
            {
                backRect = value;
                CalculateTextPosition();
            }
        }
        public Texture2D BackTexture
        { get { return backTex; } set { backTex = value; } }
    }
}
