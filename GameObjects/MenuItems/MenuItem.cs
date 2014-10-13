using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XoticEngine.Input;

namespace XoticEngine.GameObjects.MenuItems
{
    public abstract class MenuItem : GameObject, IClickable
    {
        protected Rectangle clickRect;
        //Click event
        private bool hovering, leftDown, middleDown, rightDown;
        public event InputManager.ClickEvent OnClick;

        public MenuItem(string name, Rectangle backRect)
            : base(name, backRect.Location.ToVector2())
        {
            this.clickRect = backRect;
        }
        public MenuItem(string name, Rectangle backRect, float rotation, Vector2 origin, float depth)
            : base(name, backRect.Location.ToVector2(), rotation, origin, depth)
        {
            this.clickRect = backRect;
        }
        public MenuItem(string name, Rectangle backRect, float rotation, Vector2 origin, Vector2 scale, float depth)
            : base(name, backRect.Location.ToVector2(), rotation, origin, scale, depth)
        {
            this.clickRect = backRect;
        }

        public override void Update()
        {
            //Check if the mouse is within the rectangle
            if (ClickRectangle.Contains(InputManager.Mouse.Position))
            {
                hovering = true;

                //Check for mouse events
                CheckEvents();
            }
            //Check if the mouse was previously hovering
            else if (hovering)
            {
                hovering = false;
                leftDown = false;
                middleDown = false;
                rightDown = false;
            }

            base.Update();
        }
        private void CheckEvents()
        {
            //Check for mouse presses
            if (InputManager.Mouse.LeftPressed())
                leftDown = true;
            if (InputManager.Mouse.MiddlePressed())
                middleDown = true;
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
                if (middleDown && InputManager.Mouse.MiddleReleased())
                {
                    OnClick(this, new ClickEventArgs(MouseButton.Middle, InputManager.Mouse.Position));
                    middleDown = false;
                }
                if (rightDown && InputManager.Mouse.RightReleased())
                {
                    OnClick(this, new ClickEventArgs(MouseButton.Right, InputManager.Mouse.Position));
                    rightDown = false;
                }
            }
        }

        public override Vector2 RelativePosition
        {
            get { return base.RelativePosition; }
            set
            {
                base.RelativePosition = value;
                clickRect.Location = Position.ToPoint();
            }
        }
        public Rectangle ClickRectangle
        { get { return clickRect; } }
        public bool Hovering
        { get { return hovering; } }
        public bool LeftMouseDown
        { get { return leftDown; } }
        public bool MiddleMouseDown
        { get { return middleDown; } }
        public bool RightMouseDown
        { get { return rightDown; } }
    }
}
