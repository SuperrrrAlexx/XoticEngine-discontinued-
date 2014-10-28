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
    public class GameState : IEnumerable
    {
        //Name
        public readonly string Name;

        Dictionary<string, List<GameObject>> gameObjects;

        public GameState(string name)
            : base()
        {
            this.Name = name;
            gameObjects = new Dictionary<string, List<GameObject>>();

            //Add this gamestate
            X.AddGameState(this);
        }

        public virtual void Update()
        {
            //Update each gameobject
            foreach (GameObject g in this)
                g.Update();
        }
        public virtual void Draw(SpriteBatchHolder spriteBatches)
        {
            //Draw each gameobject and its children
            foreach (GameObject g in this)
                DrawGameObject(g, spriteBatches);
        }
        private void DrawGameObject(GameObject g, SpriteBatchHolder spriteBatches)
        {
            //Draw the game object
            g.Draw(spriteBatches);

            //Draw the IXDrawable
            if (g is IXDrawable)
            {
                IXDrawable gd = (IXDrawable)g;
                if (gd.Sprite != null)
                    spriteBatches[gd.DrawMode].Draw(gd.Sprite, gd.Position, gd.SourceRectangle, gd.DrawColor, gd.Rotation, gd.Origin, gd.Scale, gd.Effects, gd.Depth);
            }

            //Draw each child
            foreach (GameObject child in g)
                DrawGameObject(child, spriteBatches);
        }

        public void Add(GameObject g)
        {
            //Check if the key exists
            if (!gameObjects.ContainsKey(g.Name))
                gameObjects.Add(g.Name, new List<GameObject>());

            //Add the object
            gameObjects[g.Name].Add(g);

            //Set the objects parent to null
            g.Parent = null;
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
            GameConsole.Warning("Gamestate " + Name + " started.");
        }
        public virtual void EndState()
        {
            GameConsole.Warning("Gamestate " + Name + " ended.");
        }

        public override string ToString()
        {
            return Name;
        }
        public override int GetHashCode()
        {
            return Name.GetHashCode();
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

        public List<GameObject> this[string name]
        {
            get
            {
                if (!gameObjects.ContainsKey(name))
                    gameObjects[name] = new List<GameObject>();
                return gameObjects[name];
            }
        }
        public Dictionary<string, List<GameObject>> GameObjects
        { get { return gameObjects; } }
    }
}
