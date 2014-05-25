using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ScorpionEngine
{
    public class Camera
    {
        Matrix transform;
        Vector2 position;
        float zoom, rotation;
        Rectangle bounds;

        public Camera()
        {
            Reset();
            UpdateMatrix();
        }
        public Camera(Vector2 position, float zoom, float rotation)
        {
            this.position = position;
            this.zoom = zoom;
            this.rotation = rotation;
            this.bounds = new Rectangle(SE.Graphics.Viewport.Width / 2, SE.Graphics.Viewport.Height / 2, SE.Graphics.Viewport.Width, SE.Graphics.Viewport.Height);

            //Update the matrix
            UpdateMatrix();
        }

        public void UpdateMatrix()
        {
            transform = Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) *
                Matrix.CreateRotationZ(rotation) *
                Matrix.CreateScale(zoom) *
                Matrix.CreateTranslation(new Vector3(bounds.Width * 0.5f, bounds.Height * 0.5f, 0));
        }
        public void UpdateMatrix(Vector2 position, float zoom, float rotation)
        {
            //Update the variables
            this.position = position;
            this.zoom = zoom;
            this.rotation = rotation;

            //Update the matrix
            UpdateMatrix();
        }

        public void Reset()
        {
            //Reset all the variables
            position = Vector2.Zero;
            zoom = 1.0f;
            rotation = 0.0f;
            bounds = new Rectangle(SE.Graphics.Viewport.Width / 2, SE.Graphics.Viewport.Height / 2, SE.Graphics.Viewport.Width, SE.Graphics.Viewport.Height);

            //Update the matrix
            UpdateMatrix();
        }

        public Matrix TransformMatrix
        { get { return transform; } }
        public Vector2 Position
        { get { return position; } set { position = value; UpdateMatrix(); } }
        public float Zoom
        { get { return zoom; } set { zoom = value; UpdateMatrix(); } }
        public float Rotation
        { get { return rotation; } set { rotation = value; UpdateMatrix(); } }
        public Rectangle Bounds
        { get { return bounds; } set { bounds = value; UpdateMatrix(); } }
    }
}
