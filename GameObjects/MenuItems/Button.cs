using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XoticEngine.GameObjects.MenuItems
{
    public class Button : GameObject
    {
        //Background
        Rectangle rect;
        Texture2D texture, hoverTexture;
        //Colors
        Color backColor, backHoverColor;
        Color textColor, textHoverColor;
        //Text
        string text;
        SpriteFont font;
        //Actions
        bool hovering = false;
        public event Action OnLeftClick, OnRightClick, OnMiddleClick;
        public event Action OnMouseEnter, OnMouseExit;

        public Button(string name, Rectangle rect, SpriteSheet backTexture, string text, SpriteFont font, Color textColor, Color textHoverColor)
            : base(name, new Vector2(rect.X, rect.Y), 0)
        {
            this.rect = rect;

            //Text
            this.text = text;
            this.font = font;
            //Text color
            this.textColor = textColor;
            this.textHoverColor = textHoverColor;

            //Back texture
            this.texture = backTexture[0];
            this.hoverTexture = backTexture[1];
            //Back color
            this.backColor = Color.White;
            this.backHoverColor = Color.White;
        }
        public Button(string name, Rectangle rect, Color[] backColors, string text, SpriteFont font, Color textColor, Color textHoverColor)
            : base(name, new Vector2(rect.X, rect.Y), 0)
        {
            this.rect = rect;

            //Text
            this.text = text;
            this.font = font;
            //Text color
            this.textColor = textColor;
            this.textHoverColor = textHoverColor;

            //Back texture
            this.texture = Assets.DummyTexture;
            this.hoverTexture = Assets.DummyTexture;
            //Back color
            this.backColor = backColors[0];
            this.backHoverColor = backColors[1];
        }

        public override void Update()
        {
            //Check if the mouse is within the rectangle
            if (rect.Contains(Input.MousePosition))
            {
                //If the mouse was previously not hovering, call OnMouseEnter
                if (!hovering)
                    CallAction(OnMouseEnter);
                //Update hovering
                hovering = true;

                //Check for clicks
                if (Input.LeftClicked())
                    CallAction(OnLeftClick);
                if (Input.RightClicked())
                    CallAction(OnRightClick);
                if (Input.MiddleClicked())
                    CallAction(OnMiddleClick);
            }
            else
            {
                //If the mouse was previously hovering, call OnMouseExit
                if (hovering)
                    CallAction(OnMouseExit);
                //Update hovering
                hovering = false;
            }

            base.Update();
        }

        public override void Draw(SpriteBatch gameBatch, SpriteBatch guiBatch)
        {
            //Draw the button
            guiBatch.Draw(hovering ? hoverTexture : texture, rect, null, hovering ? backHoverColor : backColor, 0, Vector2.Zero, SpriteEffects.None, 0 + float.Epsilon);

            //Draw the text
            if (font != null)
                guiBatch.DrawString(font, text, new Vector2(rect.Center.X - font.MeasureString(text).X / 2, rect.Center.Y - font.MeasureString(text).Y / 2), hovering ? textHoverColor : textColor, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

            base.Draw(gameBatch, guiBatch);
        }

        void CallAction(Action action)
        {
            if (action != null)
                action();
        }

        public bool Hovering
        { get { return hovering; } }
    }
}