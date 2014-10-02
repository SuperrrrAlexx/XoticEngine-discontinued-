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
        private static GraphicsDeviceManager graphics;
        //Spritebatches
        private static SpriteBatchHolder spriteBatches;
        private static SpriteBatch effectBatch;
        private static Matrix transformMatrix = Matrix.Identity;
        //Post processing
        private static List<PostProcessingEffect> postProcessing;
        private static RenderTarget2D target;

        internal static void Initialize(GraphicsDeviceManager gr)
        {
            //Create the graphics device manager
            graphics = gr;

            //Create the spritebatch settings and holder
            Dictionary<DrawModes, SpriteBatchSettings> spriteBatchSettings = new Dictionary<DrawModes, SpriteBatchSettings>()
            {
                { DrawModes.AlphaBlend, new SpriteBatchSettings(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, transformMatrix) },
                { DrawModes.Additive, new SpriteBatchSettings(SpriteSortMode.BackToFront, BlendState.Additive, null, null, null, null, transformMatrix) },
                { DrawModes.Gui, new SpriteBatchSettings(SpriteSortMode.BackToFront, null) },
            };
            spriteBatches = new SpriteBatchHolder(spriteBatchSettings);

            //Create the effect batch
            effectBatch = new SpriteBatch(Device);

            //Create a new list and render target
            postProcessing = new List<PostProcessingEffect>();
            target = new RenderTarget2D(Device, Viewport.Width, Viewport.Height);
        }

        //Drawing
        internal static void Draw()
        {
            //Clear the graphics device
            Graphics.Device.Clear(Color.Transparent);

            //Check if there are any post processing effects
            if (postProcessing.Count > 0)
            {
                //Draw to a render target
                Device.SetRenderTarget(target);
                Device.Clear(Color.Transparent);
            }

            //Begin the spritebatches
            spriteBatches.Begin();

            //If the current game state is not null, draw it
            if (X.CurrentState != null)
                X.CurrentState.Draw(spriteBatches);

            //Draw the other components
            GameConsole.Draw(spriteBatches);
            AchievementHolder.Draw(spriteBatches);

            //End the spritebatches
            spriteBatches.End();

            if (postProcessing.Count > 0)
            {
                Device.SetRenderTarget(null);
                Texture2D texture = target;

                //Apply all effects
                for (int i = 0; i < postProcessing.Count; i++)
                    texture = postProcessing[i].Apply(texture, effectBatch, Vector2.Zero);

                //Draw to the screen
                Device.SetRenderTarget(null);
                effectBatch.Begin();
                effectBatch.Draw(texture, Vector2.Zero, Color.White);
                effectBatch.End();
            }

            //Frame rate counter
            FrameRateCounter.Draw();
        }

        public static void ResetTransformMatrix()
        {
            //Reset the transform matrix to the identity matrix
            transformMatrix = Matrix.Identity;
        }

        //Device and manager
        public static GraphicsDeviceManager DeviceManager
        { get { return graphics; } }
        public static GraphicsDevice Device
        { get { return graphics.GraphicsDevice; } }
        //Screen
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
        //Drawing
        public static Matrix TransformMatrix
        {
            get { return transformMatrix; }
            set
            {
                transformMatrix = value;
                //Save the matrix in the spritebatch settings
                spriteBatches.Settings(DrawModes.AlphaBlend).TransformMatrix = transformMatrix;
                spriteBatches.Settings(DrawModes.Additive).TransformMatrix = transformMatrix;
            }
        }
        public static List<PostProcessingEffect> PostProcessing
        { get { return postProcessing; } set { postProcessing = value; } }
    }
}
