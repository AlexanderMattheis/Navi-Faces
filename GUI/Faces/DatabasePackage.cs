using Navi.GUI.Faces.Databases;
using Navi.System.State;

using FacesDataBases = Navi.GUI.Faces.Databases;
using MediaDatabases = Navi.Defaults.Databases;

namespace Navi.GUI.Faces
{
    /// <summary>
    /// Reads in databases which are needed to create Interfaces.
    /// To allow reading in all databases only once.
    /// </summary>
    public class DatabasePackage
    {
        public DatabasePackage()
        {
            AlignmentBase = new Design(MediaDatabases.Alignment);
            ColorBase = new Design(MediaDatabases.Colors);
            EffectBase = new Design(MediaDatabases.Effects);
            FontBase = new Design(MediaDatabases.Fonts);
            ImageBase = new Design(MediaDatabases.Images);
            PivotBase = new Design(MediaDatabases.Pivots);
            RotationBase = new Design(MediaDatabases.Rotations);
            ScaleBase = new Design(MediaDatabases.Scales);
            SoundBase = new Design(MediaDatabases.Sounds);
            StateBase = new Design(MediaDatabases.States);

            LanguageBase = new FacesDataBases.Language(Settings.Language);
        }

        public Design AlignmentBase { get; set; }

        public Design ColorBase { get; set; }

        public Design EffectBase { get; set; }

        public Design FontBase { get; set; }

        public Design ImageBase { get; set; }

        public Design PivotBase { get; set; }

        public Design RotationBase { get; set; }

        public Design ScaleBase { get; set; }

        public Design SoundBase { get; set; }

        public Design StateBase { get; set; }

        public FacesDataBases.Language LanguageBase { get; set; }
    }
}
