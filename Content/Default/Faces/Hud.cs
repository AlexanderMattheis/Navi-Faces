using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Navi.Audio.Player;
using Navi.GUI;
using Navi.GUI.Widgets;
using Navi.Screen;
using Navi.System;
//using Console = System.Console;

namespace Navi.Content.Faces
{
    public sealed class Hud
    {
        public const string SelectedLabel = "lblPageDisplay";

        private int currentPage;

        private Interface face;

        public Hud(ContentManagement content, GraphicsDevice graphicsDevice, SoundPlayer soundPlayer, SurfaceManager surfaceManager, Interface face)
        {
            this.face = face;
        }

        public void Previous(SurfaceWidget sender, Vector2 mousePos)
        {
            Surface currentFace = null;

            currentPage--;
            if (currentPage < 0) currentPage = face.Subsurfaces.Count - 1;
            if (currentPage >= 0) currentFace = face.Subsurfaces[currentPage];

            SetCurrentPage(currentFace);
            UpdateLabels();
        }

        public void Next(SurfaceWidget sender, Vector2 mousePos)
        {
            Surface currentFace = null;

            currentPage++;
            if (currentPage >= face.Subsurfaces.Count) currentPage = 0;
            if (currentPage < face.Subsurfaces.Count) currentFace = face.Subsurfaces[currentPage];

            SetCurrentPage(currentFace);
            UpdateLabels();
        }

        public void SetCurrentPage(Surface currentFace)
        {
            if(currentFace != null)
            {
                face.MakeSubsurfacesUnusable(currentFace);
            }
        }

        private void UpdateLabels()
        {
            Label lblMapDisplay = (Label)face.SelectByName(SelectedLabel);
            lblMapDisplay.Text = (currentPage + 1).ToString();
            lblMapDisplay.ResetSetPivot();
        }
    }
}
