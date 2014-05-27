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
        bool dragging = false;
        Vector2 buttonOffset, buttonPos;
        Rectangle boundingBox;
        //Slider values
        int offsetRight, maxValue;
        int amount = 0;
        float valueWidth;
        //Events
        int prevAmout;
        public event Action OnValueChange;

        public Slider(string name, Vector2 position, Vector2 buttonOffset, int offsetRight, Texture2D sliderBar, Texture2D sliderButton, int values)
            : base(name, position)
        {
            this.bar = sliderBar;
            //Button
            this.button = sliderButton;
            this.buttonOffset = buttonOffset;
            this.buttonPos = Position + buttonOffset;
            this.boundingBox = new Rectangle((int)(Position.X + buttonOffset.X), (int)(Position.Y + buttonOffset.Y), button.Width, button.Height);
            //Slider values
            this.maxValue = values - 1;
            this.valueWidth = (float)(bar.Width - (buttonOffset.X + offsetRight)) / maxValue;
            //Offset
            this.offsetRight = offsetRight;
        }

        public override void Update()
        {
            //Update the previous value
            prevAmout = amount;

            //Update the bouding box
            boundingBox.Location = buttonPos.ToPoint();

            //Start dragging
            if (Input.LeftClicked())
                if (boundingBox.Contains(Input.MousePosition))
                    dragging = true;
            //Stop dragging
            if (!Input.LeftMousePressed())
                dragging = false;

            //Drag the button
            if (dragging)
                amount = (int)MathHelper.Clamp((Input.MousePosition.X - (Vector2.Transform(Position, SE.Graphics.TransformMatrix).X + buttonOffset.X - valueWidth / 2)) / valueWidth, 0, maxValue);

            //Set the button position
            buttonPos = Position + new Vector2(amount * valueWidth - button.Width / 2 + buttonOffset.X, (bar.Height / 2 - button.Height / 2) + buttonOffset.Y);

            //Call the OnValueChange event
            if (prevAmout != amount && OnValueChange != null)
                    OnValueChange();

            base.Update();
        }

        public override void Draw(SpriteBatch s)
        {
            //Draw the bar and button
            s.Draw(bar, Position, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.01f);
            s.Draw(button, buttonPos, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

            base.Draw(s);
        }

        public int Value
        { get { return amount; } set { amount = value; } }
    }
}
