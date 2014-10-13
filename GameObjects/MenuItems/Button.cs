using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XoticEngine.Input;

namespace XoticEngine.GameObjects.MenuItems
{
    public class Button : Label, IClickable
    {
        //Background
        private SpriteSheet backTexture;
        //Colors
        private Color[] backColors, textColors;

        public Button(string name, Rectangle backRect, float depth, string text, SpriteFont font, Color[] textColors, Color[] backColors)
            : base(name, backRect, depth, text, font, textColors != null ? textColors[0] : Color.Black, backColors != null ? backColors[0] : Color.White)
        {
            this.textColors = textColors;
            this.backTexture = null;
            this.backColors = backColors;
        }
        public Button(string name, Rectangle backRect, float depth, string text, SpriteFont font, Color[] textColors, SpriteSheet backTexture, Color[] backColors)
            : base(name, backRect, depth, text, font, textColors != null ? textColors[0] : Color.Black, backTexture[0], backColors != null ? backColors[0] : Color.White)
        {
            this.textColors = textColors;
            this.backTexture = backTexture;
            this.backColors = backColors;
        }


        public override void Update()
        {
            base.Update();

            //Set the textures/colors
            if (Hovering)
            {
                BackTexture = backTexture != null ? backTexture[(int)MathHelper.Clamp(1, 0, backTexture.Length - 1)] : Assets.DummyTexture;
                BackColor = backColors != null ? backColors[(int)MathHelper.Clamp(1, 0, backColors.Length - 1)] : Color.White;
                TextColor = textColors != null ? textColors[(int)MathHelper.Clamp(1, 0, textColors.Length - 1)] : Color.Black;
            }
            else
            {
                BackTexture = backTexture != null ? backTexture[0] : Assets.DummyTexture;
                BackColor = backColors != null ? backColors[0] : Color.White;
                TextColor = textColors != null ? textColors[0] : Color.Black;
            }
        }
    }
}