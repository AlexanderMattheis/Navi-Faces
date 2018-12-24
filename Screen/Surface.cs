using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Navi.Scene.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Navi.Screen
{
    /// <summary>
    /// Stores elements like buttons and labels.
    /// To allow different display resolutions the
    /// width's and height's are stored in percent.
    /// </summary>
    /// <remarks>
    /// Is based on the Interface class of Navi (2014/2015).
    /// </remarks>
    public class Surface
    {
        public Surface(Vector2 resolution, string name, Surface parent, Vector2 percentSize)
        {
            Init(resolution, name, parent, percentSize);
        }

        public Surface(Vector2 resolution, string name, Surface parent)
        {
            Init(resolution, name, parent, new Vector2(100.0f, 100.0f));
        }

        #region [events]
        public delegate void Action(object args);

        public delegate void MouseAction(Surface sender, Camera cam, Vector2 mousePos);

        public delegate void KeyboardAction(Surface sender, Vector2 mousePos, double frameTime);

        public event MouseAction OnPressStart;

        public event MouseAction OnPress;

        public event MouseAction OnClick;

        public event MouseAction OnMove;
        #endregion

        public enum HorizontalAlignments
        {
            Left, 
            Center, 
            Right
        }

        public enum KeyboardActions
        {
            Click,
            Press
        }

        #region [events]
        public List<Tuple<Action, object>> Actions { get; set; }

        public List<Tuple<KeyboardAction, List<Keys[]>>> KeyPresses { get; set; }

        public List<Tuple<KeyboardAction, List<Keys[]>>> KeyClicks { get; set; }
        #endregion

        public bool IsActive { get; set; }

        public bool IsScrollable { get; set; }

        public bool IsVisible { get; set; }

        public string Name { get; private set; }

        public Surface Parent { get; set; }

        public Rectangle PixelBoundingBox { get; set; }

        public Vector2 PixelMaxPosition { get; set; }

        #region bounds
        public Vector2 PercentOrigin { get; set; }

        public Vector2 PercentSize { get; set; }
        #endregion

        #region container
        public List<SurfaceWidget> Elements { get; set; }

        public List<Surface> Subsurfaces { get; set; }
        #endregion

        #region display
        public float AspectRatio { get; set; }

        public Vector2 PixelResolution { get; set; }
        #endregion

        #region [events]
        public void Click(Camera cam, Vector2 mousePos)
        {
            if (OnClick != null && IsActive)
                OnClick(this, cam, mousePos);
        }

        public void Move(Camera cam, Vector2 mousePos)
        {
            if (OnMove != null && IsActive)
                OnMove(this, cam, mousePos);
        }

        public void Press(Camera cam, Vector2 mousePos)
        {
            if (OnPress != null && IsActive)
                OnPress(this, cam, mousePos);
        }

        public void PressStart(Camera cam, Vector2 mousePos)
        {
            if (OnPressStart != null && IsActive)
                OnPressStart(this, cam, mousePos);
        }
        #endregion

        #region [loader]
        private void Init(Vector2 resolution, string name, Surface parent, Vector2 percentSize)
        {
            InitDimensionProperties(resolution, percentSize);
            InitEvents();
            InitLists();
            InitStates();

            Name = name;
            Parent = parent;
        }

        private void InitDimensionProperties(Vector2 resolution, Vector2 percentSize)
        {
            AspectRatio = resolution.X / resolution.Y;
            PercentSize = percentSize;
            PixelBoundingBox = Bounds(percentSize, resolution);
            PixelResolution = resolution;
        }

        private void InitEvents()
        {
            KeyClicks = new List<Tuple<KeyboardAction, List<Keys[]>>>();
            KeyPresses = new List<Tuple<KeyboardAction, List<Keys[]>>>();
        }

        private void InitLists()
        {
            Actions = new List<Tuple<Action, object>>();
            Subsurfaces = new List<Surface>();
            Elements = new List<SurfaceWidget>();
        }

        private void InitStates()
        {
            IsActive = true;
            IsVisible = true;
        }
        #endregion

        #region [loner]
        private Rectangle Bounds(Vector2 percentSize, Vector2 resolution)
        {
            Vector2 pixelPosition = new Vector2();
            pixelPosition.X = (float)((PercentOrigin.X / (100.0f * AspectRatio)) * PixelResolution.X) * (percentSize.X / 100.0f);
            pixelPosition.Y = (float)((PercentOrigin.Y / 100.0f) * PixelResolution.Y) * (percentSize.Y / 100.0f);
            Vector2 pixelDimension = (percentSize / 100.0f) * resolution;

            Rectangle pixelBoundingBox = new Rectangle((int)pixelPosition.X, (int)pixelPosition.Y, (int)pixelDimension.X, (int)pixelDimension.Y);
            return pixelBoundingBox;
        }

        public void Add(SurfaceWidget element, float percentPosX, float percentPosY, float percentWidth, float percentHeight)
        {
            Vector2 percentPosition = new Vector2(percentPosX, percentPosY);
            Vector2 percentSize = new Vector2(percentWidth, percentHeight);

            element.SetParameters(percentSize, percentPosition, this.PercentSize, this.PixelResolution, this.PercentOrigin);
            RecalcutlateMaxPosition(element);
            Elements.Add(element);
        }

        protected void RecalcutlateMaxPosition(SurfaceWidget element)
        {
            int pixelElementPositionX = element.PixelBoundingBox.X;
            int pixelElementPositionY = element.PixelBoundingBox.Y;
            int pixelElementWidth = element.PixelBoundingBox.Width;
            int pixelElementHeight = element.PixelBoundingBox.Height;

            float maxPixelPositionX = 0.0f;
            float maxPixelPositionY = 0.0f;

            if (PixelMaxPosition.X < pixelElementPositionX)
                maxPixelPositionX = pixelElementPositionX + pixelElementWidth * ((100.0f - element.PercentPivotPoint.X) / 100.0f);

            if (PixelMaxPosition.Y < pixelElementPositionY)
                maxPixelPositionY = pixelElementPositionY + pixelElementHeight * ((100.0f - element.PercentPivotPoint.Y) / 100.0f);

            PixelMaxPosition = new Vector2(maxPixelPositionX, maxPixelPositionY);
        }

        public void AddEvent(Tuple<KeyboardAction, List<Keys[]>> keyboardEvent, KeyboardActions type)
        {
            switch (type)
            {
                case KeyboardActions.Click: KeyClicks.Add(keyboardEvent);   break;
                case KeyboardActions.Press: KeyPresses.Add(keyboardEvent);  break;
            }
        }

        public void MakeSubsurfacesUnusable(Surface activate)
        {
            foreach (Surface surface in Subsurfaces)
            {
                surface.SetUsabilityState(false);
            }

            activate.SetUsabilityState(true);
        }

        public void SetUsabilityState(bool isUsable)
        {
            SetActivationState(isUsable);
            SetVisibilityState(isUsable);
        }

        public void SetActivationState(bool isActive)
        {
            IsActive = isActive;

            foreach (Surface surface in Subsurfaces)
                surface.SetActivationState(isActive);
        }

        public void SetVisibilityState(bool isVisible)
        {
            IsVisible = isVisible;

            foreach (Surface surface in Subsurfaces)
                surface.SetVisibilityState(isVisible);
        }

        public SurfaceWidget SelectByName(string name)
        {
            return (from widget in this.Elements where widget.Name == name select widget).ToArray()[0];
        } 
        #endregion

        #region [updater]
        public virtual void Update(double frameTime)
        {
            foreach (SurfaceWidget element in Elements)
            {
                element.Update(frameTime);
            }

            foreach (Tuple<Action, object> actionTuple in Actions)
            {
                actionTuple.Item1(actionTuple.Item2);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (SurfaceWidget element in Elements)
            {
                element.Draw(spriteBatch);
            }
        }
        #endregion
    }
}
