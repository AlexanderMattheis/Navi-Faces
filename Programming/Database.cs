using Navi.Defaults;
using Navi.Helper.Structures;
using System.Collections.Generic;

namespace Navi.Programming
{
    public class Database
    {
        protected Dictionary<string, string> database;

        public Database()
        {
            database = new Dictionary<string, string>();
        }

        public void CreateDatebase(string[] databasePaths)
        {
            ScriptReader reader = new ScriptReader();
            string comment = ScriptSymbols.Comment;
            char escape = ScriptSymbols.Escape[0];
            char delimeter = ScriptSymbols.Delimeter[0];
            char lineTerminal = ScriptSymbols.Rubric[0];

            foreach (string path in databasePaths)
            {
                List<string> translations = reader.Commands(path, comment, escape, delimeter, lineTerminal);
                AddToDatabase(translations);
            }
        }

        private void AddToDatabase(List<string> values)
        {
            foreach (string data in values)
            {
                string[] keyAndValue = data.Split(ScriptSymbols.Equal[0]);
                string key = keyAndValue[0].Trim();
                string value = keyAndValue[1].Trim();
                ReplaceOrAdd(key, value);
            }
        }

        private void ReplaceOrAdd(string key, string value)
        {
            if (database.ContainsKey(key)) database.Remove(key);
            database.Add(key, value);
        }

        public string TextValue(string id)
        {
            return database.GetValue(id);
        }
    }
}
