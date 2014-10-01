using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XoticEngine
{
    public class SpriteBatchHolder
    {
        private Dictionary<DrawModes, Tuple<SpriteBatch, SpriteBatchSettings>> batches;

        public SpriteBatchHolder(IDictionary<DrawModes, SpriteBatchSettings> settings)
        {
            batches = new Dictionary<DrawModes, Tuple<SpriteBatch, SpriteBatchSettings>>(settings.Count);

            for (int i = 0; i < settings.Count(); i++)
            {
                //Create a spritebatch
                SpriteBatch s = new SpriteBatch(Graphics.Device);
                //Add everything to the collection
                batches.Add(settings.ElementAt(i).Key, new Tuple<SpriteBatch, SpriteBatchSettings>(s, settings.ElementAt(i).Value));
            }
        }

        public void Begin()
        {
            //Begin all spritebatches with their respective settings
            foreach (var tuple in batches.Values)
                tuple.Item1.Begin(tuple.Item2);
        }
        public void End()
        {
            
            //End all spritebatches
            foreach (var tuple in batches.Values)
                tuple.Item1.End();
        }

        public SpriteBatchSettings Settings(DrawModes drawMode)
        {
            return batches[drawMode].Item2;
        }

        public SpriteBatch this[DrawModes drawMode]
        { get { return batches[drawMode].Item1; } }
    }
}
