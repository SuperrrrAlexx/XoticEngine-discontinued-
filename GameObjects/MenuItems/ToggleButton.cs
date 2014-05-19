using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ScorpionEngine.GameObjects.MenuItems
{
    public class ToggleButton : GameObject
    {
        #region Fields
        Rectangle rect;
        bool toggled;
        SpriteSheet sheet;
        //Actions
        public event Action OnToggle;
        #endregion

        #region Constructors
        public ToggleButton(string name, Rectangle rect, SpriteSheet sheet, bool toggled)
            : base(name, new Vector2(rect.X, rect.Y))
        {
            this.rect = rect;
            this.toggled = toggled;
            this.sheet = sheet;
        }
        #endregion

        #region Methods
        public override void Update()
        {
            //Update the position
            rect.Location = new Point((int)RelativePosition.X, (int)RelativePosition.Y);

            //Check if the mouse is within the rectangle
            if (rect.Contains(Input.MousePosition))
            {
                //Check for clicks
                if (Input.LeftClicked())
                {
                    //Toggle checked
                    toggled = !toggled;

                    //Call the action
                    if (OnToggle != null)
                        OnToggle();
                }
            }

            base.Update();
        }

        public override void Draw(SpriteBatch s)
        {
            //Draw the checkbox
            s.Draw(sheet.Get(toggled ? 1 : 0), rect, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);

            base.Draw(s);
        }
        #endregion

        #region Properties
        public bool Toggled
        { get { return toggled; } set { toggled = value; } }
        #endregion
    }
}
