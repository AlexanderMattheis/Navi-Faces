using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Navi.Audio.Player;
using Navi.Screen;
using System;

namespace Navi.GUI.Widgets
{
    /// <remarks>
    /// Is based on the IconButton class of Navi (2014/2015).
    /// </remarks>
    public sealed class IconButton : SurfaceWidget
    {
        private Texture2D icon;
        private Vector2 realIconDimension;
        private Vector2 pixelIconPivotPoint;
        private Rectangle pixelIconBoundingBox;
        private float radianIconRotation;

        public IconButton(ContentManager content, GraphicsDevice graphics, SoundPlayer soundPlayer, string imagePath, string iconPath)
            : base(content, graphics, soundPlayer, imagePath)
        {
            icon = content.Load<Texture2D>(iconPath);
            realIconDimension = new Vector2(icon.Width, icon.Height);
        }

        public IconButton(ContentManager content, GraphicsDevice graphics, SoundPlayer soundPlayer, Color color, string iconPath)
            : base(color, graphics, soundPlayer)
        {
            icon = new Texture2D(graphics, 1, 1);
            icon.SetData(new Color[] { color });
            realIconDimension = new Vector2(icon.Width, icon.Height);
        }

        public SpriteEffects IconEffect { get; set; }

        public float IconDegreeRotation { get; set; }

        public Vector2 PercentIconPivot { get; set; }

        public override void SetParameters(Vector2 percentElementSize, Vector2 percentElementPosition, Vector2 percentSurfaceSize, Vector2 pixelSurfaceResolution, Vector2 percentSurfaceOrigin)
        {
            base.SetParameters(percentElementSize, percentElementPosition, percentSurfaceSize, pixelSurfaceResolution, percentSurfaceOrigin);
            SetIconBoundingBox();
        }

        private void SetIconBoundingBox()
        {
            // the icon should rotate with the object beneath on which it is sitting, so: IconDegreeRotation + DegreeRotation
            radianIconRotation = (float)((IconDegreeRotation + DegreeRotation) * Math.PI) / 180;  // reshaped: rad / pi = degree / 180°

            // center the icon pivot
            PercentIconPivot = new Vector2(50.0f, 50.0f);
            pixelIconPivotPoint = realIconDimension * (PercentIconPivot / 100.0f);

            // distance from the pivot of the button rectangle to the old pivot position of the icon's rectangle
            #region example
            // pivot a of the button rectangle shall be in the left upper corner (0, 0) and pivot b of the icon rectangle is always in the center
            //
            // a____________    _
            // | \          |   |
            // |  \ d       |   |
            // |   \        |   |
            // |     b      |   | height
            // |            |   |
            // |            |   |
            // |____________|   |
            //                  -
            // |------------|
            //      width
            //
            // where d = pixelDistanceBetweenPivots
            #endregion
            double buttonRectanglePixelPivotX = pixelBoundingBox.X;
            double buttonRectanglePixelPivotY = pixelBoundingBox.Y;
            Vector2 iconPivotPixelWorldCoordinates = OverlayObjectPivotPixelWorldCoordinates(PercentIconPivot);
            double iconRectangleOldPixelPivotX = iconPivotPixelWorldCoordinates.X;
            double iconRectangleOldPixelPivotY = iconPivotPixelWorldCoordinates.Y;

            Vector2 pixelDistanceBetweenPivotsVector = Distance(buttonRectanglePixelPivotX, buttonRectanglePixelPivotY, iconRectangleOldPixelPivotX, iconRectangleOldPixelPivotY);
            double pixelDistanceBetweenPivotsLength = pixelDistanceBetweenPivotsVector.Length();

            double radianAngleBetweenPivotsVectorAndXAxis = AngleBetween(pixelDistanceBetweenPivotsVector, Vector2.UnitX);  // how much it is rotated from beginning
            float radianRotationOfButton = radianRotation;
            Vector2 newIconCenterPixelCoordinates = RotatedCoordinates(buttonRectanglePixelPivotX, buttonRectanglePixelPivotY, pixelDistanceBetweenPivotsLength, radianRotationOfButton, radianAngleBetweenPivotsVectorAndXAxis);
            int iconCenterXAfterButtonRotation = (int)newIconCenterPixelCoordinates.X;
            int iconCenterYAfterButtonRotation = (int)newIconCenterPixelCoordinates.Y;

            SetBoundingBox(ref pixelIconBoundingBox, iconCenterXAfterButtonRotation, iconCenterYAfterButtonRotation, pixelBoundingBox.Width, pixelBoundingBox.Height);
        }

        private Vector2 OverlayObjectPivotPixelWorldCoordinates(Vector2 percentObjectTextureCoordinates)
        {
            // the pixelBoundingBox x- and y-coordinate give us the pivot of the button
            // which could be anywhere in the button's area
            // ... so first searching the left upper corner (0, 0) (world coordinates) of the button
            double leftUpperCornerX = pixelBoundingBox.X - (pixelBoundingBox.Width * (PercentPivotPoint.X / 100.0));
            double leftUpperCornerY = pixelBoundingBox.Y - (pixelBoundingBox.Height * (PercentPivotPoint.Y / 100.0));

            // .. to find the object's old pivot position (world coordinates) before the rotation
            double objectRectangleOldPixelPivotX = leftUpperCornerX + pixelBoundingBox.Width * (percentObjectTextureCoordinates.X / 100.0);
            double objectRectangleOldPixelPivotY = leftUpperCornerY + pixelBoundingBox.Height * (percentObjectTextureCoordinates.Y / 100.0);

            return new Vector2((float)objectRectangleOldPixelPivotX, (float)objectRectangleOldPixelPivotY);
        }

        private Vector2 Distance(double pos1X, double pos1Y, double pos2X, double pos2Y)
        {
            return new Vector2((float)(pos2X - pos1X), (float)(pos2Y - pos1Y));
        }

        private double AngleBetween(Vector2 vec1, Vector2 vec2)
        {
            double dotProduct = Vector2.Dot(vec1, vec2);
            double lengthsProduct = vec1.Length() * vec2.Length();
            double radianAngle = lengthsProduct > 0.0 ? Math.Acos(dotProduct / lengthsProduct) : 0.0;
            radianAngle = vec1.Y - vec2.Y < 0 ? -radianAngle : radianAngle;

            return radianAngle;
        }

        private Vector2 RotatedCoordinates(double pixelPivotPositionX, double pixelPivotPositionY, double pixelRadius, double rotationAngle, double radianStartAngle)
        {
            // newX = centre.X + radius * cos(angle)
            // newY = centre.Y + radius * sin(angle)
            float newPixelPosX = (float)(pixelPivotPositionX + pixelRadius * Math.Cos(rotationAngle + radianStartAngle));
            float newPixelPosY = (float)(pixelPivotPositionY + pixelRadius * Math.Sin(rotationAngle + radianStartAngle));
            return new Vector2(newPixelPosX, newPixelPosY);
        }

        private void SetBoundingBox(ref Rectangle boundingBox, int x, int y, int width, int height)
        {
            boundingBox.X = x;
            boundingBox.Y = y;
            boundingBox.Width = width;
            boundingBox.Height = height;
        }

        #region [updater]
        public override void Update(double frameTime) { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (icon != null)
                spriteBatch.Draw(icon, pixelIconBoundingBox, null, Tint, radianIconRotation, pixelIconPivotPoint, IconEffect, 0.0f);
        }
        #endregion
    }
}
