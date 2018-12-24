using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Navi.Audio.Player;
using Navi.Screen;

namespace Navi.Models
{
    /// <remarks>
    /// Is based on the Tile class of Navi (2014/2015).
    /// </remarks>
    public sealed class Static : Model
    {
        public Static() { }

        public Static(Model model) : base(model) { }

        public Static(ContentManager content, GraphicsDevice graphics, SoundPlayer soundPlayer, string imagePath)
            : base(content, graphics, soundPlayer, imagePath)
        {
            if (Plant.State == Plant.ProgramState.Editor) InitForGame();
        }

        private void InitForGame()
        {
            OnClick += RecalculatePaths;
            OnMouseMiddleClick += ModifyMap;
            OnMouseRightClick += Unselect;
        }

        private void RecalculatePaths(SurfaceWidget widget, Vector2 mousePos)
        {
        }

        private void ModifyMap(SurfaceWidget widget, Vector2 mousePos) 
        { 
        }

        private void Unselect(SurfaceWidget widget, Vector2 mousePos)
        {
        }
    }
}
