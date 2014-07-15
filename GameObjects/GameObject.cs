using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XoticEngine.GameObjects
{
    public class GameObject
    {
        string name;
        DrawMode drawType = DrawMode.AlphaBlend;
        //Positioning
        Vector2 position, relativePosition, origin;
        float rotation, relativeRotation, depth;
        //Parent and children
        GameObject parent;
        Dictionary<string, List<GameObject>> children = new Dictionary<string, List<GameObject>>();

        public GameObject(string name, Vector2 position)
        {
            this.name = name;
            this.relativePosition = position;
            this.relativeRotation = 0.0f;
            this.origin = Vector2.Zero;
            this.depth = 0;
            UpdatePosition();
        }
        public GameObject(string name, Vector2 position, float rotation, Vector2 origin)
        {
            this.name = name;
            this.relativePosition = position;
            this.relativeRotation = rotation;
            this.origin = origin;
            this.depth = 0;
            UpdatePosition();
        }
        public GameObject(string name, Vector2 position, float rotation, Vector2 origin, float depth)
        {
            this.name = name;
            this.relativePosition = position;
            this.relativeRotation = rotation;
            this.origin = origin;
            this.depth = depth;
            UpdatePosition();
        }

        public virtual void Update()
        {
            //Update each child
            for (int i = 0; i < children.Count; i++)
                for (int g = 0; g < children.ElementAt(i).Value.Count; g++)
                    children.ElementAt(i).Value[g].Update();
        }
        public virtual void Draw(SpriteBatch gameBatch, SpriteBatch additiveBatch, SpriteBatch guiBatch)
        {
        }
        public virtual void DrawChildren(SpriteBatch gameBatch, SpriteBatch additiveBatch, SpriteBatch guiBatch)
        {
            //Draw each child
            for (int i = 0; i < children.Count; i++)
                for (int g = 0; g < children.ElementAt(i).Value.Count; g++)
                {
                    children.ElementAt(i).Value[g].Draw(gameBatch, additiveBatch, guiBatch);
                    children.ElementAt(i).Value[g].DrawChildren(gameBatch, additiveBatch, guiBatch);
                }
        }

        public void AddChild(GameObject child)
        {
            //Switch parents
            if (child.parent != null)
                child.parent.Children[child.name].Remove(this);
            child.parent = this;

            //Check if the key exists
            if (!children.ContainsKey(child.Name))
                children.Add(child.Name, new List<GameObject>());

            //Add the object
            children[child.Name].Add(child);

            //Update the position
            UpdatePosition();
        }
        public void SetParent(GameObject parent)
        {
            //Check if the GameObject is null
            if (parent == null)
                this.parent = null;
            parent.AddChild(this);
        }

        private void UpdatePosition()
        {
            //Check if the parent is null, set the position and rotation
            if (parent == null)
            {
                position = relativePosition;
                rotation = relativeRotation;
            }
            else
            {
                rotation = parent.Rotation + relativeRotation;
                position = parent.Position + relativePosition.Rotate(rotation);
            }

            //Update the positions of all children
            for (int i = 0; i < children.Count; i++)
                for (int g = 0; g < children.ElementAt(i).Value.Count; g++)
                    children.ElementAt(i).Value[g].UpdatePosition();
        }

        public override string ToString()
        {
            return name;
        }

        public enum DrawMode
        { AlphaBlend, Additive, Gui }

        public string Name
        { get { return name; } }
        public DrawMode DrawType
        { get { return drawType; } set { drawType = value; } }
        //Position
        public Vector2 Position
        { get { return position; } }
        public Vector2 RelativePosition
        {
            get { return relativePosition; }
            set
            {
                relativePosition = value;
                UpdatePosition();
            }
        }
        //Rotation
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
        public float Depth
        { get { return depth; } set { depth = value; } }
        //Parent and children
        public GameObject Parent
        { get { return parent; } set { SetParent(value); } }
        public Dictionary<string, List<GameObject>> Children
        { get { return children; } }
        public List<GameObject> this[string name]
        { get { return children[name]; } }
    }
}
