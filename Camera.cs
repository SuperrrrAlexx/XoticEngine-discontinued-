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
        bool applyOnUpdate;

        public Camera(bool applyOnUpdate)
        {
            this.applyOnUpdate = applyOnUpdate;
            Reset();
        }
        public Camera(Vector2 position, float zoom, float rotation, bool applyOnUpdate)
        {
            this.applyOnUpdate = applyOnUpdate;
            UpdateMatrix(position, zoom, rotation);
        }

        public void UpdateMatrix()
        {
            transform = Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) *
                Matrix.CreateRotationZ(rotation) *
                Matrix.CreateScale(new Vector3(zoom, zoom, 1)) *
                Matrix.CreateTranslation(new Vector3(SE.Graphics.Viewport.Width * 0.5f, SE.Graphics.Viewport.Height * 0.5f, 0));

            if (applyOnUpdate)
                SE.Graphics.TransformMatrix = transform;
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
    }
}
