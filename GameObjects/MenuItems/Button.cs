using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XoticEngine.Input;

namespace XoticEngine.GameObjects.MenuItems
{
    public class Button : Label
    {
        //Background
        SpriteSheet backTexture;
        //Colors
        Color[] backColors, textColors;
        //Events
        bool hovering, leftDown, rightDown;
        public event Action OnLeftPress, OnRightPress, OnLeftRelease, OnRightRelease, OnMouseEnter, OnMouseExit;

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
            //Check if the mouse is within the rectangle
            if (BackRectangle.Contains(MouseInput.Position))
            {
                //Check if the mouse was previously not hovering
                if (!hovering)
                {
                    hovering = true;

                    //Call OnMouseEnter
                    CallAction(OnMouseEnter);

                    //Set the textures/colors
                    BackTexture = backTexture != null ? backTexture[(int)MathHelper.Clamp(1, 0, backTexture.Length - 1)] : Assets.DummyTexture;
                    BackColor = backColors != null ? backColors[(int)MathHelper.Clamp(1, 0, backColors.Length - 1)] : Color.White;
                    TextColor = textColors != null ? textColors[(int)MathHelper.Clamp(1, 0, textColors.Length - 1)] : Color.Black;
                }

                //Check for mouse events
                CheckEvents();
            }
            //Check if the mouse was previously hovering
            else if (hovering)
            {
                hovering = false;
                leftDown = false;
                rightDown = false;

                //Call OnMouseExit
                CallAction(OnMouseExit);

                //Set the textures/colors
                BackTexture = backTexture != null ? backTexture[0] : Assets.DummyTexture;
                BackColor = backColors != null ? backColors[0] : Color.White;
                TextColor = textColors != null ? textColors[0] : Color.Black;
            }

            base.Update();
        }

        private void CheckEvents()
        {
            //Check for mouse presses
            if (MouseInput.LeftPressed())
            {
                CallAction(OnLeftPress);
                leftDown = true;
            }
            if (MouseInput.RightPressed())
            {
                CallAction(OnRightPress);
                rightDown = true;
            }

            //Check for mouse releases
            if (leftDown && MouseInput.LeftReleased())
            {
                CallAction(OnLeftRelease);
                leftDown = false;
            }
            if (rightDown && MouseInput.RightReleased())
            {
                CallAction(OnRightRelease);
                rightDown = false;
            }
        }

        private void CallAction(Action action)
        {
            if (action != null)
                action();
        }

        public bool Hovering
        { get { return hovering; } }
    }
}