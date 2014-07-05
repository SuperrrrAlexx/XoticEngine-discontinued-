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
        //Positioning
        Vector2 position, relativePosition, origin;
        float rotation, relativeRotation, rotationOffset, depth;
        //Parent and children
        GameObject parent;
        List<GameObject> children = new List<GameObject>();

        public GameObject(string name)
        {
            this.name = name;
            this.relativePosition = Vector2.Zero;
            this.relativeRotation = 0.0f;
            this.rotationOffset = 0.0f;
            this.origin = Vector2.Zero;
            this.depth = 0.0f;
            UpdatePosition();
        }
        public GameObject(string name, Vector2 position, float rotation)
        {
            this.name = name;
            this.relativePosition = position;
            this.relativeRotation = rotation;
            this.rotationOffset = position.GetAngle();
            this.origin = Vector2.Zero;
            this.depth = 0.0f;
            UpdatePosition();
        }
        public GameObject(string name, Vector2 position, float rotation, Vector2 origin, float depth)
        {
            this.name = name;
            this.relativePosition = position;
            this.relativeRotation = rotation;
            this.rotationOffset = position.GetAngle();
            this.origin = origin;
            this.depth = depth;
            UpdatePosition();
        }

        public virtual void Update()
        {
            //Update each child
            foreach (GameObject g in children)
                g.Update();
        }
        public virtual void Draw(SpriteBatch gameBatch, SpriteBatch guiBatch)
        {
            //Draw each child
            foreach (GameObject g in children)
                g.Draw(gameBatch, guiBatch);
        }

        public void AddChild(GameObject g)
        {
            g.SetParent(this);
        }
        public void SetParent(GameObject g)
        {
            //Remove from the old parent
            if (parent != null)
                parent.Children.Remove(this);

            //Add to the new parent
            if (!g.Children.Contains(this))
                g.Children.Add(this);

            //Save the new parent
            parent = g;

            //Update the position
            UpdatePosition();
        }

        void UpdatePosition()
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
            foreach (GameObject g in children)
                g.UpdatePosition();
        }

        public override string ToString()
        {
            return name;
        }

        public string Name
        { get { return name; } }
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
        { get { return parent; } }
        public List<GameObject> Children
        { get { return children; } set { children = value; } }
    }
}
