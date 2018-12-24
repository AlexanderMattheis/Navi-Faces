using System;
using System.Collections.Generic;
using System.IO;

namespace Navi.Helper.System
{
    /// <remarks>
    /// Some source code is taken from the ContentReader class of Navi (2014/2015).
    /// </remarks> 
    public class Directory
    {
        private const char FileExtensionDot = '.';

        public List<string> FileNames(string fileExtension, string folderPath)
        {
            DirectoryInfo folder = new DirectoryInfo(folderPath);
            List<string> fileNames = new List<string>();

            foreach (FileInfo file in folder.GetFiles())
            {
                string fullName = file.Name;

                if (FileExtension(fullName).Equals(fileExtension))
                    fileNames.Add(RemoveExtension(fullName));
            }

            return fileNames;
        }

        private string FileExtension(string fullName)
        {
            string extension = string.Empty;

            try
            {
                string[] parts = fullName.Split(FileExtensionDot);

                if (parts.Length > 1)
                    extension = parts[parts.Length - 1];
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return FileExtensionDot + extension;
        }

        private string RemoveExtension(string fullName)
        {
            return fullName.Split(FileExtensionDot)[0];
        }

        public List<string> DirectoryNames(string folderPath)
        {
            DirectoryInfo folder = new DirectoryInfo(folderPath);
            List<string> directoryNames = new List<string>();

            foreach (DirectoryInfo directory in folder.GetDirectories())
                directoryNames.Add(directory.Name);

            return directoryNames;
        }
    }
}
