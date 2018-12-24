using Microsoft.Xna.Framework;
using Navi.Screen;
using Navi.System.State;

namespace Navi.Scene.Tools
{
    /// <summary>
    /// Access on the viewport and the position of the observer.
    /// </summary>
    /// <remarks>
    /// Some source code is taken from the Camera class of Navi (2014/2015).
    /// </remarks>
    public sealed class Camera
    {
        private Vector2 pixelPosition;
        private Rectangle pixelViewport;

        public Camera(int screenHeight)
        {
            Movement = new Motion(this, screenHeight);
        }

        public Motion Movement { get; set; }

        public Rectangle PixelViewport
        {
            get
            {
                return pixelViewport;
            }
        }

        public Vector2 Position 
        {
            get
            {
                return pixelPosition;
            }

            set
            {
                pixelPosition = value;
            }
        }

        /// <summary>
        /// Returns the translation matrix that is used for example to scroll with the camera.
        /// </summary>
        public Matrix TranslationMatrix
        {
            get
            {
                return Matrix.CreateTranslation(-pixelPosition.X, -pixelPosition.Y, 0.0f);  // "-" to test positive values in the update method
            }
        }

        /// <summary>
        /// To move the camera around.
        /// </summary>
        public sealed class Motion
        {
            /// <summary>
            /// To regulate the speed of the camera.
            /// </summary>
            private const double SpeedRegulator = 0.001;

            #region data
            private float pixelScrollAreaMargin;  // margin which activates camera movement
            private double speed;
            #endregion

            #region states
            private bool isBottom;
            private bool isLeft;
            private bool isRight;
            private bool isTop;
            #endregion

            private Camera cam;

            public Motion(Camera cam, int screenHeight)
            {
                this.cam = cam;
                pixelScrollAreaMargin = Scenery.PointValueScrollMargin * Settings.PixelScreenWidth * Settings.PixelScreenHeight;
                speed = SpeedRegulator * Settings.CameraSpeed * screenHeight;
            }

            public void Scrolling(Surface surface, Camera cam, Vector2 mousePos)
            {
                int x = surface.PixelBoundingBox.X;
                int y = surface.PixelBoundingBox.Y;
                int width = surface.PixelBoundingBox.Width;
                int height = surface.PixelBoundingBox.Height;

                isLeft      =   x                                   <=  mousePos.X  &&  mousePos.X  <=  x + pixelScrollAreaMargin;
                isRight     =   x + width - pixelScrollAreaMargin   <=  mousePos.X  &&  mousePos.X  <=  x + width;
                isTop       =   y                                   <=  mousePos.Y  &&  mousePos.Y  <=  y + pixelScrollAreaMargin;
                isBottom    =   y + height - pixelScrollAreaMargin  <=  mousePos.Y  &&  mousePos.Y  <=  y + height;
            }

            public void ScrollingBottom(Surface surface, Vector2 mousePos, double frameTime)
            {
                if (cam.pixelPosition.Y <= surface.PixelMaxPosition.Y) cam.pixelPosition.Y += (float)(frameTime * speed);
                cam.pixelViewport.Y = (int)(cam.pixelPosition.Y + surface.PixelBoundingBox.Y - pixelScrollAreaMargin);
            }

            public void ScrollingLeft(Surface surface, Vector2 mousePos, double frameTime)
            {
                if (cam.pixelPosition.X >= 0) cam.pixelPosition.X -= (float)(frameTime * speed);
                cam.pixelViewport.X = (int)(cam.pixelPosition.X + surface.PixelBoundingBox.X - pixelScrollAreaMargin);
            }

            public void ScrollingRight(Surface surface, Vector2 mousePos, double frameTime)
            {
                if (cam.pixelPosition.X <= surface.PixelMaxPosition.X) cam.pixelPosition.X += (float)(frameTime * speed);
                cam.pixelViewport.X = (int)(cam.pixelPosition.X + surface.PixelBoundingBox.X - pixelScrollAreaMargin);
            }

            public void ScrollingTop(Surface surface, Vector2 mousePos, double frameTime)
            {
                if (cam.pixelPosition.Y >= 0) cam.pixelPosition.Y -= (float)(frameTime * speed);
                cam.pixelViewport.Y = (int)(cam.pixelPosition.Y + surface.PixelBoundingBox.Y - pixelScrollAreaMargin);
            }

            public void Update(double frameTime, Surface surface)
            {
                if (isLeft      && cam.pixelPosition.X >= 0.0f)                         cam.pixelPosition.X -= (float)(frameTime * speed);
                if (isRight     && cam.pixelPosition.X <= surface.PixelMaxPosition.X)   cam.pixelPosition.X += (float)(frameTime * speed);
                if (isTop       && cam.pixelPosition.Y >= 0.0f)                         cam.pixelPosition.Y -= (float)(frameTime * speed);
                if (isBottom    && cam.pixelPosition.Y <= surface.PixelMaxPosition.Y)   cam.pixelPosition.Y += (float)(frameTime * speed);

                cam.pixelViewport.X = (int)(cam.pixelPosition.X + surface.PixelBoundingBox.X - pixelScrollAreaMargin);
                cam.pixelViewport.Y = (int)(cam.pixelViewport.Y + surface.PixelBoundingBox.Y - pixelScrollAreaMargin);
            }
        }
    }
}
