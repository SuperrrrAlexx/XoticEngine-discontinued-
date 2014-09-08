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
        //Lists with assets
        private static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
        private static Dictionary<string, SpriteSheet> sheets = new Dictionary<string, SpriteSheet>();
        private static Dictionary<string, SpriteFont> fonts = new Dictionary<string, SpriteFont>();
        private static Dictionary<string, Effect> effects = new Dictionary<string, Effect>();
        private static Dictionary<string, SoundEffect> sounds = new Dictionary<string, SoundEffect>();
        private static Dictionary<string, Song> songs = new Dictionary<string, Song>();
        private static Dictionary<string, Video> videos = new Dictionary<string, Video>();

        public static void Initialize(ContentManager c)
        {
            //Set the content manager
            content = c;

            //Create a dummy texture and add it to the list
            Texture2D DummyTexture = new Texture2D(Graphics.Device, 1, 1);
            DummyTexture.SetData(new Color[] { Color.White });
            textures.Add("DummyTexture", DummyTexture);
        }

        public static T Get<T>(string name)
        {
            //If the asset is not loaded yet, load it. Then return the asset.
            switch (typeof(T).Name)
            {
                case "Texture2D":
                    Load<T>(name);
                    return (T)Convert.ChangeType(textures[name], typeof(T));
                case "SpriteSheet":
                    Load<T>(name);
                    return (T)Convert.ChangeType(sheets[name], typeof(T));
                case "SpriteFont":
                    Load<T>(name);
                    return (T)Convert.ChangeType(fonts[name], typeof(T));
                case "Effect":
                    Load<T>(name);
                    return (T)Convert.ChangeType(effects[name], typeof(T));
                case "SoundEffect":
                    Load<T>(name);
                    return (T)Convert.ChangeType(sounds[name], typeof(T));
                case "Song":
                    Load<T>(name);
                    return (T)Convert.ChangeType(songs[name], typeof(T));
                case "Video":
                    Load<T>(name);
                    return (T)Convert.ChangeType(videos[name], typeof(T));
                default:
                    throw new NotSupportedException("This type (" + typeof(T).ToString() + ") is not supported.");
            }
        }

        public static void Load<T>(string name)
        {
            //If the asset is not loaded yet, load it.
            switch (typeof(T).Name)
            {
                case "Texture2D":
                    if (!textures.ContainsKey(name))
                        textures.Add(name, content.Load<Texture2D>(name));
                    break;
                case "SpriteSheet":
                    if (!sheets.ContainsKey(name))
                        sheets.Add(name, new SpriteSheet(name));
                    break;
                case "SpriteFont":
                    if (!fonts.ContainsKey(name))
                        fonts.Add(name, content.Load<SpriteFont>(name));
                    break;
                case "Effect":
                    if (!effects.ContainsKey(name))
                        effects.Add(name, content.Load<Effect>(name));
                    break;
                case "SoundEffect":
                    if (!sounds.ContainsKey(name))
                        sounds.Add(name, content.Load<SoundEffect>(name));
                    break;
                case "Song":
                    if (!songs.ContainsKey(name))
                        songs.Add(name, content.Load<Song>(name));
                    break;
                case "Video":
                    if (!videos.ContainsKey(name))
                        videos.Add(name, content.Load<Video>(name));
                    break;
                default:
                    throw new NotSupportedException("This type (" + typeof(T).ToString() + ") is not supported.");
            }
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
        { get { return Get<Texture2D>("DummyTexture"); } }
    }
}