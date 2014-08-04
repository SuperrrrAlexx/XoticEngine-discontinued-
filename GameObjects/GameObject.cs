using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XoticEngine.GameObjects
{
    public class GameObject : IEnumerable
    {
        //Name
        public readonly string Name;

        private DrawMode drawType = DrawMode.AlphaBlend;
        //Positioning
        private Vector2 position, relativePosition, origin;
        private float rotation, relativeRotation, depth;
        //Parent and children
        private GameObject parent;
        private Dictionary<string, List<GameObject>> children = new Dictionary<string, List<GameObject>>();

        public GameObject(string name, Vector2 position)
        {
            this.Name = name;
            this.relativePosition = position;
            this.relativeRotation = 0.0f;
            this.origin = Vector2.Zero;
            this.depth = 0;
            UpdatePosition();
        }
        public GameObject(string name, Vector2 position, float rotation, Vector2 origin)
        {
            this.Name = name;
            this.relativePosition = position;
            this.relativeRotation = rotation;
            this.origin = origin;
            this.depth = 0;
            UpdatePosition();
        }
        public GameObject(string name, Vector2 position, float rotation, Vector2 origin, float depth)
        {
            this.Name = name;
            this.relativePosition = position;
            this.relativeRotation = rotation;
            this.origin = origin;
            this.depth = depth;
            UpdatePosition();
        }

        public virtual void Update()
        {
            //Update each child
            foreach (GameObject g in this)
                g.Update();
        }
        public virtual void Draw(SpriteBatch gameBatch, SpriteBatch additiveBatch, SpriteBatch guiBatch)
        {
        }
        public virtual void DrawChildren(SpriteBatch gameBatch, SpriteBatch additiveBatch, SpriteBatch guiBatch)
        {
            //Draw each child
            foreach (GameObject g in this)
            {
                g.Draw(gameBatch, additiveBatch, guiBatch);
                g.DrawChildren(gameBatch, additiveBatch, guiBatch);
            }
        }

        public void AddChild(GameObject child)
        {
            //Remove the child from its old parent
            if (child.parent != null)
                child.parent.Children[child.Name].Remove(this);
            //Save this as the new parent
            child.parent = this;

            //Add the child to children
            if (!children.ContainsKey(child.Name))
                children.Add(child.Name, new List<GameObject>());
            children[child.Name].Add(child);

            //Update the childs position
            child.UpdatePosition();
        }
        public void SetParent(GameObject parent)
        {
            //Check if the new parent is null, but this does have a parent
            if (parent == null)
            {
                if (this.parent != null)
                {
                    //Remove this from its parent, set the parent to null
                    this.parent.Children[Name].Remove(this);
                    this.parent = null;
                }
            }
            else
                //Add this as a child to the new parent
                parent.AddChild(this);
        }

        private void UpdatePosition()
        {
            //Check if the parent is null, set the position and rotation
            if (parent == null)
            {
                rotation = relativeRotation;
                position = relativePosition;
            }
            else
            {
                rotation = parent.Rotation + relativeRotation;
                position = parent.Position + relativePosition.Rotate(rotation);
            }

            //Update the positions of all children
            foreach (GameObject child in this)
                child.UpdatePosition();
        }

        public override string ToString()
        {
            return Name;
        }

        public IEnumerator<GameObject> GetEnumerator()
        {
            for (int list = 0; list < children.Count; list++)
                for (int item = 0; item < children.ElementAt(list).Value.Count; item++)
                    yield return children.ElementAt(list).Value[item];
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public enum DrawMode
        { AlphaBlend, Additive, Gui }

        //Drawing
        public DrawMode DrawType
        { get { return drawType; } set { drawType = value; } }
        public float Depth
        { get { return depth; } set { depth = value; } }

        //Position and rotation
        public Vector2 Position
        { get { return position; } }
        public virtual Vector2 RelativePosition
        {
            get { return relativePosition; }
            set
            {
                relativePosition = value;
                UpdatePosition();
            }
        }
        public float Rotation
        { get { return rotation; } }
        public float RelativeRotation
        {
            get { return relativeRotation; }
            set
            {
                relativeRotation = value;
                UpdatePosition();
            }
        }
        public Vector2 Origin
        { get { return origin; } set { origin = value; } }

        //Parent and children
        public GameObject Parent
        { get { return parent; } set { SetParent(value); } }
        public Dictionary<string, List<GameObject>> Children
        { get { return children; } }
        public List<GameObject> this[string name]
        {
            get
            {
                if (!children.ContainsKey(name))
                    children[name] = new List<GameObject>();
                return children[name];
            }
        }
    }
}
