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
    public sealed class Elemental
    {
        private SaveGame<Static, Dynamic, Node> save;

        public Elemental(ContentManagement content, GraphicsDevice graphicsDevice, SoundPlayer soundPlayer, SurfaceManager surfaceManager, Interface face)
        {
            save = Save(surfaceManager);
        }

        private SaveGame<Static, Dynamic, Node> Save(SurfaceManager surfaceManager)
        {
            Scenery scene = (Scenery)surfaceManager.SelectByName(Scenery.SurfaceName);
            return scene.SaveGame;
        }

        public void SavePriorityQueueChanges(bool[] active)
        {
            save.PriorityQueue = active;
            //pathing.Init(interfaces.Ground, save, map);
        }

        public void SaveMainAlgorithmChanges(bool[] active)
        {
            save.MainAlgorithm = active;
            //pathing.Init(interfaces.Ground, save, map);
        }
    }
}
