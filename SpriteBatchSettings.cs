using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XoticEngine
{
    public class SpriteBatchSettings
    {
        //The default spritebatch settings
        private SpriteSortMode sortMode = SpriteSortMode.Deferred;
        private BlendState blendState = BlendState.AlphaBlend;
        private SamplerState samplerState = SamplerState.LinearClamp;
        private DepthStencilState depthStencilState = DepthStencilState.None;
        private RasterizerState rasterizerState = RasterizerState.CullCounterClockwise;
        private Effect effect;
        private Matrix transformMatrix = Matrix.Identity;

        public SpriteBatchSettings(SpriteSortMode sortMode, BlendState blendState)
        {
            this.sortMode = sortMode;
            this.blendState = blendState ?? BlendState.AlphaBlend;
        }
        public SpriteBatchSettings(SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState)
        {
            this.sortMode = sortMode;
            this.blendState = blendState ?? BlendState.AlphaBlend;
            this.samplerState = samplerState ?? SamplerState.LinearClamp;
            this.depthStencilState = depthStencilState ?? DepthStencilState.None;
            this.rasterizerState = rasterizerState ?? RasterizerState.CullCounterClockwise;
        }
        public SpriteBatchSettings(SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState, Effect effect)
        {
            this.sortMode = sortMode;
            this.blendState = blendState ?? BlendState.AlphaBlend;
            this.samplerState = samplerState ?? SamplerState.LinearClamp;
            this.depthStencilState = depthStencilState ?? DepthStencilState.None;
            this.rasterizerState = rasterizerState ?? RasterizerState.CullCounterClockwise;
            this.effect = effect;
        }
        public SpriteBatchSettings(SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState, Effect effect, Matrix transformMatrix)
        {
            this.sortMode = sortMode;
            this.blendState = blendState ?? BlendState.AlphaBlend;
            this.samplerState = samplerState ?? SamplerState.LinearClamp;
            this.depthStencilState = depthStencilState ?? DepthStencilState.None;
            this.rasterizerState = rasterizerState ?? RasterizerState.CullCounterClockwise;
            this.effect = effect;
            this.transformMatrix = transformMatrix;
        }

        public SpriteSortMode SortMode
        { get { return sortMode; } set { sortMode = value; } }
        public BlendState BlendState
        { get { return blendState; } set { blendState = value ?? BlendState.AlphaBlend; ; } }
        public SamplerState SamplerState
        { get { return samplerState; } set { samplerState = value ?? SamplerState.LinearClamp; } }
        public DepthStencilState DepthStencilState
        { get { return depthStencilState; } set { depthStencilState = value ?? DepthStencilState.None; } }
        public RasterizerState RasterizerState
        { get { return rasterizerState; } set { rasterizerState = value ?? RasterizerState.CullCounterClockwise; } }
        public Effect Effect
        { get { return effect; } set { effect = value; } }
        public Matrix TransformMatrix
        { get { return transformMatrix; } set { transformMatrix = value; } }
    }
}
