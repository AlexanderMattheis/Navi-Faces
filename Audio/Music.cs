using Navi.Defaults;
using Navi.Programming;
using System.Collections.Generic;
using System.Linq;

namespace Navi.Audio
{
    /// <summary>
    /// Stores the list of available songs.
    /// </summary>
    public struct Music
    {
        public const string TitleSplit = "+";

        public static List<string> MainMenu { get; private set; }

        public static List<string> Ingame { get; private set; }

        public static void Initialize()
        {
            InitTypes();
            SetTitlesFromDatabase(MusicDatabase());
        }

        private static void InitTypes()
        {
            MainMenu = new List<string>();
            Ingame = new List<string>();
        }

        private static Database MusicDatabase()
        {
            Database musicBase = new Database();
            string musicDataBasePath = Paths.MusicDatabase + FileExtensions.Database;
            musicBase.CreateDatebase(new string[] { musicDataBasePath });
            return musicBase;
        }

        private static void SetTitlesFromDatabase(Database musicBase)
        {
            string[] titles = (from title in musicBase.TextValue(Types.MainMenu).Split(TitleSplit[0]) select title.Trim()).ToArray();
            foreach (string title in titles) MainMenu.Add(Paths.Music + title);

            titles = (from title in musicBase.TextValue(Types.Ingame).Split(TitleSplit[0]) select title.Trim()).ToArray();
            foreach (string title in titles) Ingame.Add(Paths.Music + title);
        }

        public struct Types
        {
            public const string MainMenu = "MainMenu";
            public const string Ingame = "Ingame";
        }
    }
}
