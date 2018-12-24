using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Navi.Input
{
    /// <summary>
    /// Is used by the InputProcessor to look if the user has
    /// performed a key click, mouse click, mouse move or some
    /// other type of input.
    /// </summary>
    /// <remarks>
    /// Some source code is taken from the InputHandler class of Navi (2014/2015).
    /// </remarks> 
    public sealed class InputHandler
    {
        private KeyboardState keyboardState;
        private KeyboardState lastKeyboardState;

        private MouseState mouseState;
        private MouseState lastMouseState;

        public Vector2 MousePosition
        {
            get { return new Vector2(mouseState.X, mouseState.Y); }
        }

        public Keys[] ClickedKeys
        {
            get { return lastKeyboardState.GetPressedKeys(); }
        }

        public Keys[] PressedKeys
        {
            get { return keyboardState.GetPressedKeys(); }
        }

        #region input opportunities
        public bool KeyClick { get; private set; }

        public bool KeyPress { get; private set; }

        public bool MouseLeftClick { get; private set; }

        public bool MouseMove { get; private set; }

        public bool MouseRightClick { get; private set; }

        public bool NewInput { get; private set; }
        #endregion

        /// <remarks>Do not forget to update NewInput (at bottom), if you change something in this method!</remarks>
        public void Update()
        {
            // last
            lastKeyboardState = keyboardState;
            lastMouseState = mouseState;

            // current
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            KeyClick =
                keyboardState.GetPressedKeys().Length == 0 &&
                lastKeyboardState.GetPressedKeys().Length > 0;

            KeyPress = keyboardState.GetPressedKeys().Length > 0;

            MouseLeftClick = 
                mouseState.LeftButton == ButtonState.Released && 
                lastMouseState.LeftButton == ButtonState.Pressed;

            MouseMove = mouseState.X != lastMouseState.X || mouseState.Y != lastMouseState.Y;

            MouseRightClick =
                mouseState.RightButton == ButtonState.Released &&
                lastMouseState.RightButton == ButtonState.Pressed;

            NewInput = KeyClick || KeyPress || MouseLeftClick || MouseMove || MouseRightClick;
        }
    }
}
