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
            for (int i = 0; i < gameObjects.Count; i++)
                for (int g = 0; g < gameObjects.ElementAt(i).Value.Count; g++)
                    gameObjects.ElementAt(i).Value[g].Update();
        }

        public virtual void Draw(SpriteBatch s)
        {
            //Draw each gameobject
            for (int i = 0; i < gameObjects.Count; i++)
                for (int g = 0; g < gameObjects.ElementAt(i).Value.Count; g++)
                    gameObjects.ElementAt(i).Value[g].Draw(s);
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
            //Check if the key exists
            if (gameObjects.ContainsKey(g.Name))
                //Check and remove the object
                for (int i = 0; i < gameObjects[g.Name].Count; i++)
                    if (gameObjects[g.Name][i] == g)
                        gameObjects[g.Name].RemoveAt(i);
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

        public string Name
        { get { return name; } }
        public List<GameObject> this[string name]
        { get { return gameObjects.ContainsKey(name) ? gameObjects[name] : null; } }
        public Dictionary<string, List<GameObject>> GameObjects
        { get { return gameObjects; } }
    }
}
