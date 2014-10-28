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
        //Drawing
        private float depth;
        //Kinematics
        private Vector2 position, relativePosition, origin;
        private float rotation, relativeRotation;
        private Vector2 scale, relativeScale;
        private KinematicProperties kinematics;
        //Parent and children
        private GameObject parent;
        private Dictionary<string, List<GameObject>> children = new Dictionary<string, List<GameObject>>();

        public GameObject(string name)
        {
            this.Name = name;
            this.relativePosition = Vector2.Zero;
            this.relativeRotation = 0.0f;
            this.origin = Vector2.Zero;
            this.relativeScale = Vector2.One;
            this.depth = 0;
            this.kinematics = new KinematicProperties(this);
        }
        public GameObject(string name, Vector2 position)
        {
            this.Name = name;
            this.relativePosition = position;
            this.relativeRotation = 0.0f;
            this.origin = Vector2.Zero;
            this.relativeScale = Vector2.One;
            this.depth = 0;
            this.kinematics = new KinematicProperties(this);
        }
        public GameObject(string name, Vector2 position, float rotation, Vector2 origin, float depth)
        {
            this.Name = name;
            this.relativePosition = position;
            this.relativeRotation = rotation;
            this.origin = origin;
            this.relativeScale = Vector2.One;
            this.depth = depth;
            this.kinematics = new KinematicProperties(this);
        }
        public GameObject(string name, Vector2 position, float rotation, Vector2 origin, Vector2 scale, float depth)
        {
            this.Name = name;
            this.relativePosition = position;
            this.relativeRotation = rotation;
            this.origin = origin;
            this.relativeScale = scale;
            this.depth = depth;
            this.kinematics = new KinematicProperties(this);
        }

        public virtual void Update()
        {
            //Update each child
            foreach (GameObject g in this)
                g.Update();
        }
        public virtual void Draw(SpriteBatchHolder spriteBatches)
        {
            //Draw each child
            foreach (GameObject g in this)
                g.Draw(spriteBatches);
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

        internal void UpdatePosition()
        {
            //Check if the parent is null, set the position and rotation
            if (parent == null)
            {
                rotation = relativeRotation;
                position = relativePosition;
                scale = relativeScale;
            }
            else
            {
                //Set the properties based on the kinematics and the parent properties
                rotation = kinematics.RotateWithParent ? parent.Rotation + relativeRotation : relativeRotation;
                scale = kinematics.ScaleWithParent ? parent.Scale * relativeScale : relativeScale;
                position = kinematics.PositionWithParent ? parent.Position : Vector2.Zero;
                position += kinematics.MoveWithParentRotation ? relativePosition.Rotate(parent.Rotation) : relativePosition;
            }

            //Update the positions of all children
            foreach (GameObject child in this)
                child.UpdatePosition();
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
            for (int list = 0; list < children.Count; list++)
                for (int item = 0; item < children.ElementAt(list).Value.Count; item++)
                    yield return children.ElementAt(list).Value[item];
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        //Drawing
        public float Depth
        { get { return depth; } set { depth = value; } }
        //Kinematics
        public KinematicProperties Kinematics
        { get { return kinematics; } }
        public Vector2 Position
        { get { return position; } set { RelativePosition = value; } }
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
        { get { return rotation; } set { RelativeRotation = value; } }
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
        public Vector2 Scale
        { get { return scale; } set { RelativeScale = value; } }
        public Vector2 RelativeScale
        {
            get { return relativeScale; }
            set
            {
                relativeScale = value;
                UpdatePosition();
            }
        }
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
