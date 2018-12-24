using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Navi.Audio.Player;
using Navi.System.State;
using System;

namespace Navi.Screen
{
    public abstract class SurfaceWidget : IDisposable
    {
        protected float radianRotation;

        #region monogame
        private GraphicsDevice graphics;
        #endregion

        #region navi
        protected SoundPlayer soundDevice;
        #endregion

        protected Rectangle pixelBoundingBox;

        protected Vector2 pixelPivotPoint;

        protected Texture2D texture;

        protected Color[] pixels;

        private Vector2 textureDimension;

        public SurfaceWidget() { }

        public SurfaceWidget(Color color, GraphicsDevice graphics, SoundPlayer soundPlayer)
        {
            Init(color, graphics, soundPlayer);
        }

        public SurfaceWidget(ContentManager content, GraphicsDevice graphics, SoundPlayer soundPlayer, string imagePath)
        {
            Init(content, graphics, soundPlayer, imagePath);
        }

        #region [events]
        public delegate void Action(SurfaceWidget sender, Vector2 mousePos);

        public event Action OnClick;

        public event Action OnMouseLeave;

        public event Action OnMouseMiddleClick;

        public event Action OnMouseOver;

        public event Action OnMouseRightClick;
        #endregion

        public Vector2 PercentPivotPoint { get; set; }

        public float DegreeRotation { get; set; }

        public SpriteEffects Effect { get; set; }

        public Keys[] EventKeys { get; set; }

        public bool IsActive { get; set; }

        public bool IsMouseOver { get; set; }

        public string Name { get; set; }

        public float PercentScale { get; set; }

        public Rectangle PixelBoundingBox 
        { 
            get
            {
                return pixelBoundingBox;
            }

            set
            {
                pixelBoundingBox = value;
            }
        }

        public bool ReceivesMouseActions { get; set; }

        public string SoundPath { get; set; }

        public Color Tint { get; set; }

        #region [events]
        public void KeyboardClick(Vector2 mousePos)
        {
            if (OnClick != null && IsActive)
                OnClick(this, mousePos);
        }

        public void MouseLeave(Vector2 mousePos)
        {
            if (OnMouseLeave != null && IsActive && ReceivesMouseActions)
                OnMouseLeave(this, mousePos);
        }

        public void MouseLeftClick(Vector2 mousePos)
        {
            if (OnClick != null && IsActive && ReceivesMouseActions)
            {
                OnClick(this, mousePos);
                PlaySound();
            }
        }

        private void PlaySound()
        {
            if (soundDevice != null && SoundPath != null)
                soundDevice.Play(SoundPath, Settings.PercentSoundVolume);
        }

        public void MouseMiddleClick(Vector2 mousePos)
        {
            if (OnClick != null && IsActive && ReceivesMouseActions)
                OnMouseMiddleClick(this, mousePos);
        }

        public void MouseRightClick(Vector2 mousePos)
        {
            if (OnClick != null && IsActive && ReceivesMouseActions)
                OnMouseRightClick(this, mousePos);
        }

        public void MouseOver(Vector2 mousePos)
        {
            if (OnMouseOver != null && IsActive && ReceivesMouseActions)
                OnMouseOver(this, mousePos);
        }
        #endregion

        #region [leaver]
        public void Dispose()
        {
            if (texture != null)
            {
                texture.Dispose();
                texture = null;
            }
        }
        #endregion

        #region [loader]
        public virtual void Init(Color color, GraphicsDevice graphics, SoundPlayer soundPlayer)
        {
            texture = new Texture2D(graphics, 1, 1);
            texture.SetData(new Color[] { color });
            InitializeDevices(graphics, soundPlayer);
        }

        public virtual void Init(ContentManager content, GraphicsDevice graphics, SoundPlayer soundPlayer, string imagePath)
        {
            texture = content.Load<Texture2D>(imagePath);
            InitializeDevices(graphics, soundPlayer);
        }

        private void InitializeDevices(GraphicsDevice graphics, SoundPlayer soundPlayer)
        {
            this.graphics = graphics;
            textureDimension = new Vector2(texture.Width, texture.Height);

            if (Settings.Sound) soundDevice = soundPlayer;
            SetActive();
        }

        private void SetActive()
        {
            IsActive = true;
            ReceivesMouseActions = true;
        }

        public void SetPixelArray()
        {
            if (texture != null)  // text does not need a texture
            {
                // creates a texture copy that have the size of the pixelBoundingBox to allow checking 
                // if the mouse is within the texture of pixelBoundingBox size (resulting size on the screen)
                Texture2D scaledTexture = ScaleTexture(texture, pixelBoundingBox);
                pixels = new Color[scaledTexture.Width * scaledTexture.Height];
                scaledTexture.GetData<Color>(pixels);
            }
        }

        private Texture2D ScaleTexture(Texture2D texture, Rectangle destinationRectangle)
        {
            RenderTarget2D target = new RenderTarget2D(graphics, destinationRectangle.Width, destinationRectangle.Height);
            Rectangle renderRectangle = new Rectangle(0, 0, destinationRectangle.Width, destinationRectangle.Height);

            graphics.SetRenderTarget(target);
            graphics.Clear(Color.Transparent);
            {
                SpriteBatch spriteBatch = new SpriteBatch(graphics);
                spriteBatch.Begin();
                    spriteBatch.Draw(texture, renderRectangle, null, Color.White);
                spriteBatch.End();
            }

            graphics.SetRenderTarget(null);

            return target;
        }

