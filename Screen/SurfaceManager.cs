using Microsoft.Xna.Framework.Graphics;
using Navi.Scene.Tools;
using System.Collections.Generic;
using System.Linq;

namespace Navi.Screen
{
    /// <summary>
    /// To push and pop Surface's.
    /// </summary>
    /// <remarks>
    /// Is based on the InterfaceHandler class of Navi (2014/2015).
    /// </remarks>
    public sealed class SurfaceManager
    {
        public SurfaceManager()
        {
            Surfaces = new Stack<Surface>();
        }

        public Stack<Surface> Surfaces { get; set; }

        public Surface SelectByName(string name)
        {
            return (from face in Surfaces where face.Name == name select face).ToList()[0];
        }

        public void Push(Surface surface)
        {
            foreach (Surface ui in surface.Subsurfaces)
            {
                Push(ui);
            }

            Surfaces.Push(surface);
        }

        public void Pop()
        {
            Surface userInterface = Surfaces.Pop();

            foreach (Surface ui in userInterface.Subsurfaces)
            {
                Pop();
            }
        }

        #region [updater]
        public void Update(double frameTime)
        {
            // needed because it is not allowed to modify the collection during
            // the following iteration
            IEnumerable<Surface> surfaces = Surfaces.ToArray();

            ////Console.WriteLine(Surfaces.Count);
            foreach (Surface surface in surfaces)
            {
                if (surface.IsActive)
                {
                    surface.Update(frameTime);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Camera cam)
        {
            foreach (Surface surface in Surfaces.Reverse())  // interfaces have to be drawn in reverse order
            {
                if (surface.IsVisible)
                {
                    if (surface.IsScrollable)
                        DrawScrollables(spriteBatch, surface, cam);
                    else
                        DrawStatics(spriteBatch, surface);
                }
            }
        }

        private void DrawScrollables(SpriteBatch spriteBatch, Surface surface, Camera cam)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, cam.TranslationMatrix);
                surface.Draw(spriteBatch);
            spriteBatch.End();
        }

        private void DrawStatics(SpriteBatch spriteBatch, Surface surface)
        {
            spriteBatch.Begin();
                surface.Draw(spriteBatch);
            spriteBatch.End();
        }
        #endregion
    }
}
