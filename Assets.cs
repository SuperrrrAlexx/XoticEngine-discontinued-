using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace ScorpionEngine
{
    public static class Assets
    {
        #region Fields
        //Content manager
        static ContentManager content;
        //Lists with assets
        static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
        static Dictionary<string, SpriteSheet> sheets = new Dictionary<string, SpriteSheet>();
        static Dictionary<string, SpriteFont> fonts = new Dictionary<string, SpriteFont>();
        static Dictionary<string, Effect> effects = new Dictionary<string, Effect>();
        static Dictionary<string, SoundEffect> sounds = new Dictionary<string, SoundEffect>();
        static Dictionary<string, Song> songs = new Dictionary<string, Song>();
        static Dictionary<string, Video> videos = new Dictionary<string, Video>();
        //Prefixes
        static Dictionary<string, string> prefixes;
        #endregion

        #region Methods
        public static void Initialize(ContentManager c, GraphicsDevice g)
        {
            //Set the content manager
            content = c;

            //Create a dummy texture
            Texture2D DummyTexture = new Texture2D(g, 1, 1);
            DummyTexture.SetData(new Color[] { Color.White });
            //Add the dummy texture to the list
            textures.Add("DummyTexture", DummyTexture);

            //Create the prefixes
            prefixes = new Dictionary<string, string>
            {
                {"Texture2D", "Texture2D_"},
                {"SpriteFont", "SpriteFont_"},
                {"Effect", "Effect_"},
                {"SoundEffect", "SoundEffect_"},
                {"Song", "Song_"},
                {"Video", "Video_"},
            };
        }

        public static void SetPrefix<T>(string prefix)
        {
            if (!prefixes.ContainsKey(typeof(T).Name))
                throw new NotSupportedException("This type (" + typeof(T).ToString() + ") is not supported");

            //Set the prefix
            prefixes[typeof(T).Name] = prefix;
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
                        textures.Add(name, content.Load<Texture2D>(prefixes["Texture2D"] + name));
                    break;
                case "SpriteSheet":
                    if (!sheets.ContainsKey(name))
                        sheets.Add(name, new SpriteSheet(name));
                    break;
                case "SpriteFont":
                    if (!fonts.ContainsKey(name))
                        fonts.Add(name, content.Load<SpriteFont>(prefixes["SpriteFont"] + name));
                    break;
                case "Effect":
                    if (!effects.ContainsKey(name))
                        effects.Add(name, content.Load<Effect>(prefixes["Effect"] + name));
                    break;
                case "SoundEffect":
                    if (!sounds.ContainsKey(name))
                        sounds.Add(name, content.Load<SoundEffect>(prefixes["SoundEffect"] + name));
                    break;
                case "Song":
                    if (!songs.ContainsKey(name))
                        songs.Add(name, content.Load<Song>(prefixes["Song"] + name));
                    break;
                case "Video":
                    if (!videos.ContainsKey(name))
                        videos.Add(name, content.Load<Video>(prefixes["Video"] + name));
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
        #endregion

        public static ContentManager Content
        { get { return content; } }
        public static Texture2D DummyTexture
        { get { return Get<Texture2D>("DummyTexture"); } }
    }
}