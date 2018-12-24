using Navi.Defaults;
using Navi.Helper.Structures;
using Navi.System.State;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

using CsharpDirectory = System.IO.Directory;
using CsharpFile = System.IO.File;
using NaviDirectory = Navi.Helper.System.Directory;

namespace Navi.Models
{
    /// <summary>
    /// To create models of the directories which do contain models.
    /// </summary>
    public sealed class ModelPackage
    {
        private string name;

        private ModelsLoader modelsLoader;
        private Dictionary<string, Model> files;

        /// <param name="directoryPath">Path to some model package. Probably a zip-file.</param>
        /// <param name="modelsLoader">To load models.</param>
        public ModelPackage(string directoryPath, ModelsLoader modelsLoader)
        {
            this.modelsLoader = modelsLoader;
            name = FolderName(directoryPath);
            files = new Dictionary<string, Model>();

            ExtractModels(directoryPath);
            LoadModels(Paths.Temp + name);
        }

        public int Count 
        { 
            get 
            { 
                return files.Count;  
            } 
        }

        private string FolderName(string directoryPath)
        {
            string[] paths = directoryPath.Split(Paths.DirectorySymbol[0]);
            return paths[paths.Length - 1];
        }

        private void ExtractModels(string directoryPath)
        {
            NaviDirectory directory = new NaviDirectory();
            List<string> zipArchiveNames = directory.FileNames(FileExtensions.Model, directoryPath);

            foreach (string zipArchiveName in zipArchiveNames)
            {
                string sourceFilePath = directoryPath + Paths.DirectorySymbol + zipArchiveName + FileExtensions.Model;
                string destinationFolderPath = Paths.Temp + name + Paths.DirectorySymbol + zipArchiveName + Paths.DirectorySymbol;
                ExtractFiles(sourceFilePath, destinationFolderPath, !Settings.OverwriteExistingTempFiles);
            }
        }

        /// <remarks>
        /// A Solid State Drive friendly function. When onlyNewOnes is true, you do not overwrite files which already exist.
        /// </remarks>
        private void ExtractFiles(string sourceFilePath, string destinationFolderPath, bool onlyNewOnes)
        {
            ZipArchive archive = ZipFile.OpenRead(sourceFilePath);
            DirectoryInfo dir = CsharpDirectory.CreateDirectory(destinationFolderPath);

            if (onlyNewOnes)
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    string entryPath = destinationFolderPath + entry.FullName;
                    if (!CsharpFile.Exists(entryPath))
                        entry.ExtractToFile(entryPath, false);
                }
            }
            else
                foreach (ZipArchiveEntry entry in archive.Entries)
                    entry.ExtractToFile(destinationFolderPath + entry.FullName, true);
        }

        // now in the temporary rubric file folder with the extracted zip file folders
        private void LoadModels(string directoryPath)
        {
            NaviDirectory directory = new NaviDirectory();
            List<string> folderNames = directory.DirectoryNames(directoryPath);

            // iterating through each extracted archive
            foreach (string folderName in folderNames)
            {
                string modelDataPath = directoryPath + Paths.DirectorySymbol + folderName;
                Model model = CreateModel(modelDataPath);
                files.Add(folderName, model);
            }
        }

        private Model CreateModel(string modelDataPath)
        {
            NaviDirectory directory = new NaviDirectory();
            List<string> imageNames = directory.FileNames(FileExtensions.Binaries, modelDataPath);
            List<string> soundNames = directory.FileNames(FileExtensions.Sounds, modelDataPath);

            return modelsLoader.LoadModelData(directory, imageNames, soundNames, modelDataPath);
        }

        public Model File(string modelName)
        {
            return files.GetValue(modelName);
        }
    }
}
