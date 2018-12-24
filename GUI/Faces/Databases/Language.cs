using Navi.Defaults;
using Navi.Programming;

namespace Navi.GUI.Faces.Databases
{
    public sealed class Language : Database
    {
        private const string EnglishAcronym = "en";

        public Language(string language)
        {
            SetVocabulary(language);
        }

        private void SetVocabulary(string language)
        {
            string languagePath = Paths.Languages + language + FileExtensions.Database;
            string englishPath = Paths.Languages + EnglishAcronym + FileExtensions.Database;

            if (Paths.Root != Paths.DefaultRoot)  // to load the default language files even if a modification is set
            {
                string currentRoot = Paths.Root;
                
                Paths.ChangeRootPath(Paths.DefaultRoot);
                { 
                    string defaultLanguagePath = Paths.Languages + language + FileExtensions.Database;
                    string defaultEnglishPath = Paths.Languages + EnglishAcronym + FileExtensions.Database;
                    CreateDatebase(new string[] { defaultEnglishPath, defaultLanguagePath, englishPath, languagePath });
                }

                Paths.ChangeRootPath(currentRoot);
            }
            else
                CreateDatebase(new string[] { englishPath, languagePath });
        }
    }
}
