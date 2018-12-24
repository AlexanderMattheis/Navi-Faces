using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Navi.Scene.Tools;
using Navi.Screen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Navi.Input
{
    /// <summary>
    /// Processes the input of the user.
    /// </summary>
    /// <remarks>
    /// Some Source code is taken from the InputProcessor class of Navi (2014/2015).
    /// </remarks>
    public sealed class InputProcessor
    {
        private bool hitted;
        ////private bool started;
        private bool touched;

        private InputHandler inputHandler;

        public InputProcessor()
        {
            inputHandler = new InputHandler();

            hitted = false;
            ////started = false;
            ////touched = false;
        }

        private enum MouseButton
        {
            Left,
            Middle,
            Right
        }

        public void Process(SurfaceManager manager, Camera cam, double frameTime)
        {
            inputHandler.Update();
            if (inputHandler.NewInput)
            {
                List<Surface> surfaces = manager.Surfaces.ToList();
                Vector2 mousePos = inputHandler.MousePosition;

                Keyboard(surfaces, mousePos, frameTime);
                Mouse(surfaces, mousePos, cam, frameTime);
            }
        }

        public void Keyboard(List<Surface> surfaces, Vector2 mousePos, double frameTime)
        {
            if (inputHandler.KeyPress)
                KeyboardPress(inputHandler.PressedKeys, surfaces, mousePos, frameTime);

            if (inputHandler.KeyClick)
                KeyboardClick(inputHandler.ClickedKeys, surfaces, mousePos, frameTime);
        }

        public void KeyboardPress(Keys[] keys, List<Surface> surfaces, Vector2 mousePos, double frameTime)
        {
            foreach (Surface surface in surfaces)
            {
                if (surface.IsActive)
                {
                    // interface actions
                    foreach (Tuple<Surface.KeyboardAction, List<Keys[]>> tuple in surface.KeyPresses)
                    {
                        KeyboardAction(surface, tuple, keys, mousePos, frameTime);
                    }
                }
            }
        }

        private void KeyboardAction(Surface surface, Tuple<Surface.KeyboardAction, List<Keys[]>> tuple, Keys[] keys, Vector2 mousePos, double frameTime)
        {
            foreach (Keys[] shortcut in tuple.Item2)
            {
                if (keys.ContainsShortcut(shortcut))
                    tuple.Item1(surface, mousePos, frameTime);
            }
        }

        public void KeyboardClick(Keys[] keys, List<Surface> surfaces, Vector2 mousePos, double frameTime)
        {
            foreach (Surface surface in surfaces)
            {
                if (surface.IsActive)
                {
                    surface.Elements.Reverse();  // to hit the element at the top first
                    foreach (SurfaceWidget widget in surface.Elements)
                        Click(widget, keys, mousePos);
                    surface.Elements.Reverse();

                    // interface actions
                    foreach (Tuple<Surface.KeyboardAction, List<Keys[]>> tuple in surface.KeyClicks)
                    {
                        KeyboardAction(surface, tuple, keys, mousePos, frameTime);
                    }
                }
            }
        }

        public void Click(SurfaceWidget widget, Keys[] keys, Vector2 mousePos)
        {
            Keys[] widgetKeys = widget.EventKeys;

            if (widgetKeys != null)
            {
                foreach (Keys key in keys)
                {
                    foreach (Keys widgetKey in widgetKeys)
                    {
                        if (key == widgetKey)
                            widget.KeyboardClick(mousePos);
                    }
                }
            }
        }

        public void Mouse(List<Surface> surfaces, Vector2 mousePos, Camera cam, double frameTime)
        {
            if (inputHandler.MouseLeftClick)
                MouseButtonClick(surfaces, mousePos, cam, MouseButton.Left);

            if (inputHandler.MouseRightClick)
                MouseButtonClick(surfaces, mousePos, cam, MouseButton.Right);

            if (inputHandler.MouseMove)
                MouseMove(surfaces, mousePos, cam);
        }

        private void MouseButtonClick(List<Surface> surfaces, Vector2 mousePos, Camera cam, MouseButton button)
        {
            foreach (Surface surface in surfaces)
            {
                if (surface.IsActive)
                {
                    surface.Elements.Reverse();  // to hit the element at the top first
                    foreach (SurfaceWidget widget in surface.Elements)
                    {
                        Click(widget, mousePos, cam, button);
                        if (hitted) break;  // if a widget was hitted then we don't need to look beneath
                    }

                    surface.Elements.Reverse();

                    if (hitted) break;  // if a widget on a surface was hitted then we don't have to go through other surfaces
                }
            }

            hitted = false;
        }

        private void Click(SurfaceWidget widget, Vector2 mousePos, Camera cam, MouseButton button)
        {
            if (widget.IsWithin(mousePos))
            {
                if (button == MouseButton.Left)
                {
                    widget.MouseLeftClick(mousePos);
                    hitted = true;
                } 
                else if (button == MouseButton.Middle) 
                {
                    widget.MouseMiddleClick(mousePos);
                    hitted = true;
                }
                else if (button == MouseButton.Right)
                {
                    widget.MouseRightClick(mousePos);
                    hitted = true;
                } 
            }
        }

        private void MouseMove(List<Surface> surfaces, Vector2 mousePos, Camera cam)
        {
            foreach (Surface surface in surfaces)
            {
                if (surface.IsActive)
                {
                    surface.Elements.Reverse();
                    foreach (SurfaceWidget widget in surface.Elements)
                    {
                        Move(widget, mousePos, cam);

                        if (touched)  // if you have a mouse over effect then you don't want a mouse over effect on elements beneath
                            break;  // goto is not possible because in this case surface.Elements will be not reversed again
                    }

                    surface.Elements.Reverse();

                    Move(surface, mousePos, cam);

                    if (touched)  // if you have a mouse over effect then you don't want a mouse over effect in interfaces beneath
                        break;
                }
            }

            touched = false;
        }

        private void Move(SurfaceWidget widget, Vector2 mousePos, Camera cam)
        {
            if (widget.IsWithin(mousePos))
            {
                if (!widget.IsMouseOver)  // to fire an event only once
                    widget.MouseOver(mousePos);
                touched = true;
            }
            else
            {
                if (widget.IsMouseOver)  // to fire an event only once
                    widget.MouseLeave(mousePos);
            }
        }

        private void Move(Surface surface, Vector2 mousePos, Camera cam)
        {
            if (surface.PixelBoundingBox.Contains(mousePos))
                surface.Move(cam, mousePos);
        }
    }
}
