using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XoticEngine.GameObjects
{
    public class KinematicProperties
    {
        private GameObject gameObject;
        private bool positionWithParent, rotateWithParent, scaleWithParent, moveWithParentRotation;

        public KinematicProperties(GameObject gameObject)
        {
            this.gameObject = gameObject;
            this.positionWithParent = true;
            this.rotateWithParent = true;
            this.scaleWithParent = true;
            this.moveWithParentRotation = true;
            gameObject.UpdatePosition();
        }

        public bool PositionWithParent
        {
            get { return positionWithParent; }
            set
            {
                positionWithParent = value;
                gameObject.UpdatePosition();
            }
        }
        public bool RotateWithParent
        {
            get { return rotateWithParent; }
            set
            {
                rotateWithParent = value;
                gameObject.UpdatePosition();
            }
        }
        public bool ScaleWithParent
        {
            get { return scaleWithParent; }
            set
            {
                scaleWithParent = value;
                gameObject.UpdatePosition();
            }
        }
        public bool MoveWithParentRotation
        {
            get { return moveWithParentRotation; }
            set
            {
                moveWithParentRotation = value;
                gameObject.UpdatePosition();
            }
        }
    }
}
