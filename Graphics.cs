using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        public static void Initialize(GraphicsDeviceManager device)
        {
            graphics = device;

            //Create the spritebatches
            spriteBatch = new SpriteBatch(Graphics.Device);
            guiSpriteBatch = new SpriteBatch(Graphics.Device);
            effectSpriteBatch = new SpriteBatch(Graphics.Device);

            //Create a new list and render target
            postProcessing = new List<PostProcessingEffect>();
            target = new RenderTarget2D(Graphics.Device, Graphics.Viewport.Width, Graphics.Viewport.Height);

            //Set the transform matrix
            transformMatrix = Matrix.Identity;
        }

        public static void DrawAll()
        {
            //Clear the graphics device
            Graphics.Device.Clear(Color.Black);

            //Draw to a render target
            if (postProcessing.Count > 0)
            {
                Graphics.Device.SetRenderTarget(target);
                Graphics.Device.Clear(Color.Black);
            }

            //Begin the spritebatches
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, transformMatrix);
            guiSpriteBatch.Begin(SpriteSortMode.BackToFront, null);

            //If the current game state is not null, draw it
            if (X.CurrentState != null)
                X.CurrentState.Draw(spriteBatch, guiSpriteBatch);

            //End the spritebatches
            spriteBatch.End();
            guiSpriteBatch.End();

            if (postProcessing.Count > 0)
            {
                effectSpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                Graphics.Device.SetRenderTarget(null);

                //Apply all effects
                for (int i = 0; i < postProcessing.Count; i++)
                    target = (RenderTarget2D)postProcessing[i].Apply(target, effectSpriteBatch, Vector2.Zero);

                //Draw to the screen
                Graphics.Device.SetRenderTarget(null);
                effectSpriteBatch.Draw(target, Vector2.Zero, Color.White);
                effectSpriteBatch.End();
            }

            //Draw the console
            DrawConsole();
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
