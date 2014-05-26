using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ScorpionEngine.GameObjects.MenuItems
{
    public class Button : GameObject
    {
        //Background
        Rectangle rect;
        Texture2D texture;
        Texture2D hoverTexture;
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

        public Button(string name, Rectangle rect, SpriteSheet backTexture, bool useHoverTexture, Color backColor, Color backHoverColor, string text, SpriteFont font, Color textColor, Color textHoverColor)
            : base(name, new Vector2(rect.X, rect.Y))
        {
            this.rect = rect;
            //Color
            this.backColor = backColor;
            this.backHoverColor = backHoverColor;
            //Text
            this.text = text;
            this.font = font;
            //Text color
            this.textColor = textColor;
            this.textHoverColor = textHoverColor;

            //Set the texture
            if (backTexture != null)
            {
                this.texture = backTexture.Get(0);
                this.hoverTexture = backTexture.Get(useHoverTexture ? 1 : 0);
            }
            else
            {
                this.texture = Assets.Get<Texture2D>("DummyTexture");
                this.hoverTexture = Assets.Get<Texture2D>("DummyTexture");
            }
        }

        public override void Update()
        {
            //Update the position
            rect.Location = new Point((int)RelativePosition.X, (int)RelativePosition.Y);

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

        public override void Draw(SpriteBatch s)
        {
            //Draw the button
            s.Draw(hovering ? hoverTexture : texture, rect, null, hovering ? backHoverColor : backColor, 0, Vector2.Zero, SpriteEffects.None, 0);

            //Draw the text
            if (font != null)
                s.DrawString(font, text, new Vector2(rect.Center.X - font.MeasureString(text).X / 2, rect.Center.Y - font.MeasureString(text).Y / 2), hovering ? textHoverColor : textColor, 0, Vector2.Zero, 1, SpriteEffects.None, 0.01f);

            base.Draw(s);
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