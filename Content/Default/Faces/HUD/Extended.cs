using Microsoft.Xna.Framework.Graphics;
using Navi.AI.Arrangement;
using Navi.Audio.Player;
using Navi.GUI;
using Navi.Models;
using Navi.Scene;
using Navi.Screen;
using Navi.System;
using Navi.System.State;

namespace Navi.Content.Faces.HUD
{
    public sealed class Extended
    {
        private SaveGame<Static, Dynamic, Node> save;

        public Extended(ContentManagement content, GraphicsDevice graphicsDevice, SoundPlayer soundPlayer, SurfaceManager surfaceManager, Interface face)
        {
            save = Save(surfaceManager);
        }

        private SaveGame<Static, Dynamic, Node> Save(SurfaceManager surfaceManager)
        {
            Scenery scene = (Scenery)surfaceManager.SelectByName(Scenery.SurfaceName);
            return scene.SaveGame;
        }

        public void SaveDistanceMeasureChanges(bool[] active)
        {
            save.DistanceMeasure = active;
            //pathing.Init(interfaces.Ground, save, map);
        }

        public void SaveExpansionChanges(bool[] active)
        {
            save.Expansion = active;
            //pathing.Init(interfaces.Ground, save, map);
        }

        public void SaveOptionsChanges(bool[] active)
        {
            save.Options = active;
            //pathing.Init(interfaces.Ground, save, map);
        }
    }
}
