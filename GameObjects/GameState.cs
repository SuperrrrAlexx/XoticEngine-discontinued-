using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using XoticEngine.Utilities;

namespace XoticEngine.GameObjects
{
    public class GameState
    {
        string name;
        Dictionary<string, GameObject> objects;

        public GameState(string name)
            : base()
        {
            this.name = name;
            objects = new Dictionary<string, GameObject>();
        }

        public void Add(GameObject g)
        {
            objects.Add(g.Name, g);
        }
        public void Remove(string name)
        {
            objects.Remove(name);
        }

        public virtual void Update()
        {
            //Update each gameobject
            for (int i = 0; i < objects.Count; i++)
                objects.ElementAt(i).Value.Update();
        }

        public virtual void Draw(SpriteBatch s)
        {
            //Draw each gameobject
            for (int i = 0; i < objects.Count; i++)
                objects.ElementAt(i).Value.Draw(s);
        }

        public virtual void BeginState()
        {
            GameConsole.Warning("Gamestate " + name + " started.");
        }
        public virtual void EndState()
        {
            GameConsole.Warning("Gamestate " + name + " ended.");
        }

        public string Name
        { get { return name; } }
        public Dictionary<string, GameObject> Objects
        { get { return objects; } }
        public GameObject this[string name]
        { get { return objects[name]; } set { objects[name] = value; } }
    }
}
