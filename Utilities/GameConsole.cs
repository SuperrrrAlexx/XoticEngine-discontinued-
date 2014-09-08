using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XoticEngine.EventArguments;
using XoticEngine.GameObjects;
using XoticEngine.GameObjects.MenuItems;
using XoticEngine.Input;

namespace XoticEngine.Utilities
{
    public static class GameConsole
    {
        private static bool enabled, visible;
        //Input
        private static Textbox inputBox;
        private static SpriteFont font;
        //Background
        private static Rectangle backRect;
        private static Color backColor;
        //Log, commands
        private static List<Tuple<string, Color, string>> log;
        private static Dictionary<string, Action<string[]>> commands;

        static GameConsole()
        {
            backRect = new Rectangle(0, 0, Graphics.Viewport.Width, (int)(Graphics.Viewport.Height * 0.6));
            backColor = Color.Black * 0.6f;

            //Create the lists
            log = new List<Tuple<string, Color, string>>();
            commands = new Dictionary<string, Action<string[]>>();
        }

        public static void Initialize(SpriteFont consoleFont)
        {
            enabled = true;
            font = consoleFont;

            //Create the textbox
            inputBox = new Textbox("Command line", new Rectangle(0, backRect.Bottom, backRect.Width, 20), 0, consoleFont, Color.White, backColor)
            {
                MaxLines = 1,
                UseRealTime = true,
                Enabled = false
            };

            //Add the help text
            Write("Type \"commands\" for a list of commands.");
            //Initialize the commands
            InitCommands();

            //Event input
            KeyboardInput.OnKeyPressed += KeyInput;
        }
        private static void InitCommands()
        {
            //Display all the commands
            Action<string[]> showCommands = (args) =>
            {
                for (int i = 0; i < commands.Count(); i++)
                    Write(commands.Keys.ElementAt(i));
            };
            commands.Add("commands", showCommands);

            //Clear the log
            Action<string[]> clearLog = (args) =>
            {
                log.Clear();
            };
            commands.Add("clear", clearLog);

            //Show the fps
            Action<string[]> showFPS = (args) =>
            {
                Write("FPS: " + FrameRateCounter.FrameRate.ToString());
            };
            commands.Add("fps", showFPS);

            //Show the total gametime
            Action<string[]> gameTime = (args) =>
            {
                Write("Total gametime: " + Time.GameTime.TotalGameTime.ToString());
            };
            commands.Add("gametime", gameTime);

            //Show the current gamestate
            Action<string[]> gameState = (args) =>
            {
                Write("Current gamestate: " + (X.CurrentState == null ? "null" : X.CurrentState.Name));
            };
            commands.Add("gamestate", gameState);

            //Show the list of gamestates
            Action<string[]> stateList = (args) =>
            {
                Write("Gamestates:");
                List<string> states = X.GameStates.Keys.ToList();
                for (int i = 0; i < states.Count; i++)
                    Write(states[i]);
            };
            commands.Add("statelist", stateList);

            //Switch to a gamestate
            Action<string[]> switchState = (args) =>
            {
                try
                {
                    if (X.CurrentState.Name != args[0])
                        X.SwitchTo(args[0]);
                    else
                        Error(args[0] + " is already the current state.");
                }
                catch (Exception)
                { Error("Example: switchto stateName"); }
            };
            commands.Add("switchto", switchState);

            //Show the list of gameobjects in the current gamestate
            Action<string[]> currentObjects = (args) =>
            {
                Write("Gameobjects:");
                foreach (GameObject o in X.CurrentState)
                {
                    Write(o.Name);
                }
            };
            commands.Add("gameobjects", currentObjects);

            //Reset the gamestate
            Action<string[]> resetState = (args) =>
            {
                if (X.CurrentState != null)
                    X.SwitchTo(X.CurrentState.Name);
            };
            commands.Add("reset", resetState);

            //Show or set the gamespeed
            Action<string[]> gameSpeed = (args) =>
            {
                if (args.Length > 0)
                {
                    double gs;
                    if (double.TryParse(args[0], NumberStyles.AllowDecimalPoint, null, out gs))
                        Time.GameSpeed = gs;
                    else
                        Error("That is not a valid game speed. Example: gamespeed 0,5");
                }
                Write("Current game speed: " + Time.GameSpeed.ToString());
            };
            commands.Add("gamespeed", gameSpeed);
        }

        private static void KeyInput(object sender, KeyEventArgs k)
        {
            //Toggle console visibility
            if (k.Key == Keys.Tab)
            {
                visible = !visible;
                inputBox.Enabled = visible;
            }

            //If the console is visible, check the key
            if (visible && k.Key == Keys.Enter)
            {
                //Enter the command
                if (inputBox.Text.Length > 0)
                    Command(inputBox.Text);
                inputBox.Text = String.Empty;
            }
        }

        public static void Update()
        {
            if (enabled && visible)
            {
                //Update the input box
                inputBox.Update();
            }
        }
        public static void Draw(SpriteBatchHolder spriteBatches)
        {
            if (enabled && visible)
            {
                //Draw the background and textbox
                spriteBatches[DrawModes.Gui].Draw(Assets.Get<Texture2D>("DummyTexture"), backRect, null, Color.Black * 0.6f, 0, Vector2.Zero, SpriteEffects.None, float.Epsilon);
                inputBox.Draw(spriteBatches);

                //Draw the log
                for (int i = log.Count - 1; i >= 0; i--)
                {
                    //Calculate the text position
                    Vector2 linePos = new Vector2(5, inputBox.BackRectangle.Top - inputBox.Font.LineSpacing * (log.Count - i));

                    //Check if the text position is above the screen
                    if (linePos.Y < 0)
                        break;

                    //Draw the text
                    spriteBatches[DrawModes.Gui].DrawString(inputBox.Font, log[i].Item1, linePos, log[i].Item2, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                }
            }
        }

        //Write to console
        public static void WriteColored(object o, Color c, string type = "info")
        {
            log.Add(new Tuple<string, Color, string>(o.ToString(), c, type));
        }
        public static void Write(object o)
        {
            WriteColored(o, Color.White, "info");
        }
        public static void Warning(object o)
        {
            WriteColored(o, Color.Yellow, "warning");
        }
        public static void Error(object o)
        {
            WriteColored(o, Color.Red, "error");
        }

        public static void Command(string c)
        {
            //Make sure the string is in lowercase
            c = c.ToLower();

            //Get the command and parameters
            string command = c.Split(' ')[0];
            c = c.Remove(0, command.Length);
            string[] parameters = c.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            //Check if the command exists, run it
            if (commands.ContainsKey(command))
                commands[command](parameters);
            else
                Error("This command (" + command + ") does not exist. Type \"commands\" for a list of commands.");
        }

        public static void SaveLog(string path, bool append)
        {
            //Save log to file
            using (StreamWriter writer = new StreamWriter(path, append))
            {
                for (int i = 0; i < log.Count; i++)
                    writer.WriteLine("<" + log[i].Item3 + "> " + log[i].Item1);
            }
        }

        public static bool Visible
        { get { return visible; } set { visible = value && enabled; } }
        public static bool Enabled
        {
            get { return enabled; }
            set
            {
                enabled = value;
                inputBox.Enabled = value;
            }
        }
    }
}
