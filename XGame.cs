using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XoticEngine.Achievements;
using XoticEngine.Components;
using XoticEngine.Input;
using XoticEngine.Utilities;

namespace XoticEngine
{
    public class XGame : Game
    {
        protected GraphicsDeviceManager graphics;

        public XGame(int windowWidth = 1280, int windowHeight = 720)
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = windowWidth;
            graphics.PreferredBackBufferHeight = windowHeight;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            //Initialize everything
            X.Initialize(this);
            Graphics.Initialize(graphics);
            Assets.Initialize(Content);
            InputManager.Initialize();

            base.Initialize();
        }

        //Update and draw
        protected override void Update(GameTime gameTime)
        {
            //Save the game time
            Time.Update(gameTime);
            
            //Update X
            X.Update(gameTime);

            //Update all components
            InputManager.Update();
            GameConsole.Update();
            AchievementHolder.Update();
            FrameRateCounter.Update();
            Timer.UpdateAll();
            Stopwatch.UpdateAll();

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            //Save the game time
            Time.Update(gameTime);

            //Draw everything
            Graphics.Draw();

            base.Draw(gameTime);
        }
    }
}
