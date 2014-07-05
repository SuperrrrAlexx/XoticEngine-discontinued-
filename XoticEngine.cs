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
        static Game game;
        //Gamestates
        static Dictionary<string, GameState> gameStates;
        static GameState currentState;
        //Random
        static Random random;

        public static void Initialize(Game g)
        {
            game = g;
            gameStates = new Dictionary<string, GameState>();
            random = new Random();
        }

        public static void Update(GameTime g)
        {
            //If the current game state is not null, update it
            if (currentState != null)
                currentState.Update();

            //Update all components
            Input.Update();
            GameConsole.Update();
            AchievementHolder.Update();
            FrameRateCounter.Update();
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
                //End the old state
                if (currentState != null)
                    currentState.EndState();
                //Switch to the new state
                currentState = gameStates[gameStateName];
                currentState.BeginState();
            }
        }

        //Game properties
        public static bool IsMouseVisible
        { get { return game.IsMouseVisible; } set { game.IsMouseVisible = value; } }
        public static string WindowTitle
        { get { return game.Window.Title; } set { game.Window.Title = value; } }
        //Gamestate
        public static GameState CurrentState
        { get { return currentState; } }
        public static Dictionary<string, GameState> GameStates
        { get { return gameStates; } }
        //Random
        public static Random Random
        { get { return random; } }
    }
}
