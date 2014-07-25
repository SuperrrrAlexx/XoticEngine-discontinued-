using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XoticEngine.GraphicEffects
{
    public class Camera
    {
        Matrix transform;
        Vector2 position;
        float zoom, rotation;
        bool applyOnUpdate;

        //Camera shake
        Vector2 shake, shakeAmount, nextPoint;
        float shakeSpeed;
        double shakeTime;

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
            transform = Matrix.CreateTranslation(new Vector3(-position.X - shake.X, -position.Y - shake.Y, 0)) *
                Matrix.CreateRotationZ(rotation) *
                Matrix.CreateScale(new Vector3(zoom, zoom, 1)) *
                Matrix.CreateTranslation(new Vector3(Graphics.Viewport.Width * 0.5f, Graphics.Viewport.Height * 0.5f, 0));

            if (applyOnUpdate)
                Graphics.TransformMatrix = transform;
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
            position = Graphics.Viewport.Center.ToVector2();
            zoom = 1.0f;
            rotation = 0.0f;
            shake = Vector2.Zero;

            //Update the matrix
            UpdateMatrix();
        }

        public void Shake(Vector2 shakeAmount, float speed, double time)
        {
            this.shakeAmount = shakeAmount;
            this.shakeSpeed = speed;
            this.shakeTime = time;
        }
        public void Update()
        {
            if (shakeTime > 0)
            {
                //Update the shake time
                shakeTime -= Time.DeltaTime;

                //Pick a new random point to move to
                if (shake == nextPoint)
                    nextPoint = new Vector2(X.Random.NextFloat() * X.Random.NextParity() * shakeAmount.X, X.Random.NextFloat() * X.Random.NextParity() * shakeAmount.Y);
                MoveToPoint();
            }
            else if (shake != Vector2.Zero)
            {
                //Move to reset
                nextPoint = Vector2.Zero;
                MoveToPoint();
            }
        }
        private void MoveToPoint()
        {
            //Move the camera to the next point
            Vector2 move = nextPoint - shake;
            move.Normalize();
            move *= shakeSpeed * (float)Time.DeltaTime;

            //Check if move is not overshooting
            if (move.Length() >= Vector2.Distance(shake, nextPoint))
                shake = nextPoint;
            else
                shake += move;

            UpdateMatrix();
        }

        public Matrix TransformMatrix
        { get { return transform; } }
        //Position and rotation
        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
                UpdateMatrix();
            }
        }
        public float Zoom
        {
            get { return zoom; }
            set
            {
                zoom = value;
                UpdateMatrix();
            }
        }
        public float Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
                UpdateMatrix();
            }
        }
        //Shake
        public double ShakeTime
        { get { return shakeTime; } set { shakeTime = value; } }
        public Vector2 ShakeAmount
        {
            get { return shake; }
            set
            {
                shake = value;
                UpdateMatrix();
            }
        }
    }
}
