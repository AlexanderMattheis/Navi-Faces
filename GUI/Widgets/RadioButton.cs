using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Navi.Audio.Player;
using Navi.Screen;

namespace Navi.GUI.Widgets
{
    public sealed class RadioButton : TruthField
    {
        public RadioButton(ContentManager content, GraphicsDevice graphics, SoundPlayer soundPlayer, string imagePath, string imageSelectedPath)
            : base(content, graphics, soundPlayer, imagePath, imageSelectedPath)
        {
            this.OnClick += Check;
        }

        public RadioButton(ContentManager content, GraphicsDevice graphics, SoundPlayer soundPlayer, string imagePath, Color selectedColor)
            : base(content, graphics, soundPlayer, imagePath, selectedColor)
        {
            this.OnClick += Check;
        }
        private void Check(SurfaceWidget widget, Vector2 mousePos)
        {
            this.IsSet = true;
        }
    }
}
