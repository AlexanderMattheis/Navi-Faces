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
    public class Model : SurfaceWidget
    {
        private Vector2 pointValueTexturePivotPoint;

        public Model() 
        {
            pointValueTexturePivotPoint = new Vector2(0.5f, 0.5f);
            PercentScale = 100.0f;
        }

        public Model(Model model)
        {
            this.pixelBoundingBox = model.pixelBoundingBox;
            this.texture = model.texture;

            this.PixelDimension = model.PixelDimension;
            this.PixelHeight = model.PixelHeight;
            this.PixelPosition = model.PixelPosition;
            this.PixelPosX = model.PixelPosX;
            this.PixelPosY = model.PixelPosY;
            this.PercentScale = model.PercentScale;
            this.PixelWidth = model.PixelWidth;
        }

        public Model(ContentManager content, GraphicsDevice graphics, SoundPlayer soundPlayer, string imagePath)
            : base(content, graphics, soundPlayer, imagePath)
        {
            Init(content, graphics, soundPlayer, imagePath);
            pointValueTexturePivotPoint = new Vector2(0.5f, 0.5f);
        }

        public Texture2D Selected { get; set; }

        public bool IsBlocked { get; set; }

        public Vector2 PixelDimension { get; set; }

        public Vector2 PixelPosition { get; set; }
        
        public double PixelHeight { get; set; }

        public double PixelPosX { get; set; }

        public double PixelPosY { get; set; }

        public double PixelWidth { get; set; }

        /// <remarks>
        /// Has to be called if the default constructor (without parameters) has been used.
        /// </remarks>
        public override void Init(ContentManager content, GraphicsDevice graphics, SoundPlayer soundPlayer, string imagePath)
        {
            base.Init(content, graphics, soundPlayer, imagePath);
            if (Plant.State == Plant.ProgramState.Editor) InitForEditor();
        }

        private void InitForEditor()
        {
            OnClick += AddSelection;
            OnMouseRightClick += Remove;
        }

        private void AddSelection(SurfaceWidget widget, Vector2 mousePos)
        {
        }

        private void Remove(SurfaceWidget widget, Vector2 mousePos)
        {
        }

        public override void Update(double frameTime) { }
    }
}
