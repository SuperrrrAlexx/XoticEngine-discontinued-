using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XoticEngine.Utilities
{
    public static class GameConsole
    {
        static bool enabled;
        //Input, log
        static List<Tuple<string, Color>> log = new List<Tuple<string,Color>>();
        static string input = "";
        static int maxInputLength = 50;
        static string addString = "";
        //Blinking cursor
        static bool cursorVisible = true;
        static int cursorPos = 0;
        static int blinkTime = 500;
        static int blinkTimeLeft = 0;
        //Background
        static Rectangle backRect;
        static bool visible = false;
        //Text
        static Vector2 textPos;
        static SpriteFont font;
        //Commands
        static Dictionary<string, Action<string[]>> commands = new Dictionary<string, Action<string[]>>();

        public static void Initialize(string fontName)
        {
            //Set the text font and position
            font = Assets.Get<SpriteFont>(fontName);
            textPos = new Vector2(font.MeasureString(">").X + 5, (int)(X.Graphics.Viewport.Height * 0.6) - font.LineSpacing);
            
            //Set the back rectangle
            backRect = new Rectangle(0, 0, X.Graphics.Viewport.Width, (int)(X.Graphics.Viewport.Height * 0.6));

            //Add the help text
            Write("Type \"commands\" for a list of commands.");
            //Initialize the commands
            InitCommands();

            //Event input
            Input.OnKeyPressed += (k) => KeyInput(k);
            Input.OnCharEntered += (c) => CharInput(c);
        }

        //Key and character input
        static void KeyInput(Keys k)
        {
            if (k == Keys.Tab)
                //Toggle console visibility
                visible = !visible;

            //If the console is visible, check the key
            if (visible)
            {
                switch (k)
                {
                    case Keys.Enter:
                        //Enter the command
                        if (input.Length > 0)
                            Command(input);
                        input = "";
                        cursorPos = 0;
                        break;
                    case Keys.Back:
                        //Remove a letter left from the cursor
                        if (cursorPos > 0)
                        {
                            input = input.Substring(0, cursorPos - 1) + input.Substring(cursorPos, input.Length - cursorPos);
                            cursorPos--;
                        }
                        break;
                    case Keys.Delete:
                        //Remove a letter right from the cursor
                        if (cursorPos < input.Length)
                            input = input.Substring(0, cursorPos) + input.Substring(cursorPos + 1, input.Length - (cursorPos + 1));
                        break;
                    case Keys.Left:
                        //Move the cursor left
                        if (cursorPos > 0)
                            cursorPos--;
                        break;
                    case Keys.Right:
                        //Move the cursor right
                        if (cursorPos < input.Length)
                            cursorPos++;
                        break;
                    default:
                        break;
                }
            }

            //Reset the blink time
            cursorVisible = true;
            blinkTimeLeft = blinkTime;
        }
        static void CharInput(char c)
        {
            //Add the char to the addString
            //This is added in the update, to prevent weird exceptions and crashes
            if (visible)
                addString += c.ToString();
        }

        //Initialize all the commands
        static void InitCommands()
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
                    log = new List<Tuple<string, Color>>();
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
                    if (Time.GameTime != null)
                        Write("Total gametime: " + Time.GameTime.TotalGameTime.ToString());
                };
            commands.Add("gametime", gameTime);

            //Show the current gamestate
            Action<string[]> gameState = (args) =>
                {
                    Write("Current gamestate: " + X.CurrentState == null ? "null" : X.CurrentState.Name);
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
                    List<string> names = X.CurrentState.Keys.ToList();
                    for (int i = 0; i < names.Count; i++)
                        Write(names[i]);
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
                        if (double.TryParse(args[0], out gs))
                            Time.GameSpeed = gs;
                        else
                            Error("That is not a valid game speed. Example: gamespeed 0,5");
                    }
                    Write("Current game speed: " + Time.GameSpeed.ToString());
                };
            commands.Add("gamespeed", gameSpeed);
        }

        //Update and draw
        public static void Update()
        {
            if (enabled)
            {
                //Make the text cursor blink
                blinkTimeLeft -= Time.GameTime.ElapsedGameTime.Milliseconds;
                if (blinkTimeLeft <= 0)
                {
                    cursorVisible = !cursorVisible;
                    blinkTimeLeft += blinkTime;
                }

                //Add all letters from the addString to the input
                while (addString != "")
                {
                    if (input.Length < maxInputLength)
                    {
                        //Add the first letter
                        input = input.Substring(0, cursorPos) + addString[0] + input.Substring(cursorPos, input.Length - cursorPos);
                        addString = addString.Substring(1, addString.Length - 1);
                        //Set the cursor position
                        cursorPos++;
                    }
                    else
                        //Clear the addString
                        addString = "";
                }
            }
        }
        public static void Draw(SpriteBatch s)
        {
            if (enabled)
            {
                //Only draw the console if it's visible
                if (visible)
                {
                    //Draw the background
                    s.Draw(Assets.Get<Texture2D>("DummyTexture"), backRect, null, Color.Black * 0.6f, 0, Vector2.Zero, SpriteEffects.None, float.Epsilon);

                    //Draw the '>' and input
                    s.DrawString(font, ">", new Vector2(5, textPos.Y), Color.LightGray);
                    s.DrawString(font, input, textPos, Color.White);
                    //Draw the blinking text cursor
                    if (cursorVisible)
                        s.DrawString(font, "|", textPos + new Vector2(font.MeasureString(input.Substring(0, cursorPos)).X - 5, 0), Color.LightGray, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

                    //Draw the log
                    for (int i = log.Count - 1; i >= 0; i--)
                    {
                        //Calculate the text position
                        Vector2 linePos = new Vector2(5, textPos.Y - font.LineSpacing * (log.Count - i));
                        //Draw the text
                        s.DrawString(font, log[i].Item1, linePos, log[i].Item2, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                    }
                }
            }
        }

        //Write to console
        public static void WriteColored(object o, Color c)
        {
            log.Add(new Tuple<string, Color>(o.ToString(), c));
        }
        public static void Write(object o)
        {
            WriteColored(o, Color.White);
        }
        public static void Warning(object o)
        {
            WriteColored(o, Color.Yellow);
        }
        public static void Error(object o)
        {
            WriteColored(o, Color.Red);
        }

        //Command input
        public static void Command(string c)
        {
            //Make sure the string is in lowercase.
            c = c.ToLower();

            //Get the command, remove the command itself from the input string.
            string command = c.Split(' ')[0];
            c = c.Remove(0, command.Length);
            //Parameters, remove empty entries (when the param count is 0, it won't have an empty entry).
            string[] parameters = c.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            //If the command exists, invoke the method. Else display an error.
            if (commands.ContainsKey(command))
                commands[command](parameters);
            else
                Error("This command (" + command + ") does not exist. Type \"commands\" for a list of commands.");
        }

        public static bool Visible
        { get { return visible; } set { visible = value; } }
        public static bool Enabled
        { get { return enabled; } set { enabled = value; } }
    }
}