        public void SetPixelArray(int width, int height)
        {
            if (texture != null)  // text does not need a texture
            {
                Color[] data = new Color[texture.Width * texture.Height];
                texture.GetData<Color>(data);
                pixels = new Color[width * height];

                for (int i = 0; i < pixels.Length; i++)
                {
                    pixels[i] = data[0];
                }
            }
        }
        #endregion

        #region [updater]
        public abstract void Update(double frameTime);

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, pixelBoundingBox, null, Tint, radianRotation, pixelPivotPoint, Effect, 0.0f);
        }
        #endregion

        public bool IsWithin(Vector2 mousePos)
        {
            Vector2 pos = new Vector2(pixelBoundingBox.X, pixelBoundingBox.Y);
            Vector2 dimension = new Vector2(pixelBoundingBox.Width, pixelBoundingBox.Height);
            Vector2 floatingPointPivot = new Vector2(PercentPivotPoint.X, PercentPivotPoint.Y) / 100.0f;

            float leftUpperEdgePixelPosX = (float)(pos.X - floatingPointPivot.X * dimension.X);
            float leftUpperEdgePixelPosY = (float)(pos.Y - floatingPointPivot.Y * dimension.Y);

            int textureCoordinateX = (int)Math.Round(mousePos.X - leftUpperEdgePixelPosX);
            int textureCoordinateY = (int)Math.Round(mousePos.Y - leftUpperEdgePixelPosY);

            Color pixelColor = Pixel(textureCoordinateX, textureCoordinateY);
            
            if (pixelColor.A != 0)
                return true;
            else
                return false;
        }

        protected Color Pixel(int x, int y)
        {
            #region example
            // 2D:
            //          5
            //     <--------->
            //      0 1 2 3 4
            //
            // 0    x x x x x
            // 1    o x x x x
            // 2    x x x x x
            // 3    x x x x x
            // 4    x x x x x
            //
            //     0  1  2  3  4  5  6 ...
            // 1D: x  x  x  x  x  o  x ...
            //
            // position of o:   0 + 1 * 5 = 5, 
            // ->correct:       pixels[5] = o
            #endregion
            int arrayPosition = x + y * pixelBoundingBox.Width;
            bool isWithinTexture = x >= 0 && y >= 0 && x < pixelBoundingBox.Width && y < pixelBoundingBox.Height;
            if (pixels != null && isWithinTexture && arrayPosition < pixels.Length)
                return pixels[arrayPosition];
            return default(Color);
        }

        #region set data
        public virtual void SetParameters(Vector2 percentElementSize, Vector2 percentElementPosition, Vector2 percentSurfaceSize, Vector2 pixelSurfaceResolution, Vector2 percentSurfaceOrigin)
        {
            SetSize(percentElementSize, percentSurfaceSize, pixelSurfaceResolution);
            SetPosition(percentElementPosition, percentSurfaceSize, pixelSurfaceResolution, percentSurfaceOrigin);
            SetPixelData();

            radianRotation = (float)(DegreeRotation * Math.PI) / 180;  // reshaped: rad / pi = degree / 180°
            pixelPivotPoint = PixelPivot(texture);
        }

        private void SetSize(Vector2 percentElementSize, Vector2 percentSurfaceSize, Vector2 pixelSurfaceResolution)
        {
            float percentOfScreenWidht = percentSurfaceSize.X;
            float percentOfScreenHeight = percentSurfaceSize.Y;
            float floatingPointScale = PercentScale / 100.0f;

            // divided by 100*100 because both values are in percent
            float finalFloatingPointWidth = (percentElementSize.X * percentOfScreenWidht * floatingPointScale) / 10000.0f;
            float finalFloatingPointHeight = (percentElementSize.Y * percentOfScreenHeight * floatingPointScale) / 10000.0f;

            pixelBoundingBox.Width = (int)(finalFloatingPointWidth * pixelSurfaceResolution.Y);
            pixelBoundingBox.Height = (int)(finalFloatingPointHeight * pixelSurfaceResolution.Y);
        }

        private void SetPosition(Vector2 percentElementPosition, Vector2 percentSurfaceSize, Vector2 pixelSurfaceResolution, Vector2 percentSurfaceOrigin)
        {
            float elementFloatingPointPosX = (percentElementPosition.X + percentSurfaceOrigin.X) / 100.0f;
            float elementFloatingPointPosY = (percentElementPosition.Y + percentSurfaceOrigin.Y) / 100.0f;

            double floatingPointOfSurface = percentSurfaceSize.Y / 100.0f;  // percentOfScreenHeight / 100.0f to get point numbers

            int finalPixelPosX = (int)(elementFloatingPointPosX * pixelSurfaceResolution.Y);
            int finalPixelPosY = (int)(elementFloatingPointPosY * pixelSurfaceResolution.Y * floatingPointOfSurface);

            pixelBoundingBox.X = finalPixelPosX;
            pixelBoundingBox.Y = finalPixelPosY;
        }

        private void SetPixelData()
        {
            if (textureDimension.X == 1 && textureDimension.Y == 1) SetPixelArray((int)pixelBoundingBox.X, (int)pixelBoundingBox.Y);
            else SetPixelArray();
        }

        public Vector2 PixelPivot(Texture2D texture)
        {
            if (texture != null)
            {
                Vector2 textureDimension = new Vector2(texture.Width, texture.Height);
                return new Vector2(textureDimension.X, textureDimension.Y) * (PercentPivotPoint / 100.0f);
            }

            return Vector2.Zero;
        }
        #endregion
    }
}
