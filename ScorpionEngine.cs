using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using ScorpionEngine.GameObjects;

namespace ScorpionEngine
{
    public static class SE
    {
        #region Fields
        //Time
        static GameTime gameTime;
        static TimeSpan deltaTime;
        static float gameSpeed = 1.0f;
        //Graphics
        static GraphicsDeviceManager graphics;
        static SpriteBatch spriteBatch;
        //Gamestates
        static Dictionary<string, GameState> gameStates = new Dictionary<string, GameState>();
        static GameState currentState;
        //Random
        static Random random;
        #endregion

        #region Methods
        public static void Initialize(GraphicsDeviceManager g, ContentManager c, string consoleFont)
        {
            graphics = g;
            //Create a new spritebatch
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Initialize the assets, input, console
            Assets.Initialize(c, GraphicsDevice);
            Input.Initialize();
            GameConsole.Initialize(consoleFont);

            //Randomness
            random = new Random();
        }

        public static void Update(GameTime g)
        {
            //Update the gametime
            gameTime = g;
            //Update the delta time
            deltaTime = TimeSpan.FromMilliseconds(g.ElapsedGameTime.TotalMilliseconds * gameSpeed);

            //If the current game state is not null, update it
            if (currentState != null)
                currentState.Update();

            //Update the game console
            Input.Update();
            GameConsole.Update();
        }

        public static void Draw()
        {
            //Clear the graphics device
            GraphicsDevice.Clear(Color.Black);
            //Begin the spritebatch
            spriteBatch.Begin();

            //If the current game state is not null, draw it
            if (currentState != null)
                currentState.Draw(spriteBatch);

            //Draw the console
            GameConsole.Draw(spriteBatch);

            //End the spritebatch
            spriteBatch.End();
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
        #endregion

        #region Properties
        //Time
        public static GameTime GameTime
        { get { return gameTime; } }
        public static TimeSpan DeltaTime
        { get { return deltaTime; } }
        public static float GameSpeed
        { get { return gameSpeed; } set { gameSpeed = value; } }
        //Graphics, screen
        public static GraphicsDevice GraphicsDevice
        { get { return graphics.GraphicsDevice; } }
        public static Point Screen
        { get { return new Point(GraphicsDevice.DisplayMode.Width, GraphicsDevice.DisplayMode.Height); } }
        public static Rectangle Viewport
        { get { return GraphicsDevice.Viewport.Bounds; } }
        public static bool Fullscreen
        { get { return graphics.IsFullScreen; } set { graphics.IsFullScreen = value; } }
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
        #endregion
    }
}
