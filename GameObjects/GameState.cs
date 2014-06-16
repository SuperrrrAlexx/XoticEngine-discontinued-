using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using XoticEngine.Utilities;

namespace XoticEngine.GameObjects
{
    public class GameState : Dictionary<string, List<GameObject>>
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
            for (int i = 0; i < this.Count; i++)
                for (int g = 0; g < this.ElementAt(i).Value.Count; g++)
                    this.ElementAt(i).Value[g].Update();
        }

        public virtual void Draw(SpriteBatch s)
        {
            //Draw each gameobject
            for (int i = 0; i < this.Count; i++)
                for (int g = 0; g < this.ElementAt(i).Value.Count; g++)
                    this.ElementAt(i).Value[g].Draw(s);
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
    }
}
