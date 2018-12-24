using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Navi.Audio.Player;
using Navi.Screen;

namespace Navi.GUI.Widgets
{
    /// <summary>
    /// To draw logos, textures and colored areas.
    /// </summary>
    /// <remarks>
    /// Is based on the Image class of Navi (2014/2015).
    /// </remarks>
    public sealed class Image : SurfaceWidget
    {
        public Image(Color color, GraphicsDevice graphics, SoundPlayer soundPlayer) : base(color, graphics, soundPlayer) { }

        public Image(ContentManager content, GraphicsDevice graphics, SoundPlayer soundPlayer, string imagePath) : base(content, graphics, soundPlayer, imagePath) { }

        #region [updater]
        public override void Update(double frameTime) { }
        #endregion
    }
}
