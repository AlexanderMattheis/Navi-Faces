using Microsoft.Xna.Framework.Content;

namespace Navi.System
{
    /// <summary>
    /// Creates different ContentManagers for different parts of the memory.
    /// This way it is possible to delete specific content.
    /// </summary>
    /// <example>
    /// Allows to delete map data to load a new map without deleting the GUI data.
    /// </example>
    /// <remarks>
    /// Is based on the ContentHandler class of Navi (2014/15).
    /// </remarks>
    public sealed class ContentManagement
    {
        public ContentManagement(ContentManager content)
        {
            Menu = new ContentManager(content.ServiceProvider, content.RootDirectory);
            Map = new ContentManager(content.ServiceProvider, content.RootDirectory);
        }

        /// <summary>
        /// Stores static menu textures, sounds and music.
        /// </summary>
        public ContentManager Menu { get; private set; }

        /// <summary>
        /// Stores only map textures, sounds and map music.
        /// Should be deleted everytimes a new map is loaded.
        /// </summary>
        public ContentManager Map { get; private set; }
    }
}
