using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XoticEngine.Input;

namespace XoticEngine.GameObjects.MenuItems
{
    public class Slider : MenuItem
    {
        //Background
        Texture2D sliderBar, button;
        //Rectangles
        Rectangle backRect;
        //Colors
        Color barColor, buttonColor;
        //Button
        Vector2 buttonOffset;
        //Slider values
        int offsetRight, maxValue, prevAmount;
        int amount = 0;
        float valueWidth;
        //Event
        public delegate void ValueEvent(object sender, SliderEventArgs s);
        public event ValueEvent OnValueChange;

        public Slider(string name, Rectangle backRect, float depth, Vector2 buttonOffset, int offsetRight, Texture2D sliderBar, Texture2D sliderButton, int values)
            : base(name, backRect, 0, Vector2.Zero, depth)
        {
            //Background
            this.sliderBar = sliderBar;
            this.backRect = backRect;
            //Button
            this.button = sliderButton;
            //Colors
            this.barColor = Color.White;
            this.buttonColor = Color.White;
            //Offset
            this.buttonOffset = buttonOffset;
            this.offsetRight = offsetRight;
            this.clickRect = new Rectangle(0, 0, button.Width, button.Height);
            this.clickRect.Location = (Position + buttonOffset + new Vector2(-button.Width / 2 + buttonOffset.X, backRect.Height / 2 - button.Height / 2 + buttonOffset.Y)).ToPoint();
            //Slider values
            this.maxValue = values - 1;
            this.valueWidth = (float)(backRect.Width - (buttonOffset.X + offsetRight)) / maxValue;
        }
        public Slider(string name, Rectangle backRect, float depth, Vector2 buttonOffset, int offsetRight, Texture2D sliderBar, Color barColor, Texture2D sliderButton, Color buttonColor, int values)
            : base(name, backRect, 0, Vector2.Zero, depth)
        {
            //Background
            this.sliderBar = sliderBar;
            this.backRect = backRect;
            //Button
            this.button = sliderButton;
            //Colors
            this.barColor = barColor;
            this.buttonColor = buttonColor;
            //Offset
            this.buttonOffset = buttonOffset;
            this.offsetRight = offsetRight;
            this.clickRect = new Rectangle(0, 0, button.Width, button.Height);
            this.clickRect.Location = (Position + buttonOffset + new Vector2(-button.Width / 2 + buttonOffset.X, backRect.Height / 2 - button.Height / 2 + buttonOffset.Y)).ToPoint();
            //Slider values
            this.maxValue = values - 1;
            this.valueWidth = (float)(backRect.Width - (buttonOffset.X + offsetRight)) / maxValue;
        }

        public override void Update()
        {
            //Update the previous value
            prevAmount = amount;

            //Drag the button
            if (LeftMouseDown)
                amount = (int)MathHelper.Clamp((InputManager.Mouse.Position.X - (Position.X + buttonOffset.X - valueWidth / 2)) / valueWidth, 0, maxValue);

            //Set the button position and bounding box
            clickRect.Location = new Point((int)(Position.X + buttonOffset.X + amount * valueWidth - button.Width / 2), clickRect.Location.Y);

            //Call the OnValueChange event
            if (prevAmount != amount && OnValueChange != null)
                    OnValueChange(this, new SliderEventArgs(amount));

            base.Update();
        }
        public override void Draw(SpriteBatchHolder spriteBatches)
        {
            //Draw the bar and button
            spriteBatches[DrawModes.Gui].Draw(sliderBar, backRect, null, barColor, 0, Vector2.Zero, SpriteEffects.None, MathHelper.Clamp(Depth + float.Epsilon, 0, 1));
            spriteBatches[DrawModes.Gui].Draw(button, clickRect, null, buttonColor, 0, Vector2.Zero, SpriteEffects.None, Depth);

            base.Draw(spriteBatches);
        }

        public override Vector2 RelativePosition
        {
            get { return base.RelativePosition; }
            set
            {
                base.RelativePosition = value;
                //Set the background and button locations
                backRect.Location = Position.ToPoint();
                clickRect.Location = (Position + buttonOffset + new Vector2(amount * valueWidth - button.Width / 2, backRect.Height / 2 - button.Height / 2 + buttonOffset.Y)).ToPoint();
            }
        }
        public int Value
        { get { return amount; } set { amount = value; } }
        //Colors
        public Color BarColor
        { get { return barColor; } set { barColor = value; } }
        public Color ButtonColor
        { get { return buttonColor; } set { buttonColor = value; } }
    }
}
