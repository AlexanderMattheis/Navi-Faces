using Navi.GUI.Faces.Functions.Subfunctions;
using Navi.GUI.Faces.Language;
using Navi.GUI.Faces.Parser;
using Navi.GUI.Faces.Structs;
using System.Collections.Generic;

using Symbols = Navi.GUI.Faces.Language.Vocabulary.Symbols;

namespace Navi.GUI.Faces.Functions
{
    public struct Define
    {
        private Compiler compiler;
        private DefineArrays defineArrays;
        private DefineVariables defineVariables;

        public Define(Compiler compiler)
        {
            this.compiler = compiler;
            defineArrays = new DefineArrays(compiler);
            defineVariables = new DefineVariables(compiler);
        }

        public void ReadIn(List<string> commands, ref int line)
        {
            if (line < commands.Count)
            {
                if (commands[line].Trim() == Vocabulary.Functions.Define + Symbols.FunctionMarker)
                {
                    line++;  // skip "Define"-line

                    Variable variableData = compiler.VariableAnalyzer.GetData(commands, line);
                    string variableType = variableData.Type.Replace(Symbols.LeftCurlyBracket + Symbols.RightCurlyBracket, string.Empty);

                    while (Vocabulary.IsVariableType(variableType))
                    {
                        SetData(variableData);

                        line++;
                        variableData = compiler.VariableAnalyzer.GetData(commands, line);

                        variableType = variableData.Type.Replace(Symbols.LeftCurlyBracket + Symbols.RightCurlyBracket, string.Empty);
                    }
                }

                compiler.Debugger.Message(string.Empty);
            }
        }

        public void SetData(Variable variableData)
        {
            if (variableData.Type.Contains(Symbols.LeftCurlyBracket))
                defineArrays.SetData(variableData);
            else
                defineVariables.SetData(variableData);
        }

        public void CompilerMessage(Variable variableData, bool isCorrect)
        {
            compiler.Debugger.Message(variableData.Type + (variableData.ID.Length > 0 ?
                (Symbols.LeftAngleBracket + variableData.ID + Symbols.RightAngleBracket) : string.Empty),
                isCorrect);
        }
    }
}
