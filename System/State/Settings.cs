using Microsoft.Xna.Framework;
using Navi.Helper.System;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Navi.System.State
{
    /// <summary>
    /// To load the program settings from a file.
    /// </summary>
    public static class Settings
    {
        #region audio
        public static bool Music { get; set; }

        public static int PercentMusicVolume { get; set; }

        public static bool Sound { get; set; }

        public static int PercentSoundVolume { get; set; }

        #endregion

        #region developer
        public static bool DebugMode { get; set; }

        public static string StartFile { get; set; }
        #endregion

        #region general
        public static string Language { get; set; }

        public static string Modification { get; set; }

        public static float CameraSpeed { get; set; }
        #endregion

        #region graphics
        public static bool Antialiasing { get; set; }

        public static Color ClearColor { get; set; }

        public static bool FrameLimit { get; set; }

        public static bool FullScreen { get; set; }

        public static int PixelScreenHeight { get; set; }

        public static int PixelScreenWidth { get; set; }
        #endregion

        #region specific
        public static bool SkipLoadingScreen { get; set; }

        public static bool OverwriteExistingTempFiles { get; set; }

        public static uint MsTimespanForViewUpdate { get; set; }
        #endregion

        #region [loader]
        public static void Initialize(string path)
        {
            if (!ReadConfig(path))
                LoadDefaultConfig();
        }

        private static bool ReadConfig(string path)
        {
            FileLoader fileLoader = new FileLoader();
            bool success = fileLoader.Load(path);

            if (success)
                success = LoadConfig(fileLoader.DataReader);

            fileLoader.Close();

            return success;
        }

        private static bool LoadConfig(StreamReader reader)
        {
            bool success = true;

            try
            {
                string[] strings;

                // general
                strings = reader.ReadLine().Split(' ');
                Language = Convert.ToString(strings[1]);

                strings = reader.ReadLine().Split(' ');
                Modification = Convert.ToString(strings[1]);

                reader.ReadLine(); // skips empty line

                // graphics
                strings = reader.ReadLine().Split(' ');
                PixelScreenWidth = Convert.ToInt32(strings[1]);

                strings = reader.ReadLine().Split(' ');
                PixelScreenHeight = Convert.ToInt32(strings[1]);

                strings = reader.ReadLine().Split(' ');
                FullScreen = Convert.ToBoolean(strings[1]);

                strings = reader.ReadLine().Split(' ');
                Antialiasing = Convert.ToBoolean(strings[1]);

                strings = reader.ReadLine().Split(' ');
                FrameLimit = Convert.ToBoolean(strings[1]);

                strings = reader.ReadLine().Split(' ');
                CameraSpeed = float.Parse(strings[1], CultureInfo.InvariantCulture.NumberFormat);

                strings = Regex.Split(reader.ReadLine(), @"(?=[\(])|(?=\s[A-Z])");
                ClearColor = Color(strings[1].Trim());

                reader.ReadLine(); // skips empty line

                // audio
                strings = reader.ReadLine().Split(' ');
                Music = Convert.ToBoolean(strings[1]);

                strings = reader.ReadLine().Split(' ');
                Sound = Convert.ToBoolean(strings[1]);

                strings = reader.ReadLine().Split(' ');
                PercentMusicVolume = Convert.ToInt32(strings[1]);

                strings = reader.ReadLine().Split(' ');
                PercentSoundVolume = Convert.ToInt32(strings[1]);

                reader.ReadLine();  // skips empty line

                // developer
                strings = reader.ReadLine().Split(' ');
                DebugMode = Convert.ToBoolean(strings[1]);

                strings = reader.ReadLine().Split(' ');
                StartFile = Convert.ToString(strings[1]);

                reader.ReadLine();  // skips empty line

                // specific
                strings = reader.ReadLine().Split(' ');
                SkipLoadingScreen = Convert.ToBoolean(strings[1]);

                strings = reader.ReadLine().Split(' ');
                OverwriteExistingTempFiles = Convert.ToBoolean(strings[1]);

                strings = reader.ReadLine().Split(' ');
                MsTimespanForViewUpdate = Convert.ToUInt32(strings[1]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                success = false;
            }

            return success;
        }

        private static Color Color(string data)
        {
            bool isVector = data.Contains("(");

            if (isVector)
            {
                List<int> values = (from value in data.Split('(', ')', ',') where value.Trim() != string.Empty select Convert.ToInt32(value)).ToList();

                if (values.Count == 4)
                    return new Color(values[0], values[1], values[2], values[3]);
                return new Color(values[0], values[1], values[2]);
            }

            // else if text value
            PropertyInfo info = typeof(Color).GetProperty(data);
            if (info != null)
                return (Color)info.GetValue(null);

            return default(Color);
        }

        private static void LoadDefaultConfig()
        {
            // general
            Language = "en";
            Modification = "Default";

            // graphics
            PixelScreenWidth = 1024;
            PixelScreenHeight = 576;
            FullScreen = false;
            Antialiasing = false;
            FrameLimit = true;
            CameraSpeed = 1.0f;
            ClearColor = new Color(164, 164, 164);

            // audio
            Music = false;
            Sound = false;
            PercentMusicVolume = 100;
            PercentSoundVolume = 100;

            // developer
            StartFile = "MainMenu";
            DebugMode = false;

            // specific
            SkipLoadingScreen = true;
            OverwriteExistingTempFiles = false;
            MsTimespanForViewUpdate = 40;
        }
        #endregion
    }
}
