using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Navi.AI.Arrangement;
using Navi.Audio.Player;
using Navi.Defaults;
using Navi.World;
using Navi.Models;
using Navi.World.Tools;
using Navi.Input;
using Navi.Scene.Tools;
using Navi.Screen;
using Navi.System;
using Navi.System.State;
using System;
using System.Collections.Generic;

using NaviModel = Navi.Models.Model;

namespace Navi.Scene
{
    /// <remarks>
    /// Is based on the Actionground class of Navi (2014/2015).
    /// </remarks>
    public sealed class Scenery : Surface
    {
        public const string SurfaceName = "Scenery";
        public const float PointValueScrollMargin = 0.00005f;

        private double elapsedTime;

        public Scenery(SaveGame<Static, Dynamic, Node> saveGame, ContentManagement contentAccess, GraphicsDevice graphicsDevice, SoundPlayer soundPlayer, SurfaceManager surfaceManager, Camera cam, Vector2 resolution, Vector2 percentSize)
            : base(resolution, SurfaceName, null, percentSize)
        {
            SaveGame = saveGame;
            this.Cam = cam;
            this.Map = MapLoader.Load(Paths.DefaultMap, saveGame, contentAccess.Map, graphicsDevice, soundPlayer);

            PlaceModelsOnSurface();
            SelfLink();
            SetSurfaceProperties();
        }

        public Camera Cam { get; set; }

        public Map Map { get; set; }

        public SaveGame<Static, Dynamic, Node> SaveGame { get; set; }

        #region [loader]
        public void PlaceModelsOnSurface()
        {
            for (int i = 0; i < Map.Models.Count; i++)
            {
                NaviModel element = (NaviModel)Map.Models[i];

                float pointValueInterfaceSizeX = PercentSize.X / 100.0f;
                float pointValueInterfaceSizeY = PercentSize.Y / 100.0f;
                double pixelInterfaceResolutionX = PixelResolution.X / AspectRatio;
                double pixelInterfaceResolutionY = PixelResolution.Y;

                Vector2 pixelTranslation;  // vector because later it's maybe possible to scale the width and height independently
                SetDimensions(out pixelTranslation, element, pixelInterfaceResolutionX, pixelInterfaceResolutionY);
                SetPositions(pixelTranslation, element, pointValueInterfaceSizeX, pointValueInterfaceSizeY, pixelInterfaceResolutionX, pixelInterfaceResolutionY);

                element.PixelBoundingBox = new Rectangle((int)element.PixelPosX, (int)element.PixelPosY, (int)element.PixelWidth, (int)element.PixelHeight);
            }

            CreateVisibilityList();  // to get something on the surface even if this surface is not active
        }

        private void SetDimensions(out Vector2 pixelTranslation, NaviModel element, double pixelInterfaceResolutionX, double pixelInterfaceResolutionY)
        {
            // width
            double percentWidth = element.PixelWidth / pixelInterfaceResolutionY;
            double pointValueResolutionX = pixelInterfaceResolutionX / 100.0f;
            double pointValueElementSizeX = element.PercentScale / 100.0f;
            double pixelWidthUnscaled = (percentWidth * PercentSize.X) * pointValueResolutionX;
            element.PixelWidth = pixelWidthUnscaled * pointValueElementSizeX;
            float translationX = (float)(element.PixelWidth - pixelWidthUnscaled) / 2.0f;

            // height
            double percentHeight = element.PixelHeight / pixelInterfaceResolutionY;
            double pointValueResolutionY = pixelInterfaceResolutionY / 100.0f;
            double pointValueElementSizeY = element.PercentScale / 100.0f;
            double pixelHeightUnscaled = (percentHeight * PercentSize.Y) * pointValueResolutionY;
            element.PixelHeight = pixelHeightUnscaled * pointValueElementSizeY;
            float translationY = (float)(element.PixelHeight - pixelHeightUnscaled) / 2.0f;

            element.PixelDimension = new Vector2((float)Math.Round(element.PixelWidth, 6),
                                                (float)Math.Round(element.PixelHeight, 6));  // to avoid floating point errors the double value is rounded to the precision of float
           
            RecalcutlateMaxPosition(element);
            pixelTranslation = new Vector2(translationX, translationY);
        }

        private void SetPositions(Vector2 pixelTranslation, NaviModel element, float pointValueInterfaceSizeX, float pointValueInterfaceSizeY, double pixelInterfaceResolutionX, double pixelInterfaceResolutionY)
        {
            // position x
            double percentPosX = element.PixelPosX / PixelResolution.Y;
            element.PixelPosX = ((percentPosX + (PercentOrigin.X / 100.0f)) * pixelInterfaceResolutionX) * pointValueInterfaceSizeX - pixelTranslation.X;

            // position y
            double percentPosY = element.PixelPosY / PixelResolution.Y;
            element.PixelPosY = (percentPosY + PercentOrigin.Y / 100.0f) * pixelInterfaceResolutionY * pointValueInterfaceSizeY - pixelTranslation.Y;

            element.PixelPosition = new Vector2((float)Math.Round(element.PixelPosX, 6),
                                                (float)Math.Round(element.PixelPosY, 6));  // to avoid floating point errors the double value is rounded to the precision of float
        }

        private void SelfLink()
        {
            OnMove += Cam.Movement.Scrolling;
            SetCameraMovementKeys();
        }

        public void SetCameraMovementKeys()
        {
            AddEvent(new Tuple<KeyboardAction, List<Keys[]>>(Cam.Movement.ScrollingBottom, Shortcuts.ScrollBottom), KeyboardActions.Press);
            AddEvent(new Tuple<KeyboardAction, List<Keys[]>>(Cam.Movement.ScrollingTop, Shortcuts.ScrollTop), KeyboardActions.Press);
            AddEvent(new Tuple<KeyboardAction, List<Keys[]>>(Cam.Movement.ScrollingLeft, Shortcuts.ScrollLeft), KeyboardActions.Press);
            AddEvent(new Tuple<KeyboardAction, List<Keys[]>>(Cam.Movement.ScrollingRight, Shortcuts.ScrollRight), KeyboardActions.Press);
        }

        private void SetSurfaceProperties()
        {
            IsScrollable = true;
            IsActive = false;
        }
        #endregion

        #region [updater]
        public override void Update(double frameTime)
        {
            base.Update(frameTime);
            SetVisibles(frameTime);
            Cam.Movement.Update(frameTime,  this);
        }

        private void SetVisibles(double frameTime)
        {
            if (elapsedTime >= Settings.MsTimespanForViewUpdate)
            {
                elapsedTime = 0.0;
                CreateVisibilityList();
            }

            elapsedTime += frameTime;
        }

        private void CreateVisibilityList()
        {
            Elements.Clear();

            foreach (NaviModel element in Map.Models)
                if (element != null)  // && Cam.PixelViewport.Contains(element.PixelPosition))
                    Elements.Add(element);
        }
        #endregion
    }
}
