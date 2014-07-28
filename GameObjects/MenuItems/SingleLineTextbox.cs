﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XoticEngine.Input;

namespace XoticEngine.GameObjects.MenuItems
{
    public class SingleLineTextbox : Label
    {
        private bool enabled = true;
        //Text cursor
        Vector2 cursorPos;
        int cursorTextPos = 0;
        private bool cursorVisible = true;

        public SingleLineTextbox(string name, Rectangle backRect, float depth, SpriteFont font, Color textColor, Color backColor)
            : base(name, backRect, depth, "", font, textColor, backColor)
        {
            //Set the alignment to top-left
            Alignment = new Alignment(HorizontalAlignment.Left, VerticalAlignment.Top);

            //Set the cursor position
            cursorPos = TextPosition;

            //Hook into keyboard input
            KeyboardInput.OnCharEntered += OnCharEntered;
            KeyboardInput.OnKeyPressed += OnKeyPressed;
        }

        private void OnCharEntered(char c)
        {
            //Add the character to the text
            if (enabled)
            {
                Text += c.ToString();
                cursorTextPos++;
            }
        }
        private void OnKeyPressed(Keys k)
        {
            if (enabled)
            {
                switch (k)
                {
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
            //Set the text cursor position
            cursorPos = TextPosition + new Vector2(Font.MeasureString(Text.Substring(0, cursorTextPos)).X - 4, 0);

            base.Update();
        }
        public override void Draw(SpriteBatch gameBatch, SpriteBatch additiveBatch, SpriteBatch guiBatch)
        {
            //Draw the text cursor
            if (cursorVisible)
                guiBatch.DrawString(Font, "|", cursorPos, TextColor, 0, Vector2.Zero, 1, SpriteEffects.None, Depth);

            base.Draw(gameBatch, additiveBatch, guiBatch);
        }

        public bool Enabled
        { get { return enabled; } set { enabled = value; } }
    }
}
