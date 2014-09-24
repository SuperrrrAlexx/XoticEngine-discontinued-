using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XoticEngine.Input;

namespace XoticEngine.GameObjects.MenuItems
{
    public class Textbox : Label, IClickable
    {
        //Text cursor
        private Vector2 cursorPos;
        private bool cursorVisible = true;
        private bool showCursor = true;
        //Blinking
        private double blinkTime = 0.5;
        private double blinkTimeLeft = 0.5;
        private bool useRealTime = true;
        //Text input
        private bool enabled = true;
        private int maxLines;
        private string baseText;
        //Click event
        public event MouseInput.ClickEvent OnClick;

        public Textbox(string name, Rectangle backRect, float depth, SpriteFont font, Color textColor, Color backColor)
            : base(name, backRect, depth, "", font, textColor, backColor)
        {
            Create();
        }
        public Textbox(string name, Rectangle backRect, float depth, SpriteFont font, Color textColor, Texture2D background, Color backColor)
            : base(name, backRect, depth, "", font, textColor, background, backColor)
        {
            Create();
        }

        private void Create()
        {
            //Set the alignment to top-left
            Alignment = new Alignment(HorizontalAlignment.Left, VerticalAlignment.Top);

            //Set the base text
            Text = text;

            //Set the cursor position
            cursorPos = TextPosition;
            //Calculate how many lines of text can be displayed
            maxLines = Math.Max(1, BackRectangle.Height / Font.LineSpacing);

            //Hook into keyboard and mouse input
            KeyboardInput.OnCharEntered += OnCharEntered;
            KeyboardInput.OnKeyPressed += OnKeyPressed;
            MouseInput.OnClick += OnMouseClick;
        }

        private void OnMouseClick(object sender, ClickEventArgs e)
        {
            //Fire the event
            if (ClickRectangle.Contains(MouseInput.Position) && OnClick != null)
                OnClick(this, e);
        }
        private void OnCharEntered(object sender, CharEventArgs c)
        {
            if (enabled)
            {
                //Check if the new amount of lines is smaller than maxLines, add the character to the text
                string newText = Font.Wrap(baseText + c.Character, BackRectangle.Width);
                if (newText.Split(new string[] { "\n" }, StringSplitOptions.None).Length <= maxLines)
                    Text += c.Character;

                //Make the cursor visible
                cursorVisible = true;
                blinkTimeLeft = blinkTime;
            }
        }
        private void OnKeyPressed(object sender, KeyEventArgs k)
        {
            if (enabled)
            {
                switch (k.Key)
                {
                    case Keys.Enter:
                        //If there are less than maxLines lines, add a newline
                        if (text.Split(new string[] { "\n" }, StringSplitOptions.None).Length < maxLines)
                            Text += "\n";
                        break;
                    case Keys.Back:
                        //Remove the last letter
                        if (Text.Length > 0)
                            Text = Text.Remove(Text.Length - 1, 1);
                        break;
                }

                //Make the cursor visible
                cursorVisible = true;
                blinkTimeLeft = blinkTime;
            }
        }

        public override void Update()
        {
            //Get the last line
            string line = text.Split(new string[] { "\n" }, StringSplitOptions.None).Last();

            //Set the text cursor position
            cursorPos = TextPosition + new Vector2(Font.MeasureString(line).X - Font.MeasureString("|").X, Font.MeasureString(text).Y - Font.MeasureString("|").Y);

            //Blinking cursor
            if (showCursor)
            {
                blinkTimeLeft -= useRealTime ? Time.RealTime : Time.DeltaTime;
                if (blinkTimeLeft <= 0)
                {
                    cursorVisible = !cursorVisible;
                    blinkTimeLeft = blinkTime;
                }
            }

            base.Update();
        }
        public override void Draw(SpriteBatchHolder spriteBatches)
        {
            //Draw the text cursor
            if (showCursor && cursorVisible)
                spriteBatches[DrawModes.Gui].DrawString(Font, "|", cursorPos, TextColor, 0, Vector2.Zero, 1, SpriteEffects.None, Depth);

            base.Draw(spriteBatches);
        }

        public override string Text
        {
            get { return baseText; }
            set
            {
                baseText = value;
                text = Font.Wrap(baseText, BackRectangle.Width);
            }
        }
        public bool Enabled
        {
            get { return enabled; }
            set
            {
                enabled = value;
                showCursor = value;
            }
        }
        public bool ShowCursor
        {
            get { return showCursor; }
            set
            {
                showCursor = value;
                if (showCursor)
                    blinkTimeLeft = blinkTime;
            }
        }
        public int MaxLines
        { get { return maxLines; } set { maxLines = value; } }
        public bool UseRealTime
        { get { return useRealTime; } set { useRealTime = value; } }
        public Rectangle ClickRectangle
        { get { return BackRectangle; } }
    }
}
