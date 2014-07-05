using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XoticEngine
{
    public class XGame : Game
    {
        protected GraphicsDeviceManager graphics;

        public XGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            //Initialize everything
            X.Initialize(this);
            Graphics.Initialize(graphics);
            Assets.Initialize(Content);
            Input.Initialize();

            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            //Save the game time
            Time.Update(gameTime);
            
            //Update X
            X.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //Save the game time
            Time.Update(gameTime);

            //Draw everything
            Graphics.DrawAll();

            base.Draw(gameTime);
        }
    }
}
