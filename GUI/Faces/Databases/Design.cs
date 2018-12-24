using Navi.Defaults;
using Navi.Programming;

namespace Navi.GUI.Faces.Databases
{
    public sealed class Design : Database
    {
        public Design(string database)
        {
            Set(database);
        }

        private void Set(string database)
        {
            string path = Paths.Databases + database + FileExtensions.Database;
            
            if (Paths.Root != Paths.DefaultRoot)  // to load the default color files even if a modification is set
            {
                string currentRoot = Paths.Root;

                Paths.ChangeRootPath(Paths.DefaultRoot);
                {
                    string defaultColorPath = Paths.Databases + database + FileExtensions.Database;
                    CreateDatebase(new string[] { defaultColorPath, path });
                }

                Paths.ChangeRootPath(currentRoot);
            }
            else
                CreateDatebase(new string[] { path });
        }
    }
}
