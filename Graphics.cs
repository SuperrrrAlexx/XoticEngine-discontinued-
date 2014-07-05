using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XoticEngine.Achievements;
using XoticEngine.GraphicEffects;
using XoticEngine.Utilities;

namespace XoticEngine
{
    public static class Graphics
    {
        static GraphicsDeviceManager graphics;
        //Spritebatches
        static SpriteBatch gameBatch, additiveBatch, guiBatch;
        static Matrix transformMatrix;
        //Post processing
        static List<PostProcessingEffect> postProcessing;
        static RenderTarget2D target;

        public static void Initialize(GraphicsDeviceManager gr)
        {
            //Create the graphics device manager
            graphics = gr;

            //Create the spritebatches
            gameBatch = new SpriteBatch(Device);
            additiveBatch = new SpriteBatch(Device);
            guiBatch = new SpriteBatch(Device);

            //Create a new list and render target
            postProcessing = new List<PostProcessingEffect>();
            target = new RenderTarget2D(Device, Viewport.Width, Viewport.Height);

            //Set the transform matrix
            transformMatrix = Matrix.Identity;
        }

        public static void DrawAll()
        {
            //Clear the graphics device
            Graphics.Device.Clear(Color.Transparent);

            //Draw to a render target
            if (postProcessing.Count > 0)
            {
                Device.SetRenderTarget(target);
                Device.Clear(Color.Transparent);
            }

            //Draw the current game state
            DrawGameState();

            if (postProcessing.Count > 0)
            {
                Device.SetRenderTarget(null);
                Texture2D texture = target;

                //Apply all effects
                for (int i = 0; i < postProcessing.Count; i++)
                    texture = postProcessing[i].Apply(texture, gameBatch, Vector2.Zero);

                //Draw to the screen
                Device.SetRenderTarget(null);
                gameBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                gameBatch.Draw(texture, Vector2.Zero, Color.White);
                gameBatch.End();
            }

            //Draw the gui elements
            DrawGui();
            //Draw the framerate counter
            FrameRateCounter.Draw();
        }
        static void DrawGameState()
        {
            //Begin the spritebatches
            gameBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, transformMatrix);
            additiveBatch.Begin(SpriteSortMode.BackToFront, BlendState.Additive, null, null, null, null, transformMatrix);
            guiBatch.Begin(SpriteSortMode.BackToFront, null);

            //If the current game state is not null, draw it
            if (X.CurrentState != null)
                X.CurrentState.Draw(gameBatch, additiveBatch, guiBatch);

            //End the spritebatches
            gameBatch.End();
            additiveBatch.End();
            guiBatch.End();
        }
        static void DrawGui()
        {
            guiBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            //Draw the console and schievements
            GameConsole.Draw(guiBatch);
            AchievementHolder.Draw(guiBatch);

            guiBatch.End();
        }

        public static void ResetTransformMatrix()
        {
            //Reset the transform matrix to the identity matrix
            transformMatrix = Matrix.Identity;
        }

        //Properties
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
        public static List<PostProcessingEffect> PostProcessing
        { get { return postProcessing; } set { postProcessing = value; } }
    }
}
