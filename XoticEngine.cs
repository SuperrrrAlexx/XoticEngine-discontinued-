using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using XoticEngine.Achievements;
using XoticEngine.GameObjects;
using XoticEngine.Utilities;

namespace XoticEngine
{
    public static class X
    {
        //Game class
        private static Game game;
        //Gamestates
        private static Dictionary<string, GameState> gameStates;
        private static GameState currentState;
        //Random
        private static Random random;

        internal static void Initialize(Game g)
        {
            game = g;
            gameStates = new Dictionary<string, GameState>();
            random = new Random();
        }

        internal static void Update(GameTime g)
        {
            //If the current game state is not null, update it
            if (currentState != null)
                currentState.Update();
        }

        public static void AddGameState(GameState g)
        {
            //Check if the gamestate is null
            if (g == null)
                throw new ArgumentNullException("The gamestate can not be null.");
            //Check if the gamestate already exists
            if (gameStates.ContainsKey(g.Name))
                throw new ArgumentException("The gamestate \"" + g.Name + "\" already exists.");

            //Add the gamestate to the list
            gameStates.Add(g.Name, g);
        }
        public static void SwitchTo(string gameStateName)
        {
            //Check if the gamestate exists
            if (!gameStates.ContainsKey(gameStateName))
                throw new KeyNotFoundException("A gamestate with the name \"" + gameStateName + "\" was not found");

            //End the old state
            if (currentState != null)
                currentState.EndState();
            //Switch to the new state
            currentState = gameStates[gameStateName];
            currentState.BeginState();
        }

        //Game properties
        public static bool IsMouseVisible
        { get { return game.IsMouseVisible; } set { game.IsMouseVisible = value; } }
        public static string WindowTitle
        { get { return game.Window.Title; } set { game.Window.Title = value; } }
        public static bool GameActive
        { get { return game.IsActive; } }
        //Gamestates
        public static GameState CurrentState
        { get { return currentState; } }
        public static Dictionary<string, GameState> GameStates
        { get { return gameStates; } }
        //Random
        public static Random Random
        { get { return random; } }
    }
}
