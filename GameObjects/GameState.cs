using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using XoticEngine.Utilities;

namespace XoticEngine.GameObjects
{
    public class GameState : List<GameObject>
    {
        string name;

        public GameState(string name)
            : base()
        {
            this.name = name;
        }

        public virtual void Update()
        {
            //Update each gameobject
            if (Count > 0)
                for (int i = 0; i < Count; i++)
                    this[i].Update();
        }

        public virtual void Draw(SpriteBatch s)
        {
            //Draw each gameobject
            if (Count > 0)
                for (int i = 0; i < Count; i++)
                    this[i].Draw(s);
        }

        public virtual void BeginState()
        {
            GameConsole.Warning("Gamestate " + name + " started.");
        }
        public virtual void EndState()
        {
            GameConsole.Warning("Gamestate " + name + " ended.");
        }

        //Find/remove GameObjects
        public GameObject Find(string name)
        {
            //Check each object's name, return the first
            for (int i = 0; i < Count; i++)
                if (this[i].Name == name)
                    return this[i];

            //If no object was found, throw an exception
            throw new KeyNotFoundException("No GameObject with the name " + name + " was found.");
        }
        public List<GameObject> FindAll(string name)
        {
            //Create a list
            List<GameObject> list = new List<GameObject>();

            //Check each object's name, add them to the list
            for (int i = 0; i < Count; i++)
                if (this[i].Name == name)
                    list.Add(this[i]);

            //Return the list
            return list;
        }
        public void Remove(string name)
        {
            //Remove all GameObjects with a certain name
            for (int i = 0; i < Count; i++)
                if (this[i].Name == name)
                    RemoveAt(i);
        }

        public string Name
        { get { return name; } }
    }
}
