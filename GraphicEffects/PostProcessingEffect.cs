using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XoticEngine.GraphicEffects
{
    public abstract class PostProcessingEffect
    {
        protected Effect effect;
        protected RenderTarget2D target1, target2;
        private bool secondTarget = true;

        public PostProcessingEffect(Effect effect)
        {
            this.effect = effect;

            //Create the render targets
            target1 = new RenderTarget2D(Graphics.Device, Graphics.Viewport.Width, Graphics.Viewport.Height);
            target2 = new RenderTarget2D(Graphics.Device, Graphics.Viewport.Width, Graphics.Viewport.Height);
        }

        public virtual Texture2D Apply(Texture2D texture, SpriteBatch spriteBatch, Vector2 pos)
        {
            //Switch the render target and clear
            Graphics.Device.SetRenderTarget(target1);
            Graphics.Device.Clear(Color.Black);

            //Apply the initial pass
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            effect.CurrentTechnique.Passes[0].Apply();
            spriteBatch.Draw(texture, Vector2.Zero, Color.White);
            spriteBatch.End();
            
            //Apply all other passes
            for (int pass = 1; pass < effect.CurrentTechnique.Passes.Count; pass++)
            {
                //Switch the render target and clear
                Graphics.Device.SetRenderTarget(secondTarget ? target2 : target1);
                Graphics.Device.Clear(Color.Black);

                //Apply the pass
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                effect.CurrentTechnique.Passes[pass].Apply();
                spriteBatch.Draw(!secondTarget ? target2 : target1, Vector2.Zero, Color.White);
                spriteBatch.End();

                //Switch secondTarget
                secondTarget = !secondTarget;
            }

            //Return the correct render target
            return !secondTarget ? target2 : target1;
        }
    }
}
