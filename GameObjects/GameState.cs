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

        public virtual void Update()
        {
            //Update each gameobject
            foreach(GameObject obj in objects.Values)
                obj.Update();
        }

        public virtual void Draw(SpriteBatch s)
        {
            //Draw each gameobject
            foreach (GameObject obj in objects.Values)
                obj.Draw(s);
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
