using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using XoticEngine.GameObjects.MenuItems;
using XoticEngine.Utilities;

namespace XoticEngine.GameObjects
{
    public class GameState : IEnumerable<GameObject>
    {
        readonly string name;
        Dictionary<string, List<GameObject>> gameObjects;

        public GameState(string name)
            : base()
        {
            this.name = name;
            gameObjects = new Dictionary<string, List<GameObject>>();
        }

        public virtual void Update()
        {
            //Update each gameobject
            foreach (GameObject g in this)
                g.Update();
        }
        public virtual void Draw(SpriteBatch gameBatch, SpriteBatch additiveBatch, SpriteBatch guiBatch)
        {
            //Draw each gameobject and its children
            foreach (GameObject g in this)
            {
                g.Draw(gameBatch, additiveBatch, guiBatch);
                g.DrawChildren(gameBatch, additiveBatch, guiBatch);
            }
        }

        public void Add(GameObject g)
        {
            //Check if the key exists
            if (!gameObjects.ContainsKey(g.Name))
                gameObjects.Add(g.Name, new List<GameObject>());

            //Add the object
            gameObjects[g.Name].Add(g);
        }
        public void RemoveAll(string name)
        {
            gameObjects.Remove(name);
        }
        public void Remove(GameObject g)
        {
            gameObjects[g.Name].Remove(g);
        }

        public virtual void BeginState()
        {
            GameConsole.Warning("Gamestate " + name + " started.");
        }
        public virtual void EndState()
        {
            GameConsole.Warning("Gamestate " + name + " ended.");
        }

        public override string ToString()
        {
            return name;
        }

        public IEnumerator<GameObject> GetEnumerator()
        {
            for (int list = 0; list < gameObjects.Count; list++)
                for (int item = 0; item < gameObjects.ElementAt(list).Value.Count; item++)
                    yield return gameObjects.ElementAt(list).Value[item];
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public string Name
        { get { return name; } }
        public List<GameObject> this[string name]
        { get { return gameObjects.ContainsKey(name) ? gameObjects[name] : null; } }
        public Dictionary<string, List<GameObject>> GameObjects
        { get { return gameObjects; } }
    }
}
