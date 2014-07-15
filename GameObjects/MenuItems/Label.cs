using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XoticEngine.GameObjects.MenuItems
{
    public class Label : GameObject
    {
        string text;
        SpriteFont font;
        Color textColor;

        public Label(string name, Vector2 position, float depth, string text, SpriteFont font, Color textColor)
            : base(name, position, 0, Vector2.Zero, depth)
        {
            this.text = text;
            this.font = font;
            this.textColor = textColor;
        }

        public override void Draw(SpriteBatch gameBatch, SpriteBatch additiveBatch, SpriteBatch guiBatch)
        {
            //Draw the text
            guiBatch.DrawString(font, text, Position, textColor, 0, Vector2.Zero, 1, SpriteEffects.None, Depth);
            
            base.Draw(gameBatch, additiveBatch, guiBatch);
        }

        public string Text
        { get { return text; } set { text = value; } }
        public SpriteFont Font
        { get { return font; } set { font = value; } }
        public Color TextColor
        { get { return textColor; } set { textColor = value; } }
    }
}
