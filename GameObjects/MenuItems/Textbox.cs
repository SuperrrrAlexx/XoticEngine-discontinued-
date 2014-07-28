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
        //Text input
        private bool enabled = true;
        private int maxLines;

        public Textbox(string name, Rectangle backRect, float depth, SpriteFont font, Color textColor, Color backColor)
            : base(name, backRect, depth, "", font, textColor, backColor)
        {
            //Set the alignment to top-left
            Alignment = new Alignment(HorizontalAlignment.Left, VerticalAlignment.Top);

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
            //Add the character to the text
            if (enabled)
            {
                Text = Text.Insert(cursorTextPos, c.ToString());
                cursorTextPos++;
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
                        if (Text.Split(new string[] { "\n" }, StringSplitOptions.None).Length < maxLines)
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
            }
        }

        public override void Update()
        {
            //Get the newline before and after the cursor
            int before = Text.LastIndexOf("\n", (int)Math.Max(0, cursorTextPos - 1));
            int after = Text.IndexOf("\n", cursorTextPos);

            string line = "";
            //Check if the cursor is at the first or last line
            if (before == -1)
                line = Text.Split(new string[] { "\n" }, StringSplitOptions.None).First();
            else if (after == -1)
                line = Text.Split(new string[] { "\n" }, StringSplitOptions.None).Last();
            else
                line = Text.Substring(before, after - before);

            //Set the text cursor position
            cursorPos = TextPosition + new Vector2(Font.MeasureString(line.Substring(0, cursorTextPos - (before + 1))).X - 4,
                Font.MeasureString(Text.Substring(0, cursorTextPos)).Y - (cursorTextPos > 0 ? Font.MeasureString("|").Y : 0));

            base.Update();
        }
        public override void Draw(SpriteBatch gameBatch, SpriteBatch additiveBatch, SpriteBatch guiBatch)
        {
            //Draw the text cursor
            if (cursorVisible)
                guiBatch.DrawString(Font, "|", cursorPos, TextColor, 0, Vector2.Zero, 1, SpriteEffects.None, Depth);

            base.Draw(gameBatch, additiveBatch, guiBatch);
        }

        private int FirstIndexBefore(string value, int startIndex)
        {
            return 0;
        }

        public bool Enabled
        { get { return enabled; } set { enabled = value; } }
        public int MaxLines
        { get { return maxLines; } set { maxLines = value; } }
    }
}
