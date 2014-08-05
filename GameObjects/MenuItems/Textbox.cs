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
    public class Textbox : Label
    {
        //Text cursor
        private Vector2 cursorPos;
        private int cursorTextPos = 0;
        private bool cursorVisible = true;
        //Blinking
        private double blinkTime = 0.5;
        private double blinkTimeLeft = 0.5;
        //Text input
        private bool enabled = true;
        private int maxLines;
        private string baseText;

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
            baseText = text;

            //Set the cursor position
            cursorPos = TextPosition;
            //Calculate how many lines of text can be displayed
            maxLines = Math.Max(1, BackRectangle.Height / Font.LineSpacing);

            //Hook into keyboard input
            KeyboardInput.OnCharEntered += OnCharEntered;
            KeyboardInput.OnKeyPressed += OnKeyPressed;
        }

        private void OnCharEntered(char c)
        {
            if (enabled)
            {
                //Check if the new amount of lines is smaller than maxLines
                string newText = WrapText(text.Insert(cursorTextPos, c.ToString()));
                if (newText.Split(new string[] { "\n" }, StringSplitOptions.None).Length <= maxLines)
                {
                    //Add the character to the text
                    Text = Text.Insert(cursorTextPos, c.ToString());
                    cursorTextPos++;
                }

                //Make the cursor visible
                cursorVisible = true;
                blinkTimeLeft = blinkTime;
            }
        }
        private void OnKeyPressed(Keys k)
        {
            if (enabled)
            {
                switch (k)
                {
                    case Keys.Enter:
                        //If there are less than maxLines lines, add a newline
                        if (text.Split(new string[] { "\n" }, StringSplitOptions.None).Length < maxLines)
                        {
                            Text = Text.Insert(cursorTextPos, "\n");
                            cursorTextPos++;
                        }
                        break;
                    case Keys.Back:
                        //Remove the letter left from the cursor
                        if (cursorTextPos > 0)
                        {
                            Text = Text.Remove(cursorTextPos - 1, 1);
                            cursorTextPos--;
                        }
                        break;
                    case Keys.Delete:
                        //Remove the letter right from the cursor
                        if (cursorTextPos < Text.Length)
                            Text = Text.Remove(cursorTextPos, 1);
                        break;
                    case Keys.Left:
                        //Move the cursor left
                        if (cursorTextPos > 0)
                            cursorTextPos--;
                        break;
                    case Keys.Right:
                        //Move the cursor right
                        if (cursorTextPos < Text.Length)
                            cursorTextPos++;
                        break;
                    case Keys.Home:
                        cursorTextPos = 0;
                        break;
                    case Keys.End:
                        cursorTextPos = Text.Length;
                        break;
                }

                //Make the cursor visible
                cursorVisible = true;
                blinkTimeLeft = blinkTime;
            }
        }

        public override void Update()
        {
            //Get the newline before and after the cursor
            int before = text.LastIndexOf("\n", (int)Math.Max(0, cursorTextPos + cursorOffset - 1));
            int after = text.IndexOf("\n", cursorTextPos + cursorOffset);

            string line = String.Empty;
            //Check if the cursor is at the first or last line
            if (before == -1)
                line = text.Split(new string[] { "\n" }, StringSplitOptions.None).First();
            else if (after == -1)
                line = text.Split(new string[] { "\n" }, StringSplitOptions.None).Last();
            else
                line = text.Substring(before, after - before);

            //Set the text cursor position
            cursorPos = TextPosition + new Vector2(Font.MeasureString(line.Substring(0, cursorTextPos + cursorOffset - (before + 1))).X - 4,
                Font.MeasureString(text.Substring(0, cursorTextPos)).Y - (cursorTextPos > 0 ? Font.MeasureString("|").Y : 0));

            //Blinking cursor
            blinkTimeLeft -= Time.DeltaTime;
            if (blinkTimeLeft <= 0)
            {
                cursorVisible = !cursorVisible;
                blinkTimeLeft = blinkTime;
            }

            base.Update();
        }
        public override void Draw(SpriteBatch gameBatch, SpriteBatch additiveBatch, SpriteBatch guiBatch)
        {
            //Draw the text cursor
            if (cursorVisible)
                guiBatch.DrawString(Font, "|", cursorPos, TextColor, 0, Vector2.Zero, 1, SpriteEffects.None, Depth);

            base.Draw(gameBatch, additiveBatch, guiBatch);
        }

        private string WrapText(string text)
        {
            string wrapped = String.Empty;
            string line = String.Empty;

            //Get all words
            string[] words = baseText.Split(' ');

            for (int i = 0; i < words.Length; i++)
            {
                //Check for newline
                if (words[i] == "\n")
                {
                    wrapped += line + "\n";
                    line = String.Empty;
                }
                //Check if the line is longer than the width of the textbox
                else if (Font.MeasureString(line + words[i]).X >= BackRectangle.Width)
                {
                    wrapped += line + "\n";
                    line = words[i] + " ";
                }
                //Add a word to the line
                else
                    line += words[i] + " ";

                //TODO: Check if the word is longer than the line
            }
            //Return the wrapped text plus the last line
            return wrapped + line;
        }

        private int cursorOffset
        { get { return text.Split(new string[] { "\n" }, StringSplitOptions.None).Length - baseText.Split(new string[] { "\n" }, StringSplitOptions.None).Length; } }
        public override string Text
        {
            get { return baseText; }
            set
            {
                baseText = value;
                text = WrapText(baseText);
            }
        }
        public bool Enabled
        { get { return enabled; } set { enabled = value; } }
        public int MaxLines
        { get { return maxLines; } set { maxLines = value; } }
        public bool Hovering
        { get { return BackRectangle.Contains(MouseInput.Position); } }
    }
}
