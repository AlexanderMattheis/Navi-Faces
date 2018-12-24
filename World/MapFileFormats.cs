namespace Navi.World
{
    /// <summary>
    /// Stores informations about map file formats.
    /// </summary>
    public struct MapFileFormats
    {
        public enum Names
        {
            Benchmark,
            Default
        }

        public static Names FileFormat(string[] mapData)
        {
            if (mapData.Length > 0)
            {
                string[] split = mapData[0].Split(' ');

                if (split.Length == 2) return Names.Benchmark;
            }

            return Names.Default;
        }
    }
}
