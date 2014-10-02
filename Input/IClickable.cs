using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XoticEngine.Input
{
    public interface IClickable
    {
        event InputManager.ClickEvent OnClick;

        Rectangle ClickRectangle { get; }
    }
}
