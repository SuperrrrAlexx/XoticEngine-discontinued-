﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XoticEngine.Achievements
{
    public class Achievement
    {
        string name;
        string[] description;
        SpriteFont nameFont, descFont;
        Texture2D picture;
        Color backColor, textColor;
        //Positions
        Vector2 position, namePos, descPos, picPos;
        Rectangle backRect;
        //Achieved
        bool achieved = false;

        public Achievement(string name, string description, SpriteFont nameFont, SpriteFont descFont, Color textColor, Color backColor, Texture2D picture)
        {
            this.name = name;
            this.description = description.Split(new string[] { "\n" }, StringSplitOptions.None);
            this.nameFont = nameFont;
            this.descFont = descFont;
            this.textColor = textColor;
            this.backColor = backColor;
            this.picture = picture;

            //Positions
            backRect = new Rectangle(Graphics.Viewport.Width - 280, Graphics.Viewport.Height, 280, 120);
            position = backRect.Location.ToVector2();
            picPos = new Vector2(10);
            namePos = new Vector2(120, 10);
            descPos = new Vector2(120, nameFont.LineSpacing + 10);
        }

        public void Draw(SpriteBatch s)
        {
            //Draw the text
            s.DrawString(nameFont, name, position + namePos, textColor, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            for (int i = 0; i < description.Count(); i++)
                s.DrawString(descFont, description[i], position + descPos + new Vector2(0, i * descFont.LineSpacing), textColor, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            //Draw the picture
            s.Draw(picture, new Rectangle((int)(position.X + picPos.X), (int)(position.Y + picPos.Y), 100, 100), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            //Draw the background
            s.Draw(Assets.DummyTexture, backRect, null, backColor, 0, Vector2.Zero, SpriteEffects.None, float.Epsilon);
        }

        public string Name
        { get { return name; } }
        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
                backRect.Location = position.ToPoint();
            }
        }
        public bool Achieved
        { get { return achieved; } set { achieved = value; } }
        public Rectangle BoundingBox
        { get { return backRect; } }
    }
}