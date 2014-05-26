using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ScorpionEngine.GameObjects.MenuItems
{
    public class Slider : GameObject
    {
        //Background
        Texture2D bar, button;
        //Button
        Rectangle buttonRect;
        bool dragging = false;
        Vector2 buttonPos;
        //Slider values
        int maxValue;
        int amount = 0;
        float valueWidth;
        //Offsets
        int offsetLeft, offsetRight, yOffset;
        //Events
        int prevAmout;
        public event Action OnValueChange;

        public Slider(string name, Vector2 position, Texture2D sliderBar, Texture2D sliderButton, int offsetLeft, int offsetRight, int yOffset, int values)
            : base(name, position)
        {
            this.bar = sliderBar;
            this.button = sliderButton;
            this.buttonRect = new Rectangle((int)RelativePosition.X, (int)RelativePosition.Y, button.Width, button.Height);
            this.buttonPos = RelativePosition;
            //Slider values
            this.maxValue = values - 1;
            this.valueWidth = (float)(bar.Width - (offsetLeft + offsetRight)) / maxValue;
            //Offsets
            this.offsetLeft = offsetLeft;
            this.offsetRight = offsetRight;
            this.yOffset = yOffset;
        }

        public override void Update()
        {
            //Update the previous value
            prevAmout = amount;

            //Update the bouding box
            Rectangle boundingBox = buttonRect;
            boundingBox.Location = Vector2.Transform(buttonRect.Location.ToVector2(), SE.Graphics.TransformMatrix).ToPoint();

            //Start dragging
            if (Input.LeftClicked())
                if (boundingBox.Contains(Input.MousePosition))
                    dragging = true;
            //Stop dragging
            if (!Input.LeftMousePressed())
                dragging = false;

            //Drag the button
            if (dragging)
                amount = (int)MathHelper.Clamp((Input.MousePosition.X - ((boundingBox.X + offsetLeft) - valueWidth / 2)) / valueWidth, 0, maxValue);

            //Set the button position
            buttonPos = new Vector2(amount * valueWidth - button.Width / 2 + offsetLeft, (bar.Height / 2 - button.Height / 2) + yOffset);
            //Update the button rectangle
            buttonRect.Location = (RelativePosition + buttonPos).ToPoint();

            //Call the OnValueChange event
            if (prevAmout != amount)
                if (OnValueChange != null)
                    OnValueChange();

            base.Update();
        }

        public override void Draw(SpriteBatch s)
        {
            //Draw the bar and button
            s.Draw(bar, RelativePosition, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.01f);
            s.Draw(button, buttonRect, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);

            base.Draw(s);
        }

        public int Value
        { get { return amount; } set { amount = value; } }
    }
}
