using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Navi.Audio.Player;
using Navi.Screen;

namespace Navi.GUI.Widgets
{
    /// <remarks>
    /// Some source code is taken from the CheckBox class of Navi (2014/2015).
    /// </remarks> 
    public sealed class CheckBox : TruthField
    {
        public CheckBox(ContentManager content, GraphicsDevice graphics, SoundPlayer soundPlayer, string imagePath, string imageCheckedPath)
            : base(content, graphics, soundPlayer, imagePath, imageCheckedPath)
        {
            this.OnClick += Check;
        }

        public CheckBox(ContentManager content, GraphicsDevice graphics, SoundPlayer soundPlayer, string imagePath, Color checkedColor)
            : base(content, graphics, soundPlayer, imagePath, checkedColor)
        {
            this.OnClick += Check;
        }
        private void Check(SurfaceWidget widget, Vector2 mousePos)
        {
            this.IsSet = !this.IsSet;
        }
    }
}
