using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ScorpionEngine.GameObjects;

namespace ScorpionEngine
{
    public class Camera2D : GameObject
    {
        //Position etc, movement
        Vector2 origin;
        float scale;
        float rotation;
        float moveSpeed;
        //Screen
        Vector2 screenCenter;
        //Transform matrix
        Matrix transform;
        //Focus object
        GameObject focus;

        public Camera2D(GameObject focus)
            : base("Camera2D")
        {
            screenCenter = new Vector2(SE.Viewport.Width / 2, SE.Viewport.Height / 2);
            scale = 0.5f;
            moveSpeed = 1.25f;
            this.focus = focus;
        }

        public override void Update()
        {
            //Create the transform matrix
            transform = Matrix.Identity *
                        Matrix.CreateTranslation(-Position.X, -Position.Y, 0) *
                        Matrix.CreateRotationZ(rotation) *
                        Matrix.CreateTranslation(origin.X, origin.Y, 0) *
                        Matrix.CreateScale(new Vector3(scale, scale, scale));

            //origin = screenCenter / scale;

            //Move the camera
            RelativePosition = new Vector2(focus.Position.X - Position.X, focus.Position.Y - Position.Y) * (moveSpeed * (float)SE.DeltaTime.TotalSeconds);

            SE.CameraMatrix = transform;

            base.Update();
        }

        #region Properties
        public float Rotation
        { get { return rotation; } set { rotation = value; } }
        public Vector2 Origin
        { get { return origin; } set { origin = value; } }
        public float Scale
        { get { return scale; } set { scale = value; } }
        public GameObject Focus
        { get { return focus; } set { focus = value; } }
        public float MoveSpeed
        { get { return moveSpeed; } set { moveSpeed = value; } }
        #endregion
    }
}