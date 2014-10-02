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
        //Events
        private bool hovering, leftDown, rightDown;
        public event InputManager.ClickEvent OnClick;

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
            if (ClickRectangle.Contains(InputManager.Mouse.Position))
            {
                //Check if the mouse was previously not hovering
                if (!hovering)
                {
                    hovering = true;

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
            if (InputManager.Mouse.LeftPressed())
                leftDown = true;
            if (InputManager.Mouse.RightPressed())
                rightDown = true;

            if (OnClick != null)
            {
                //Check for mouse releases
                if (leftDown && InputManager.Mouse.LeftReleased())
                {
                    OnClick(this, new ClickEventArgs(MouseButton.Left, InputManager.Mouse.Position));
                    leftDown = false;
                }
                if (rightDown && InputManager.Mouse.RightReleased())
                {
                    OnClick(this, new ClickEventArgs(MouseButton.Right, InputManager.Mouse.Position));
                    rightDown = false;
                }
            }
        }

        public bool Hovering
        { get { return hovering; } }
        public bool LeftMouseDown
        { get { return leftDown; } }
        public bool RightMouseDown
        { get { return rightDown; } }
        public Rectangle ClickRectangle
        { get { return BackRectangle; } }
    }
}