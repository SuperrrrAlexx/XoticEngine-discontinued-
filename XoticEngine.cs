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
        //Gamestates
        static Dictionary<string, GameState> gameStates = new Dictionary<string, GameState>();
        static GameState currentState;
        //Random
        static Random random;

        public static void Initialize(Game g)
        {
            //Save the game
            game = g;

            //Randomness
            random = new Random();
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
