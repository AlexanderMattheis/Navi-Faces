namespace Navi.Defaults
{
    public sealed class Paths
    {
        public const string ContentFolder = "./Content/";
        public const string XnaDirectorySymbols = "\\";
        public const string DirectorySymbol = "/";
        public const string DefaultRoot = "Default";
        public const string FacesNamesSpace = "Navi.Content.Faces.";
        public const string Namespace = ".";

        static Paths()
        {
            Root = DefaultRoot;
        }

        #region content
        public static string Databases { get; private set; }

        public static string DefaultMap { get; private set; }

        public static string Fonts { get; private set; }

        public static string Images { get; private set; }

        public static string Languages { get; private set; }

        public static string Maps { get; private set; }

        public static string Models { get; private set; }

        public static string Music { get; private set; }

        public static string MusicDatabase { get; private set; }

        public static string Sounds { get; private set; }

        public static string Surfaces { get; private set; }

        public static string Temp { get; private set; }
        #endregion

        public static string Root { get; private set; }

        public static void ChangeRootPath(string modification)
        {
            Fonts = modification + "\\Fonts\\";
            Images = modification + "\\Images\\";

            Databases = ContentFolder + modification + "/Databases/";
            Languages = ContentFolder + modification + "/Databases/Languages/";
            Maps = ContentFolder + modification + "/Maps/";
            DefaultMap = Maps + "Default.map";
            Models = ContentFolder + modification + "/Models/";
            Music = ContentFolder + modification + "/Audio/Music/";
            MusicDatabase = Databases + "System/music";
            Sounds = ContentFolder + modification + "/Audio/Sounds/";
            Surfaces = ContentFolder + modification + "/Faces/";
            Temp = ContentFolder + modification + "/Temp/";

            Root = modification;
        }
    }
}
