using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XoticEngine.Input
{
    public static partial class InputManager
    {
        //Events
        //Mouse
        public delegate void ClickEvent(object sender, ClickEventArgs e);
        public static event ClickEvent OnClick;
        //Keyboard
        public delegate void KeyEvent(object sender, KeyEventArgs k);
        public static event KeyEvent OnKeyPressed;
        public delegate void CharEvent(object sender, CharEventArgs c);
        public static event CharEvent OnCharEntered;

        internal static void Initialize()
        {
            Keyboard.Initialize();
            GamePad.Initialize();
        }

        internal static void Update()
        {
            Keyboard.Update();
            Mouse.Update();
            GamePad.Update();
        }
    }
}
