using Microsoft.Xna.Framework;
using Navi.Screen;

using CsharpSystem = System;

namespace Navi.GUI.Faces.Parser
{
    /// <summary>
    /// Stores functions which can be assigned to widgets.
    /// </summary>
    public class Methods
    {
        private SurfaceManager surfaceManager;
        private Surface parent;

        public Methods(SurfaceManager surfaceManager)
        {
            this.surfaceManager = surfaceManager;
        }

        public Methods(SurfaceManager surfaceManager, Surface parent)
        {
            this.surfaceManager = surfaceManager;
            this.parent = parent;
        }

        public Interface NextInterface { get; set; }

        public void Back(SurfaceWidget sender, Vector2 mousePos)
        {
            surfaceManager.Pop();
            surfaceManager.Push(parent);
        }

        public void Exit(SurfaceWidget sender, Vector2 mousePos)
        {
            sender.Dispose();
            CsharpSystem.Environment.Exit(1);  // not a nice way because propably some errors can happen, so before plant.Exit(); was used
        }

        public void Push(SurfaceWidget sender, Vector2 mousePos)
        {
            surfaceManager.Pop();
            surfaceManager.Push(NextInterface);
        }
    }
}
