using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Navi.Audio.Player;
using Navi.GUI.Faces.Parser;
using Navi.Screen;
using Navi.System;

namespace Navi.GUI
{
    public class Interface : Surface
    {
        public Interface(ContentManagement contentAccess, GraphicsDevice graphicsDevice, SoundPlayer soundPlayer, SurfaceManager surfaceManager, Vector2 resolution, string name, Surface parent)
            : base(resolution, name, parent)
        {
            Compiler = new Compiler(contentAccess, graphicsDevice, soundPlayer, surfaceManager, this, true);
        }

        public Compiler Compiler { get; set; }

        public Interface Instance()
        {
            return Compiler.Create();
        }
    }
}
