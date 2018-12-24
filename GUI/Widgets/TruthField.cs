using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Navi.Audio.Player;
using Navi.Screen;

namespace Navi.GUI.Widgets
{
    /// <summary>
    /// Generalization of checkboxes and radio buttons.
    /// </summary>
    public class TruthField : SurfaceWidget
    {
        private const float ColorScale = 0.85f;  // because of the shadow of an image we have to scale the color rectangle down
        private const int PixelTextureDimension = 1;

        private Rectangle pixelColorBoundingBox;
        private Texture2D onTexture;
        private Vector2 pixelColorPivot;

        public TruthField(ContentManager content, GraphicsDevice graphics, SoundPlayer soundPlayer, string imagePath, string imageCheckedPath)
            : base(content, graphics, soundPlayer, imagePath)
        {
            onTexture = content.Load<Texture2D>(imageCheckedPath);
        }

        public TruthField(ContentManager content, GraphicsDevice graphics, SoundPlayer soundPlayer, string imagePath, Color checkedColor)
            : base(content, graphics, soundPlayer, imagePath)
        {
            onTexture = new Texture2D(graphics, PixelTextureDimension, PixelTextureDimension);
            onTexture.SetData(new Color[] { checkedColor });
        }

        public bool IsSet { get; set; }

        public override void SetParameters(Vector2 percentElementSize, Vector2 percentElementPosition, Vector2 percentSurfaceSize, Vector2 pixelSurfaceResolution, Vector2 percentSurfaceOrigin)
        {
            base.SetParameters(percentElementSize, percentElementPosition, percentSurfaceSize, pixelSurfaceResolution, percentSurfaceOrigin);
            
            pixelColorBoundingBox = PixelColorBounds();
            pixelColorPivot = PixelPivot(onTexture);
        }

        protected Rectangle PixelColorBounds()
        {
            int oldWidth = pixelBoundingBox.Width;
            int oldHeight = pixelBoundingBox.Height;
            int width = (int)(oldWidth * ColorScale);
            int height = (int)(oldHeight * ColorScale);

            float translationX = (oldWidth - width) / 2.0f;
            float translationY = (oldHeight - height) / 2.0f;

            #region example
            // Pivot (0, 0):
            //
            //      oldWidth
            // --------------------->
            //
            //      width        l_1
            // -----------------><->
            //  ____________________
            // |                |   |   ^           ^
            // |                |   |   |           |
            // |                |   |   |           |
            // |                |   |   | height    |   oldHeight
            // |                |   |   |           |
            // |________________|   |   |           |
            // |                    |   ^           |
            // |____________________|   | l_2       |
            //
            // translationX = l_1/2
            // translationY = l_2/2
            // so... to center the small rectangle in the big rectangle
            // Pivot(0, 0)          then (+translationX, +translationY)
            // Pivot(50%, 50%)      then (0, 0)
            // Pivot(100%, 100%)    then (-translationX, -translationY)
            #endregion
            int posX = (int)(pixelBoundingBox.X + translationX * (1.0f - 2 * PercentPivotPoint.X / 100.0f));
            int posY = (int)(pixelBoundingBox.Y + translationY * (1.0f - 2 * PercentPivotPoint.Y / 100.0f));
            return new Rectangle(posX, posY, width, height);
        }

        public override void Update(double frameTime) { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsSet) base.Draw(spriteBatch);
            if (IsSet) spriteBatch.Draw(onTexture, pixelColorBoundingBox, null, Tint, radianRotation, pixelColorPivot, Effect, 0.0f);
        }
    }
}
