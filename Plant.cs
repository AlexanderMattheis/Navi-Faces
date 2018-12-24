/*
 * The zlib/libpng License
 * 
 * Copyright(c) 2015 Alexander Mattheis <mattheia@informatik.uni-freiburg.de>
 * 
 * This software is provided 'as-is', without any express or implied warranty.
 * In no event will the authors be held liable
 * for any damages arising from the use of this software.
 * 
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it
 * and redistribute it freely, subject to the following restrictions:
 * 
 * 1.   The origin of this software must not be misrepresented;
 *      you must not claim that you wrote the original software.
 *      If you use this software in a product, an acknowledgement
 *      in the product documentation would be appreciated but is not required.
 * 2.   Altered source versions must be plainly marked as such,
 *      and must not be misrepresented as being the original software.
 * 3.   This notice may not be removed or altered from any source distribution.
 * 
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Navi.AI.Arrangement;
using Navi.Audio.Player;
using Navi.World;
using Navi.Models;
using Navi.Input;
using Navi.Scene;
using Navi.Scene.Tools;
using Navi.Screen;
using Navi.System;
using Navi.System.State;
using System;
using CsharpSystem = System;

namespace Navi
{
    /// <summary>
    /// Main class that declares objects.
    /// </summary>
    public sealed class Plant : Game
    {
        private const string ExitMessage = "Goodbye!";
        public static ProgramState State;

        public Plant() : base()
        {
            // This initiliziation can't be done in the ProgramLoader because otherwise no GraphicsDevice will be created in between
            // the constructor- and the Initialize method-call.
            Graphics = new GraphicsDeviceManager(this);
        }

        public enum ProgramState
        {
            Editor,
            Game
        }

        #region monogame
        public SpriteBatch Drawer { get; set; }

        public GraphicsDeviceManager Graphics { get; set; }
        #endregion

        #region navi
        public ContentManagement ContentAccess { get; set; }

        public SurfaceManager InterfaceHeap { get; set; }

        public InputProcessor Input { get; set; }

        public MapController Map { get; set; }

        public MusicPlayer Music { get; set; }

        public SaveGame<Static, Dynamic, Node> Save { get; set; }

        public SoundPlayer Sound { get; set; }

        public Scenery SceneGround { get; set; }

        public Camera View { get; set; }
        #endregion

        #region [loader]
        /// <summary>
        /// Does all the initialization stuff that is needed to draw Interface's.
        /// </summary>
        protected override void Initialize()
        {
            (new ProgramLoader()).Load(this);
        }
        #endregion

        #region [updater]
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Settings.ClearColor);

            InterfaceHeap.Draw(Drawer, View);

            base.Draw(gameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            double frameTime = gameTime.ElapsedGameTime.TotalMilliseconds;

            InterfaceHeap.Update(frameTime);
            Input.Process(InterfaceHeap, View, frameTime);

            base.Update(gameTime);
        }
        #endregion

        #region [leaver]
        protected override void OnExiting(object sender, EventArgs args)
        {
            base.OnExiting(sender, args);

            Console.WriteLine(ExitMessage);
            DisposeAudio();
            CsharpSystem.Environment.Exit(1);  // to close the console too
        }

        private void DisposeAudio()
        {
            if (Music != null) Music.Dispose();
            if (Sound != null) Sound.Dispose();
        }
        #endregion
    }
}
