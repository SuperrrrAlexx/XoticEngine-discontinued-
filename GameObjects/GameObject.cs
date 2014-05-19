using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ScorpionEngine.GameObjects
{
    public class GameObject
    {
        #region Fields
        string name;
        //The position based on 
        Vector2 relativePosition;
        //Parent and children
        GameObject parent;
        List<GameObject> children = new List<GameObject>();
        #endregion

        #region Constructors
        public GameObject(string name)
        {
            this.name = name;
            this.relativePosition = Vector2.Zero;
        }
        public GameObject(string name, Vector2 position)
        {
            this.name = name;
            this.relativePosition = position;
        }
        #endregion

        #region Methods
        public virtual void Update()
        {
            //Update each child
            if (children.Count > 0)
                for (int i = 0; i < children.Count; i++)
                    children[i].Update();
        }

        public virtual void Draw(SpriteBatch s)
        {
            //Draw each child
            if (children.Count > 0)
                for (int i = 0; i < children.Count; i++)
                    children[i].Draw(s);
        }

        public void AddChild(GameObject g)
        {
            //Check if the list already contains the object
            if (!children.Contains(g))
            {
                //Add the child
                children.Add(g);
                //Set the parent to this on the child
                g.Parent = this;
            }
        }
        #endregion

        #region Properties
        public string Name
        { get { return name; } }
        //Position
        public Vector2 Position
        {
            get
            {
                if (parent != null)
                    return parent.Position + relativePosition;
                return relativePosition;
            }
        }
        public Vector2 RelativePosition
        { get { return relativePosition; } set { relativePosition = value; } }
        //Parent and children
        public GameObject Parent
        { get { return parent; } set { parent = value; } }
        public List<GameObject> Children
        { get { return children; } set { children = value; } }
        #endregion
    }
}
