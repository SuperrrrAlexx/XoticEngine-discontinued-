using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XoticEngine.Input;

namespace XoticEngine.GameObjects.MenuItems
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
        int offsetRight, maxValue, prevAmount;
        int amount = 0;
        float valueWidth;
        //Event
        public event Action OnValueChange;

        public Slider(string name, Vector2 position, float depth, Vector2 buttonOffset, int offsetRight, Texture2D sliderBar, Texture2D sliderButton, int values)
            : base(name, position, 0, Vector2.Zero, depth)
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
            prevAmount = amount;

            //Update the bouding box
            boundingBox.Location = buttonPos.ToPoint();

            //Start/stop dragging
            if (MouseInput.LeftClicked())
                if (boundingBox.Contains(MouseInput.Position))
                    dragging = true;
            if (!MouseInput.LeftDown())
                dragging = false;

            //Drag the button
            if (dragging)
                amount = (int)MathHelper.Clamp((MouseInput.Position.X - (Position.X + buttonOffset.X - valueWidth / 2)) / valueWidth, 0, maxValue);

            //Set the button position
            buttonPos = Position + new Vector2(amount * valueWidth - button.Width / 2 + buttonOffset.X, (bar.Height / 2 - button.Height / 2) + buttonOffset.Y);

            //Call the OnValueChange event
            if (prevAmount != amount && OnValueChange != null)
                    OnValueChange();

            base.Update();
        }

        public override void Draw(SpriteBatch gameBatch, SpriteBatch additiveBatch, SpriteBatch guiBatch)
        {
            //Draw the bar and button
            guiBatch.Draw(bar, Position, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0 + float.Epsilon);
            guiBatch.Draw(button, buttonPos, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

            base.Draw(gameBatch, additiveBatch, guiBatch);
        }

        public int Value
        { get { return amount; } set { amount = value; } }
        public bool Dragging
        { get { return dragging; } }
    }
}
