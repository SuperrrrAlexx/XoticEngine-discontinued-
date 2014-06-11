using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using XoticEngine.GameObjects;
using XoticEngine.Utilities;

namespace XoticEngine
{
    public static class X
    {
        //Game class
        static Game game;
        //Graphics
        static GraphicsDeviceManager graphics;
        static SpriteBatch spriteBatch, noCamSpriteBatch;
        static Matrix transformMatrix;
        //Gamestates
        static Dictionary<string, GameState> gameStates = new Dictionary<string, GameState>();
        static GameState currentState;
        //Random
        static Random random;

        public static void Initialize(Game g, GraphicsDeviceManager device, string consoleFont)
        {
            game = g;
            graphics = device;
            //Create a new spritebatch
            spriteBatch = new SpriteBatch(Graphics.Device);
            noCamSpriteBatch = new SpriteBatch(Graphics.Device);

            //Initialize the assets, input, console
            Assets.Initialize(game.Content, Graphics.Device);
            Input.Initialize();
            if (consoleFont != null)
            {
                GameConsole.Enabled = true;
                GameConsole.Initialize(consoleFont);
            }
            else
                GameConsole.Enabled = false;

            //Randomness
            random = new Random();

            //Set the transform matrix
            transformMatrix = Matrix.Identity;
        }

        public static void Update(GameTime g)
        {
            //Update the gametime
            Time.Update(g);

            //If the current game state is not null, update it
            if (currentState != null)
                currentState.Update();

            //Update the game console
            Input.Update();
            GameConsole.Update();

            //Update the framerate counter
            FrameRateCounter.Update();
        }

        public static void Draw()
        {
            //Clear the graphics device
            Graphics.Device.Clear(Color.Black);

            //Begin the spritebatches
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied, null, null, null, null, transformMatrix);
            noCamSpriteBatch.Begin(SpriteSortMode.BackToFront, null);

            //If the current game state is not null, draw it
            if (currentState != null)
                currentState.Draw(spriteBatch);

            //Draw the console
            GameConsole.Draw(noCamSpriteBatch);

            //End the spritebatches
            spriteBatch.End();
            noCamSpriteBatch.End();

            //Draw the framerate counter
            FrameRateCounter.Draw();
        }

        public static void AddGameState(GameState g)
        {
            //Check if the gamestate already exists
            if (gameStates.ContainsKey(g.Name))
                GameConsole.Error("A gamestate with this name (" + g.Name + ") already exists.");
            else
                //Add the gamestate to the list
                gameStates.Add(g.Name, g);
        }

        public static void SwitchTo(string gameStateName)
        {
            //Check if the gamestate exists
            if (!gameStates.ContainsKey(gameStateName))
                GameConsole.Error("The gamestate \"" + gameStateName + "\" does not exist.");
            else
            {
                //Call EndState on the current state
                if (currentState != null)
                    currentState.EndState();
                //Switch the current state
                currentState = gameStates[gameStateName];
                //Call beginState on the current state
                currentState.BeginState();
            }
        }

        //Graphics, screen
        public struct Graphics
        {
            public static GraphicsDeviceManager DeviceManager
            { get { return graphics; } }
            public static GraphicsDevice Device
            { get { return graphics.GraphicsDevice; } }
            public static Point Screen
            { get { return new Point(Device.DisplayMode.Width, Device.DisplayMode.Height); } }
            public static Rectangle Viewport
            { get { return Device.Viewport.Bounds; } }
            public static bool Fullscreen
            {
                get { return graphics.IsFullScreen; }
                set
                {
                    graphics.IsFullScreen = value;
                    graphics.ApplyChanges();
                }
            }
            public static Matrix TransformMatrix
            { get { return transformMatrix; } set { transformMatrix = value; } }
            public static void ResetTransformMatrix()
            { transformMatrix = Matrix.Identity; }
            public static SpriteBatch NoCamSpriteBatch
            { get { return noCamSpriteBatch; } }
            public static bool VSync
            {
                get { return graphics.SynchronizeWithVerticalRetrace; }
                set
                {
                    graphics.SynchronizeWithVerticalRetrace = value;
                    game.IsFixedTimeStep = value;
                    graphics.ApplyChanges();
                }
            }
        }
        public static bool IsMouseVisible
        { get { return game.IsMouseVisible; } set { game.IsMouseVisible = value; } }
        public static string WindowTitle
        { get { return game.Window.Title; } set { game.Window.Title = value; } }
        //Gamestate
        public static GameState CurrentState
        { get { return currentState; } }
        public static string CurrentStateName
        {
            get
            {
                if (currentState != null)
                    return currentState.Name;
                return "null";
            }
        }
        public static List<string> GameStateNameList
        { get { return new List<string>(gameStates.Keys); } }
        public static List<string> GameObjectNameList
        {
            get
            {
                List<string> gameObjectNames = new List<string>();
                if (currentState != null)
                    for (int i = 0; i < currentState.Count; i++)
                        gameObjectNames.Add(currentState[i].Name);
                return gameObjectNames;
            }
        }
        //Random
        public static Random Random
        { get { return random; } }
    }
}
