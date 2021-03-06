﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XoticEngine.GameObjects;

namespace XoticEngine.GraphicEffects
{
    public sealed class Camera : IMoveable, ICloneable
    {
        private Matrix transform;
        private Vector2 position, nextPosition;
        private float zoom, rotation;
        private float smoothness = 1;
        private bool applyOnUpdate = true;
        private Rectangle? bounds = null;

        //Camera shake
        private Vector2 shake, shakeAmount, nextPoint;
        private float shakeSpeed;
        private double shakeTime;

        public Camera()
        {
            Reset();
        }
        public Camera(Vector2 position)
        {
            Reset();
            SetPosition(position);
        }
        public Camera(Vector2 position, float zoom)
        {
            Reset();
            this.zoom = zoom;
            SetPosition(position);
        }
        public Camera(Vector2 position, float zoom, float rotation)
        {
            Reset();
            this.zoom = zoom;
            this.rotation = rotation;
            SetPosition(position);
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

        //Update the transform matrix
        public void UpdateMatrix()
        {
            //Check the bounds
            ConstrainBounds();

            //Create the transform matrix
            transform = Matrix.CreateTranslation(new Vector3(-(position + shake), 0)) *
                Matrix.CreateRotationZ(rotation) *
                Matrix.CreateScale(new Vector3(zoom, zoom, 1)) *
                Matrix.CreateTranslation(new Vector3(Graphics.Viewport.Center.ToVector2(), 0));

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
            if (bounds.HasValue)
            {
                Vector2 topLeft = bounds.Value.Location.ToVector2() + Size / 2;
                Vector2 bottomRight = new Vector2(bounds.Value.Right, bounds.Value.Bottom) - Size / 2;
                position = Vector2.Clamp(position, topLeft, bottomRight);
                nextPosition = Vector2.Clamp(nextPosition, topLeft, bottomRight);
            }
        }

        public void Reset()
        {
            //Reset all the variables
            SetPosition(Graphics.Viewport.Center.ToVector2());
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

        //IMoveable
        public void Move(Vector2 amount)
        {
            NextPosition += amount;
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
            move *= shakeSpeed * TimeF.DeltaTime;

            //Check if move is not overshooting
            if (move.Length() >= Vector2.Distance(shake, nextPoint))
                shake = nextPoint;
            else
                shake += move;
        }

        //ICloneable
        public object Clone()
        {
            return new Camera(position, zoom, rotation)
            {
                ApplyOnUpdate = this.applyOnUpdate,
                Bounds = this.bounds,
                Smoothness = this.smoothness
            };
        }

        public bool ApplyOnUpdate
        { get { return applyOnUpdate; } set { applyOnUpdate = value; } }
        public Matrix TransformMatrix
        { get { return transform; } }
        public Rectangle? Bounds
        {
            get { return bounds; }
            set
            {
                bounds = value;
                UpdateMatrix();
            }
        }
        public Vector2 Size
        { get { return Graphics.Viewport.Size().ToVector2() / zoom; } }
        public float Smoothness
        { get { return 1.0f - smoothness; } set { smoothness = 1.0f - value; } }
        //Position and rotation
        public Vector2 Position
        { get { return position; } set { position = value; } }
        public Vector2 NextPosition
        { get { return nextPosition; } set { nextPosition = value; } }
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
