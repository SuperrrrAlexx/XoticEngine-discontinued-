using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using XoticEngine.GameObjects;

namespace XoticEngine
{
    public static class Assets
    {
        //Content manager
        private static ContentManager content;
        //All assets
        private static Dictionary<Type, Dictionary<string, object>> assets;
        //Dummy texture
        private static Texture2D dummyTex;

        public static void Initialize(ContentManager c)
        {
            //Create the dictionary
            assets = new Dictionary<Type, Dictionary<string, object>>();

            //Set the content manager
            content = c;

            //Create a dummy texture and add it to the list
            dummyTex = new Texture2D(Graphics.Device, 1, 1);
            dummyTex.SetData(new Color[] { Color.White });
        }

        public static T Get<T>(string name)
        {
            //If the asset is not loaded yet, load it
            if (!assets.ContainsKey(typeof(T)) || !assets[typeof(T)].ContainsKey(name))
                Load<T>(name);

            //Return the asset
            return (T)Convert.ChangeType(assets[typeof(T)][name], typeof(T));
        }
        public static void Load<T>(string name)
        {
            //Get the type
            Type t = typeof(T);

            //Check if the dict a;ready contains a dict for the type
            if (!assets.ContainsKey(t))
                assets.Add(t, new Dictionary<string, object>());
            //Check if the asset is already loaded
            else if (assets[t].ContainsKey(name))
                return;

            //If the asset is not loaded yet, load it.
            switch (typeof(T).Name)
            {
                case "SpriteSheet":
                    //Load the spritesheet
                    assets[t].Add(name, new SpriteSheet(name));
                    break;
                default:
                    assets[t].Add(name, content.Load<T>(name));
                    break;
            }
        }
        public static void Store(object item, string name)
        {
            //Get the object type
            Type t = item.GetType();

            //Check if the key exists
            if (!assets.ContainsKey(t))
                assets.Add(t, new Dictionary<string, dynamic>());

            //Add the item
            assets[t].Add(name, item);
        }


        public static void PlaySound(string name)
        {
            Get<SoundEffect>(name).Play();
        }
        public static void PlaySound(string name, float volume, float pitch, float pan)
        {
            Get<SoundEffect>(name).Play(volume, pitch, pan);
        }

        public static ContentManager Content
        { get { return content; } }
        public static Texture2D DummyTexture
        { get { return dummyTex; } }
    }
}