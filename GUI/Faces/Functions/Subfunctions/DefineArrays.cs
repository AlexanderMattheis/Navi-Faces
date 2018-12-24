using Navi.GUI.Faces.Parser;
using Navi.GUI.Faces.Structs;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Pattern = Navi.GUI.Faces.Language.Patterns;
using Symbols = Navi.GUI.Faces.Language.Vocabulary.Symbols;
using Variables = Navi.GUI.Faces.Language.Vocabulary.Variables;

namespace Navi.GUI.Faces.Functions.Subfunctions
{
    public struct DefineArrays
    {
        private Compiler compiler;

        public DefineArrays(Compiler compiler)
        {
            this.compiler = compiler;
        }

        public void SetData(Variable variableData)
        {
            switch (variableData.Type)
            {
                case Variables.Text + Symbols.GroupBrackets:    StoreText(variableData);    break;
            }
        }

        private void StoreText(Variable variableData)
        {
            bool text = Regex.IsMatch(variableData.Value, Pattern.Variables.Text);

            if (text)
                compiler.TextArrays.Add(variableData.TypeID, new List<string>() { variableData.Value });

            compiler.Definition.CompilerMessage(variableData, text);
        }
    }
}
