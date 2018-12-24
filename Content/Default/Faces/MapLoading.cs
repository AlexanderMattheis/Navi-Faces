using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Navi.Audio.Player;
using Navi.Defaults;
using Navi.GUI;
using Navi.GUI.Widgets;
using Navi.Helper.System;
using Navi.Scene;
using Navi.Screen;
using Navi.System;
using Navi.World.Tools;
using System;
using System.Collections.Generic;

namespace Navi.Content.Faces
{
    public sealed class MapLoading
    {
        public const string NextSurface = "Wait";
        public const string Editor = "Editor";
        public const string IngameHud = "Hud";

        public const string SelectedLabel = "lblMapDisplay";
        public const string SelectedCheckBox = "chbEditMode";

        private int currentFile;
        private List<string> fileNames;
        private string selectedMap;

        private Interface face;

        private ContentManagement content;
        private GraphicsDevice graphicsDevice;
        private SoundPlayer soundPlayer;
        private SurfaceManager surfaceManager;

        public MapLoading(ContentManagement content, GraphicsDevice graphicsDevice, SoundPlayer soundPlayer, SurfaceManager surfaceManager, Interface face)
        {
            fileNames = new Directory().FileNames(FileExtensions.Map, Paths.Maps);
            if (fileNames.Count > 0) selectedMap = fileNames[0];

            this.face = face;
            this.content = content;
            this.graphicsDevice = graphicsDevice;
            this.soundPlayer = soundPlayer;
            this.surfaceManager = surfaceManager;

            UpdateLabels();
        }

        public void Previous(SurfaceWidget sender, Vector2 mousePos)
        {
            currentFile--;

            if (currentFile < 0) currentFile = fileNames.Count - 1;

            if (currentFile < fileNames.Count) selectedMap = fileNames[currentFile];

            UpdateLabels();
        }

        public void Next(SurfaceWidget sender, Vector2 mousePos)
        {
            currentFile++;

            if (currentFile >= fileNames.Count) currentFile = 0;

            if (currentFile < fileNames.Count) selectedMap = fileNames[currentFile];

            UpdateLabels();
        }

        private void UpdateLabels()
        {
            Label lblMapDisplay = (Label)face.SelectByName(SelectedLabel);
            lblMapDisplay.Text = fileNames[currentFile].ToUpper();
            lblMapDisplay.ResetSetPivot();
        }

        public void Start(SurfaceWidget sender, Vector2 mousePos)
        {
            face.SetVisibilityState(false);  // this.face

            Scenery scenerySurface = (Scenery)surfaceManager.SelectByName(Scenery.SurfaceName);
            StartLoading(scenerySurface);
        }

        private void StartLoading(Surface surface)
        {
            Surface loading = new Interface(content, graphicsDevice, soundPlayer, surfaceManager, surface.PixelResolution, NextSurface, null).Instance();
            loading.Actions.Add(new Tuple<Surface.Action, object>(Load, surface));
            surfaceManager.Push(loading);
        }

        public void Load(object args)
        {
            Scenery scenerySurface = (Scenery)args;
            
            //LoadMap(scenerySurface);
            LoadHud(scenerySurface.PixelResolution);

            scenerySurface.SetActivationState(true);
        }

        private void LoadMap(Scenery scenerySurface)
        {
            string mapName = selectedMap + FileExtensions.Map;

            content.Map.Unload();
            scenerySurface.Map = MapLoader.Load(Paths.Maps + mapName, scenerySurface.SaveGame, content.Map, graphicsDevice, soundPlayer);
            scenerySurface.PlaceModelsOnSurface();
        }

        private void LoadHud(Vector2 pixelResolution)
        {
            CheckBox chbEditMode = (CheckBox)face.SelectByName(SelectedCheckBox);
            Interface hud = null;

            if (chbEditMode.IsSet)
                hud = CreateHud(Editor, pixelResolution);
            else
                hud = CreateHud(IngameHud, pixelResolution);

            surfaceManager.Pop();  // Wait
            surfaceManager.Pop();  // MapLoading
            //Console.WriteLine(surfaceManager.Surfaces.Count);
            surfaceManager.Push(hud);
        }

        public Interface CreateHud(String name, Vector2 pixelResolution)
        {
            Interface hud = null;

            if(name.Equals(IngameHud))
            {
                hud = new Interface(content, graphicsDevice, soundPlayer, surfaceManager, pixelResolution, IngameHud, null).Instance();

                Interface elemental = new Interface(content, graphicsDevice, soundPlayer, surfaceManager, pixelResolution, "HUD\\Elemental", null).Instance();
                Interface extended = new Interface(content, graphicsDevice, soundPlayer, surfaceManager, pixelResolution, "HUD\\Extended", null).Instance();
                Interface options = options = new Interface(content, graphicsDevice, soundPlayer, surfaceManager, pixelResolution, "HUD\\Options", null).Instance();

                extended.SetUsabilityState(false);
                options.SetUsabilityState(false);

                hud.Subsurfaces.Add(elemental);
                hud.Subsurfaces.Add(extended);
                hud.Subsurfaces.Add(options);
            }
            else //if(name.Equals(Editor))
            {
                hud = new Interface(content, graphicsDevice, soundPlayer, surfaceManager, pixelResolution, Editor, null).Instance();
                Plant.State = Plant.ProgramState.Editor;
            }
           
            return hud;
        }
    }
}
