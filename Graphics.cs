using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XoticEngine.GraphicEffects;
using XoticEngine.Utilities;

namespace XoticEngine
{
    public static class Graphics
    {
        static GraphicsDeviceManager graphics;
        //Spritebatches
        static SpriteBatch spriteBatch, guiSpriteBatch, effectSpriteBatch;
        static Matrix transformMatrix;
        //Post processing
        static List<PostProcessingEffect> postProcessing;
        static RenderTarget2D target;

        public static void Initialize(GraphicsDeviceManager gr)
        {
            //Create the graphics device manager
            graphics = gr;

            //Create the spritebatches
            spriteBatch = new SpriteBatch(Device);
            guiSpriteBatch = new SpriteBatch(Device);
            effectSpriteBatch = new SpriteBatch(Device);

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
                Texture2D texture = target;

                //Apply all effects
                for (int i = 0; i < postProcessing.Count; i++)
                    texture = postProcessing[i].Apply(texture, effectSpriteBatch, Vector2.Zero);

                //Draw to the screen
                Device.SetRenderTarget(null);
                effectSpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                effectSpriteBatch.Draw(texture, Vector2.Zero, Color.White);
                effectSpriteBatch.End();
            }

            //Draw the console
            DrawConsole();

            //Draw the framerate counter
            FrameRateCounter.Draw();
        }
        static void DrawGameState()
        {
            //Begin the spritebatches
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, transformMatrix);
            guiSpriteBatch.Begin(SpriteSortMode.BackToFront, null);

            //If the current game state is not null, draw it
            if (X.CurrentState != null)
                X.CurrentState.Draw(spriteBatch, guiSpriteBatch);

            //End the spritebatches
            spriteBatch.End();
            guiSpriteBatch.End();
        }
        static void DrawConsole()
        {
            if (GameConsole.Enabled)
            {
                guiSpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                GameConsole.Draw(guiSpriteBatch);
                guiSpriteBatch.End();
            }
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
        public static void ResetTransformMatrix()
        { transformMatrix = Matrix.Identity; }
        public static SpriteBatch GUISpriteBatch
        { get { return guiSpriteBatch; } }
        public static List<PostProcessingEffect> PostProcessing
        { get { return postProcessing; } set { postProcessing = value; } }
    }
}
