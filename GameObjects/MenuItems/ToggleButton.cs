using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XoticEngine.GameObjects.MenuItems
{
    public class ToggleButton : GameObject
    {
        Rectangle rect;
        bool toggled;
        SpriteSheet sheet;
        //Actions
        public event Action OnToggle;

        public ToggleButton(string name, Rectangle rect, SpriteSheet sheet, bool toggled)
            : base(name, new Vector2(rect.X, rect.Y), 0)
        {
            this.rect = rect;
            this.toggled = toggled;
            this.sheet = sheet;
        }

        public override void Update()
        {
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

        public override void Draw(SpriteBatch gameBatch, SpriteBatch additiveBatch, SpriteBatch guiBatch)
        {
            //Draw the checkbox
            guiBatch.Draw(sheet[toggled ? 1 : 0], rect, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);

            base.Draw(gameBatch, additiveBatch, guiBatch);
        }

        public bool Toggled
        { get { return toggled; } set { toggled = value; } }
    }
}
