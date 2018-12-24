using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Navi.AI.Arrangement;
using Navi.Audio;
using Navi.Audio.Player;
using Navi.Defaults;
using Navi.World;
using Navi.Models;
using Navi.GUI;
using Navi.Input;
using Navi.Scene;
using Navi.Scene.Tools;
using Navi.Screen;
using Navi.System.State;
using System;

namespace Navi.System
{
    /// <summary>
    /// Used to initialize all classes.
    /// </summary>
    public sealed class ProgramLoader
    {
        private const string LoadingSurface = "Loading";
        private const string ProgramParameterFile = "Program.cfg";
        private const string ContentRootDirectory = "Content";

        private static int loadState;

        private Plant plant;
        private Vector2 resolution;

        #region [loader]
        public void Load(Plant plant)
        {
            InitStatics(plant);

            // Monogame
            plant.Content.RootDirectory = ContentRootDirectory;
            LoadGraphicsPreferences(plant.Graphics, plant, out resolution);
            LoadWindowPreferences(plant);

            plant.Drawer = new SpriteBatch(plant.GraphicsDevice);

            // Navi
            plant.ContentAccess = new ContentManagement(plant.Content);
            plant.View = new Camera(plant.Graphics.PreferredBackBufferHeight);
            plant.InterfaceHeap = new SurfaceManager();

            LoadFundamentals(plant);
            this.plant = plant;
        }

        private void InitStatics(Plant plant)
        {
            Settings.Initialize(ProgramParameterFile);
            Paths.ChangeRootPath(Settings.Modification);
            if (Settings.Music) Music.Initialize();
        }

        private void LoadGraphicsPreferences(GraphicsDeviceManager graphics, Game game, out Vector2 resolution)
        {
            graphics.PreferredBackBufferWidth = Settings.PixelScreenWidth;
            graphics.PreferredBackBufferHeight = Settings.PixelScreenHeight;
            resolution = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            graphics.IsFullScreen = Settings.FullScreen;
            graphics.ApplyChanges();
            graphics.PreferMultiSampling = Settings.Antialiasing;

            bool frameLimit = Settings.FrameLimit;

            graphics.SynchronizeWithVerticalRetrace = frameLimit;
            game.IsFixedTimeStep = frameLimit;
        }

        private void LoadWindowPreferences(Game game)
        {
            game.Window.AllowUserResizing = false;
            game.IsMouseVisible = true;
        }

        private void LoadFundamentals(Plant p)
        {
            string surfaceName;
            if (Settings.SkipLoadingScreen) surfaceName = Settings.StartFile;
            else surfaceName = LoadingSurface;

            Surface loading = new Interface(p.ContentAccess, p.GraphicsDevice, p.Sound, p.InterfaceHeap, resolution, surfaceName, null).Instance();
            loading.Actions.Add(new Tuple<Surface.Action, object>(Load, null));
            p.InterfaceHeap.Push(loading);
        }

        public void Load(object args)
        {
            switch (loadState)
            {
                case 0: CreateInputController();    break;
                case 1: CreateSaveState();          break;
                case 2: StartAudio();               break;
                case 3: LoadMapTools();             break;
                case 4: LoadNextMenu();             break;  // have to be done as last because after it, the current surface is popped
            }

            loadState++;
        }

        private void CreateInputController()
        {
            plant.Input = new InputProcessor();
        }

        private void CreateSaveState()
        {
            plant.Save = new SaveGame<Static, Dynamic, Node>();
        }

        private void LoadMapTools()
        {
            var p = plant;
            p.Map = new MapController();
            p.SceneGround = new Scenery(p.Save, p.ContentAccess, p.GraphicsDevice, p.Sound, p.InterfaceHeap, p.View, resolution, new Vector2(100.0f, 100.0f));
        }

        private void LoadNextMenu()
        {
            if (!Settings.SkipLoadingScreen)
            {
                plant.InterfaceHeap.Pop();  // to pop the Loading surface
                plant.InterfaceHeap.Push(plant.SceneGround);
                string surfaceName = Settings.StartFile;
                var p = plant;
                plant.InterfaceHeap.Push(new Interface(p.ContentAccess, p.GraphicsDevice, p.Sound, p.InterfaceHeap, resolution, surfaceName, null).Instance());
            }
        }

        private void StartAudio()
        {
            if (Settings.Music)
            {
                plant.Music = new MusicPlayer();
                MusicData musicFile = new MusicData(Music.MainMenu[0], Music.Types.MainMenu, Settings.PercentMusicVolume);
                plant.Music.Init();
                plant.Music.Play(musicFile);
            }

            if (Settings.Sound)
            {
                plant.Sound = new SoundPlayer();
                plant.Sound.Init();
            }
        }
        #endregion
    }
}
