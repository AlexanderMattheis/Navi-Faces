using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Navi.Audio.Player;
using Navi.Screen;
using System;

namespace Navi.GUI.Widgets
{
    /// <summary>
    /// To create elements on which you can put some text.
    /// </summary>
    /// <remarks>
    /// Is based on the Label class of Navi (2014/2015).
    /// </remarks>
    public class Label : SurfaceWidget
    {
        private const float StringScale = 0.00075f;

        protected SpriteFont font;
        protected float textScale;
        protected Vector2 pixelPosition;
        protected Vector2 pixelTextPivotPoint;
        protected float radianTextRotation;

        public Label(ContentManager content, string fontPath)
        {
            InitializeText(content, fontPath);
        }

        public Label(ContentManager content, GraphicsDevice graphics, SoundPlayer soundPlayer, Color color, string fontPath)
            : base(color, graphics, soundPlayer)
        {
            InitializeText(content, fontPath);
        }

        public Label(ContentManager content, GraphicsDevice graphics, SoundPlayer soundPlayer, string imagePath, string fontPath) 
            : base(content, graphics, soundPlayer, imagePath)
        {
            InitializeText(content, fontPath);
        }

        public enum HorizontalAlignments
        {
            Left, Center, Right
        }

        public HorizontalAlignments HorizontalAlignment { get; set; }

        public string Text { get; set; }

        public Color TextColor { get; set; }

        public float TextDegreeRotation { get; set; }

        public SpriteEffects TextEffect { get; set; }

        public float PercentTextScale { get; set; }

        public Vector2 PercentTextPivotPoint { get; set; }

        #region [loader]
        private void InitializeText(ContentManager content, string fontPath)
        {
            font = content.Load<SpriteFont>(fontPath);
        }
        #endregion

        #region set bounds
        public override void SetParameters(Vector2 percentElementSize, Vector2 percentElementPosition, Vector2 percentSurfaceSize, Vector2 pixelSurfaceResolution, Vector2 percentSurfaceOrigin)
        {
            base.SetParameters(percentElementSize, percentElementPosition, percentSurfaceSize, pixelSurfaceResolution, percentSurfaceOrigin);
            SetTextPosition();
            textScale = (pixelSurfaceResolution.Y * (percentSurfaceSize.Y / 100.0f)) * StringScale * (PercentTextScale / 100.0f);  // makes the textScale dependant on the resolution and percentSize of the surface
            radianTextRotation = (float)((TextDegreeRotation + DegreeRotation) * Math.PI) / 180;  // reshaped: rad / pi = degree / 180°
        }

        private void SetTextPosition()
        {
            ResetSetPivot();
            SetAlignment();
        }

        public void ResetSetPivot()
        {
            Vector2 pixelTextdimension = font.MeasureString(Text);
            pixelTextPivotPoint = pixelTextdimension * (PercentTextPivotPoint / 100.0f);
        }

        private void SetAlignment()
        {
            pixelPosition = new Vector2(pixelBoundingBox.X, pixelBoundingBox.Y);  // position of a default label without an image

            // setting position within an image
            if (texture != null)
            {
                int pixelPosX = pixelBoundingBox.X;
                int pixelWidth = pixelBoundingBox.Width;
                int pixelPosY = pixelBoundingBox.Y;
                int pixelHeight = pixelBoundingBox.Height;
                Vector2 pointNumberPivotPoint = PercentPivotPoint / 100.0f;

                if (HorizontalAlignment == HorizontalAlignments.Left)
                    pixelPosition = new Vector2((float)(pixelPosX - pixelWidth * pointNumberPivotPoint.X), 
                                                (float)(pixelPosY + pixelHeight * (0.5f - pointNumberPivotPoint.Y)));
                else if (HorizontalAlignment == HorizontalAlignments.Center)
                    pixelPosition = new Vector2((float)(pixelPosX + pixelWidth * (0.5f - pointNumberPivotPoint.X)),
                                                (float)(pixelPosY + pixelHeight * (0.5f - pointNumberPivotPoint.Y)));
                else if (HorizontalAlignment == HorizontalAlignments.Right)
                    pixelPosition = new Vector2((float)(pixelPosX + pixelWidth * (1.0f - pointNumberPivotPoint.X)),
                                                (float)(pixelPosY + pixelHeight * (0.5f - pointNumberPivotPoint.Y)));
            }
        }
        #endregion

        #region [updater]
        public override void Update(double frameTime) { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (texture != null)
                base.Draw(spriteBatch);
            DrawText(spriteBatch);
        }

        protected void DrawText(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, Text, pixelPosition, TextColor, radianTextRotation, pixelTextPivotPoint, textScale, TextEffect, 0.0f);
        }
        #endregion
    }
}
