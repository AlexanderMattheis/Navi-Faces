using Navi.GUI.Faces.Parser;
using Navi.GUI.Faces.Structs;
using Navi.Helper.Structures;
using System.Collections.Generic;

using ReturnValues = Navi.GUI.Faces.Functions.Create.ReturnValues;
using Symbols = Navi.GUI.Faces.Language.Vocabulary.Symbols;
using Variables = Navi.GUI.Faces.Language.Vocabulary.Variables;

namespace Navi.GUI.Faces.Functions.Subfunctions
{
    public struct CreateObjectGroups
    {
        private int biggest;
        private Compiler compiler;

        public CreateObjectGroups(Compiler compiler)
        {
            biggest = int.MinValue;
            this.compiler = compiler;
        }

        public int Biggest
        {
            get { return biggest; }
        }

        public ReturnValues SetObjectParameters(Structs.Object objectData, string objectName, Create.VariableData data)
        {
            List<string> textList = new List<string>();
            ReturnValues doneCreation = ReturnValues.NoErrors;

            for (int i = 0; i < objectData.Parameters.Length; i++)
            {
                Variable variableInfo = compiler.VariableAnalyzer.GetData(objectData, i);

                if (!variableInfo.Type.Contains(Symbols.GroupBrackets))
                {
                    doneCreation = ReturnValues.ContainsVariables;
                    continue;
                }
                
                switch (variableInfo.Type)
                {
                    case Variables.Text + Symbols.GroupBrackets:    SetTextList(textList, variableInfo, data);  break;  // 15
                    default:                                        doneCreation = ReturnValues.Errors;         break;
                }
            }

            return doneCreation;
        }

        private void SetTextList(List<string> textList, Variable variableInfo, Create.VariableData data)
        {
            textList = compiler.TextArrays.GetValue(variableInfo.TypeID);

            if (textList.Count == 1)  // if something like "#input=Tasks"
            {
                string value = textList[0].Trim();
                data.TextGroup = (List<string>)compiler.Interpreter.Interpret(ref value, null, null, null, compiler.Face, null);
                SetMaximum(data.TextGroup.Count);
            }
        }

        private void SetMaximum(int size)
        {
            if (biggest < size)
                biggest = size;
        }

        public struct Groups
        {
            public const string TextGroup = "TextGroup";
        }
    }
}
