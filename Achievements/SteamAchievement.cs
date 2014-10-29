using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XoticEngine.Components;

namespace XoticEngine.Achievements
{
    public class SteamAchievement : IAchievement
    {
        private const float speed = 250;
        private bool achieved = false;
        private bool done = false;
        private readonly string name;
        //Text
        private SpriteFont titleFont, descFont;
        private Color titleColor, descColor;
        private string title, desc;
        //Drawing
        private Texture2D icon;
        private Rectangle backRect, imageRect;
        private Vector2 position;
        private Color backColor;
        //Timing
        private Timer showTimer;
        private string move;

        public SteamAchievement(string name, Color backColor, Texture2D icon, SpriteFont titleFont, Color titleColor, string title, SpriteFont descFont, Color descColor, string desc)
        {
            this.name = name;
            this.backColor = backColor;
            this.icon = icon;
            this.titleFont = titleFont;
            this.titleColor = titleColor;
            this.title = title;
            this.descFont = descFont;
            this.descColor = descColor;
            this.desc = desc;
            showTimer = new Timer(5.0, false) { Enabled = false, UseRealTime = true };
            showTimer.Tick += TimerTick;
            move = "up";
            this.backRect = new Rectangle(Graphics.Viewport.Width - 300, Graphics.Viewport.Height, 300, 120);
            this.position = backRect.Location.ToVector2();
            this.imageRect = new Rectangle(0, 0, backRect.Height - 20, backRect.Height - 20);
        }

        public void Update()
        {
            switch (move)
            {
                case "up":
                    //Move up
                    position.Y -= speed * TimeF.RealTime;

                    //Check if the achievement is fully on the screen
                    if (position.Y <= Graphics.Viewport.Height - backRect.Height)
                    {
                        position.Y = Graphics.Viewport.Height - backRect.Height;
                        move = "show";
                        showTimer.Enabled = true;
                    }
                    break;
                case "down":
                    //Move down
                    position.Y += speed * TimeF.RealTime;

                    //Check if the achievement is fully off the screen
                    if (position.Y >= Graphics.Viewport.Height)
                        done = true;
                    break;
            }

            //Set the back and image rectangle locations
            backRect.Location = position.ToPoint();
            imageRect.Location = (position + new Vector2(10)).ToPoint();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            //Change move to down
            move = "down";
        }

        public void Draw(SpriteBatchHolder s)
        {
            //Draw the back rectangle and icon
            s[DrawModes.AlphaBlend].Draw(Assets.DummyTexture, backRect, null, backColor, 0, Vector2.Zero, SpriteEffects.None, float.Epsilon);
            s[DrawModes.AlphaBlend].Draw(icon, imageRect, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            //Draw the title and text
            s[DrawModes.AlphaBlend].DrawString(titleFont, title, new Vector2(imageRect.Right + 10, imageRect.Top), titleColor, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            s[DrawModes.AlphaBlend].DrawString(descFont, desc, new Vector2(imageRect.Right + 10, imageRect.Top + titleFont.LineSpacing), descColor, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
        }

        public string Name
        { get { return name; } }
        public bool Achieved
        { get { return achieved; } set { achieved = value; } }
        public bool Done
        { get { return done; } }
    }
}
