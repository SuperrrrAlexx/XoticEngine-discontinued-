using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XoticEngine
{
    public class XGame : Game
    {
        GraphicsDeviceManager graphics;

        public XGame()
        {
            graphics = new GraphicsDeviceManager(this);
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
            //Update X
            X.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //Draw everything
            Graphics.DrawAll();

            base.Draw(gameTime);
        }
    }
}
