using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XoticEngine.GraphicEffects
{
    public class Camera
    {
        private Matrix transform;
        private Vector2 position, nextPosition, size;
        private float zoom, rotation;
        private float smoothness = 1;
        private bool applyOnUpdate;
        private Rectangle bounds;

        //Camera shake
        private Vector2 shake, shakeAmount, nextPoint;
        private float shakeSpeed;
        private double shakeTime;

        public Camera(bool applyOnUpdate)
        {
            this.applyOnUpdate = applyOnUpdate;
            this.size = new Vector2(Graphics.Viewport.Width, Graphics.Viewport.Height);
            Reset();
        }
        public Camera(Vector2 position, float zoom, float rotation, bool applyOnUpdate)
        {
            this.applyOnUpdate = applyOnUpdate;
            this.size = new Vector2(Graphics.Viewport.Width, Graphics.Viewport.Height);
            this.position = position;
            UpdateMatrix(position, zoom, rotation);
        }

        public void Update()
        {
            //Update the smooth position
            position = Vector2.Lerp(position, nextPosition, smoothness);

            //Update the shake
            if (shakeTime > 0)
            {
                //Update the shake time
                shakeTime -= Time.DeltaTime;

                //Pick a new random point to move to
                if (shake == nextPoint)
                    nextPoint = new Vector2(X.Random.NextFloat() * X.Random.NextSign() * shakeAmount.X, X.Random.NextFloat() * X.Random.NextSign() * shakeAmount.Y);
                MoveToPoint();
            }
            else if (shake != Vector2.Zero)
            {
                //Move to reset
                nextPoint = Vector2.Zero;
                MoveToPoint();
            }

            UpdateMatrix();
        }

        public void UpdateMatrix()
        {
            //Check the bounds
            ConstrainBounds();

            //Create the transform matrix
            transform = Matrix.CreateTranslation(new Vector3(-position - shake, 0)) *
                Matrix.CreateRotationZ(rotation) *
                Matrix.CreateScale(new Vector3(zoom, zoom, 1)) *
                Matrix.CreateTranslation(new Vector3(size * 0.5f, 0));

            //Apply the matrix
            if (applyOnUpdate)
                Graphics.TransformMatrix = transform;
        }
        public void UpdateMatrix(Vector2 position, float zoom, float rotation)
        {
            //Update the variables
            this.nextPosition = position;
            this.zoom = zoom;
            this.rotation = rotation;

            //Update the matrix
            UpdateMatrix();
        }

        private void ConstrainBounds()
        {
            //Constrain the x and y to the bounds
            if (bounds.Width > 0)
                position.X = MathHelper.Clamp(position.X, bounds.Left + size.X / 2, bounds.Right - size.X / 2);
            if (bounds.Height > 0)
                position.Y = MathHelper.Clamp(position.Y, bounds.Top + size.Y / 2, bounds.Bottom - size.Y / 2);
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
        public void SetPosition(Vector2 position)
        {
            //Save the position as the next and current position
            this.nextPosition = position;
            this.position = position;
        }

        public void Shake(Vector2 shakeAmount, float speed, double time)
        {
            this.shakeAmount = shakeAmount;
            this.shakeSpeed = speed;
            this.shakeTime = time;
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
        }

        public Matrix TransformMatrix
        { get { return transform; } }
        public Rectangle Bounds
        {
            get { return bounds; }
            set
            {
                bounds = value;
                UpdateMatrix();
            }
        }
        public Vector2 Size
        {
            get { return size; }
            set
            {
                size = value;
                UpdateMatrix();
            }
        }
        public float Smoothness
        { get { return 1.0f - smoothness; } set { smoothness = 1.0f - value; } }
        //Position and rotation
        public Vector2 Position
        {
            get { return nextPosition; }
            set
            {
                nextPosition = Vector2.Clamp(value, bounds.Location.ToVector2() + size / 2, new Vector2(bounds.Right, bounds.Bottom) - size / 2); ;
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
