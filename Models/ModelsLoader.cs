using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Navi.Audio.Player;
using Navi.Defaults;
using Navi.Helper.Structures;
using Navi.Helper.System;
using Navi.System.State;
using Navi.World;
using System.Collections.Generic;

namespace Navi.Models
{
    /// <summary>
    /// To interprete the files contained with a model.
    /// </summary>
    public sealed class ModelsLoader
    {
        private const char PackageAndNameSeperator = '_';

        private ContentManager content;
        private GraphicsDevice graphics;
        private SoundPlayer soundPlayer;

        private ModelScriptInterpreter interpreter;
        private Dictionary<string, ModelPackage> modelPackages;

        public ModelsLoader(ContentManager content, GraphicsDevice graphics, SoundPlayer soundPlayer)
        {
            interpreter = new ModelScriptInterpreter();
            this.content = content;
            this.graphics = graphics;
            this.soundPlayer = soundPlayer;

            CreatePackages(content, graphics, soundPlayer);
        }

        public int NumberOfModels
        {
            get
            {
                int number = 0;

                foreach (KeyValuePair<string, ModelPackage> pair in modelPackages)
                    number += pair.Value.Count;

                return number;
            }
        }

        private void CreatePackages(ContentManager content, GraphicsDevice graphics, SoundPlayer soundPlayer)
        {
            Directory directory = new Directory();
            modelPackages = new Dictionary<string, ModelPackage>();

            List<string> folderNames = directory.DirectoryNames(Paths.Models);

            foreach (string folderName in folderNames)
            {
                string folderPath = Paths.Models + folderName;
                ModelPackage package = new ModelPackage(folderPath, this);
                modelPackages.Add(folderName, package);
            }
        }

        public Model LoadModelData(Directory directory, List<string> imageNames, List<string> soundNames, string modelPath)
        {
            Model model = interpreter.Create(ModelDescriptionPath(directory, modelPath));

            LoadImages(content, model, modelPath, imageNames);
            LoadSounds(model, modelPath, soundNames);

            string imagePath = ToXNAPath(modelPath + Paths.DirectorySymbol + imageNames[0]);
            model.Init(content, graphics, soundPlayer, imagePath);

            return model;
        }

        private string ModelDescriptionPath(Directory directory, string modelDataPath)
        {
            List<string> modelDescriptions = directory.FileNames(FileExtensions.ModelDescription, modelDataPath);
            string modelDescriptionName = modelDescriptions.Count > 0 ? modelDescriptions[0] : null;

            if (modelDescriptionName != null)
                return modelDataPath + Paths.DirectorySymbol + modelDescriptionName + FileExtensions.ModelDescription;
            else
                return null;
        }

        public string ToXNAPath(string ordinaryPath)
        {
            string convertedPath = ordinaryPath.Replace(Paths.ContentFolder, string.Empty);
            return convertedPath.Replace(Paths.DirectorySymbol, Paths.XnaDirectorySymbols);
        }

        private void LoadImages(ContentManager content, Model model, string modelPath, List<string> imageNames)
        {
            foreach (string imageName in imageNames)
            {
                string[] nameSplit = imageName.Split(PackageAndNameSeperator);
                string imageType = nameSplit.Length > 1 ? nameSplit[1] : null;
                string imagePath = ToXNAPath(modelPath + Paths.DirectorySymbol + imageName);

                switch (imageType)
                {
                    case ModelStates.Angry:
                        Dynamic dynamic = model as Dynamic;
                        dynamic.Angry = content.Load<Texture2D>(imagePath);                         break;
                    case ModelStates.Selected: model.Selected = content.Load<Texture2D>(imagePath); break;
                }
            }
        }

        private void LoadSounds(Model model, string modelPath, List<string> soundNames)
        {
        }

        public Model CreateModel(string packageName, string name, int posX, int posY)
        {
            ModelPackage package = modelPackages.GetValue(packageName);
            Model model = package.File(name);
            if (model is Static) model = new Static(model);
            if (model is Dynamic) model = new Dynamic(model);
            SetModelPosition(model, posX, posY);
            return model;
        }

        private void SetModelPosition(Model model, int posX, int posY)
        {
            float modelDefaultSize = Map.PercentCellDimension * Settings.PixelScreenHeight;

            model.PixelPosX = modelDefaultSize * posX * Map.PercentGapDistance;
            model.PixelPosY = modelDefaultSize * posY * Map.PercentGapDistance;
            model.PixelPosition = new Vector2((float)model.PixelPosX, (float)model.PixelPosY);
            model.PixelWidth = modelDefaultSize;
            model.PixelHeight = modelDefaultSize;
            model.Tint = Color.White;
        }
    }
}
