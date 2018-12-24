using Navi.Defaults;

namespace Navi.Models.Script
{
    public struct Vocabulary
    {
        public struct Symbols
        {
            public const string Comma = ",";
            public const string Comment = ScriptSymbols.Comment;
            public const string Delimeter = ScriptSymbols.Delimeter;
            public const string Escape = ScriptSymbols.Escape;
            public const string VariableAssigment = ScriptSymbols.Rubric;
        }

        public struct Variables
        {
            public const string Friction = "friction";
            public const string Type = "type";
            public const string Size = "size";
            public const string SubType = "subtype";
            public const string UpdateTime = "updateTime";
        }
    }
}
