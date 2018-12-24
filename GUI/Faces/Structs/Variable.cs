using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Pattern = Navi.GUI.Faces.Language.Patterns;
using Symbols = Navi.GUI.Faces.Language.Vocabulary.Symbols;

namespace Navi.GUI.Faces.Structs
{
    public struct Variable
    {
        public string ID { get; set; }

        public string Type { get; set; }

        public string TypeID { get; set; }

        public string Value { get; set; }

        public Variable GetData(List<string> commands, int i)
        {
            bool isVariableInitialization = i < commands.Count ? Regex.IsMatch(commands[i].Trim(), Pattern.VariableInitialization) : false;
            Type = string.Empty;
            ID = string.Empty;
            Value = string.Empty;

            if (isVariableInitialization)
            {
                string[] segments = commands[i].Split(Symbols.Equal[0]);  // "="
                string[] leftPart = segments[0].Split(Symbols.LeftAngleBracket[0], Symbols.RightAngleBracket[0]); // "<", ">"

                bool isArray = leftPart[0].Contains(Symbols.LeftCurlyBracket);

                if (isArray)
                {
                    string curlyBrackets =  Symbols.LeftCurlyBracket + Symbols.RightCurlyBracket;
                    Type = leftPart[0].Replace(Symbols.LeftCurlyBracket, string.Empty).Replace(Symbols.RightCurlyBracket, string.Empty).Trim() + curlyBrackets;
                }
                else
                    Type = leftPart[0].Trim();

                // if there is something like "text{}<0> = #fx=Tasks" then we need the right part after the "+"-symbol
                Value = segments[1].Trim() + (segments.Length == 3 ? Symbols.Equal[0] + segments[2] : string.Empty);

                if (leftPart.Length > 1)  // example:  image<0> = Logos.navi_logo_white
                    ID = leftPart[1].Trim();

                TypeID = Type + ID;
            }

            return this;
        }

        public Variable GetData(string parameter)
        {
            Type = string.Empty;
            ID = string.Empty;
            Value = string.Empty;

            string[] variableSplit = (from value in parameter.Split(Symbols.Equal[0]) select value.Trim()).ToArray();
            Value = variableSplit[1] + (variableSplit.Length == 3 ? Symbols.Equal + variableSplit[2] : string.Empty);

            variableSplit = variableSplit[0].Split(Symbols.LeftAngleBracket[0], Symbols.RightAngleBracket[0]);
            Type = variableSplit[0];

            if (variableSplit.Length > 1)
                ID = variableSplit[1];

            TypeID = Type + ID;

            return this;
        }

        public Variable GetData(Structs.Object objectData, int parameterPos)
        {
            string[] parameters = objectData.Parameters[parameterPos].Split(Symbols.LeftAngleBracket[0], Symbols.RightAngleBracket[0]);  // "<", ">"
            ID = string.Empty;

            Type = parameters[0].Trim();
            if (parameters.Length > 1)  // example: image<0>
                ID = parameters[1];

            TypeID = Type + ID;

            return this;
        }
    }
}
