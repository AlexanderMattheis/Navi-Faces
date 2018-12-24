using Microsoft.Xna.Framework;
using Navi.GUI.Faces.Parser;
using Navi.GUI.Faces.Structs;
using Navi.Helper.Structures;

using ReturnValues = Navi.GUI.Faces.Functions.Create.ReturnValues;
using State = Navi.GUI.Faces.Datatypes.State;
using Symbols = Navi.GUI.Faces.Language.Vocabulary.Symbols;
using Variables = Navi.GUI.Faces.Language.Vocabulary.Variables;

namespace Navi.GUI.Faces.Functions.Subfunctions
{
    public struct CreateObjects
    {
        private Compiler compiler;

        public CreateObjects(Compiler compiler)
        {
            this.compiler = compiler;
        }

        public ReturnValues SetObjectParameters(Structs.Object objectData, string objectName, Create.VariableData data)
        {
            string text = string.Empty;
            Color color = Color.Transparent;
            ReturnValues doneCreation = ReturnValues.NoErrors;

            for (int i = 0; i < objectData.Parameters.Length; i++)
            {
                Variable variableInfo = compiler.VariableAnalyzer.GetData(objectData, i);

                if (variableInfo.Type.Contains(Symbols.GroupBrackets))
                {
                    doneCreation = ReturnValues.ContainsArrays;
                    continue;
                }

                switch (variableInfo.Type)
                {
                    case Variables.CheckedColor:    data.CheckedColor = compiler.CheckedColorVariables.GetValue(variableInfo.TypeID);       break;  // 1
                    case Variables.Color:           data.Color = compiler.ColorVariables.GetValue(variableInfo.TypeID);
                                                    data.Tint = compiler.ColorVariables.GetValue(variableInfo.TypeID);                      break;  // 2
                    case Variables.Effect:          data.Effect = compiler.EffectVariables.GetValue(variableInfo.TypeID);                   break;  // 3
                    case Variables.Font:            data.FontPath = compiler.FontVariables.GetValue(variableInfo.TypeID);                   break;  // 4
                    case Variables.Highlight:       data.HighlightColor = compiler.HighlightColorVariables.GetValue(variableInfo.TypeID);   break;  // 5
                    case Variables.Icon:            data.IconPath = compiler.IconVariables.GetValue(variableInfo.TypeID);                   break;  // 6
                    case Variables.IconEffect:      data.IconEffect = compiler.IconEffectVariables.GetValue(variableInfo.TypeID);           break;  // 7
                    case Variables.IconPivot:       data.IconPivot = compiler.IconPivotVariables.GetValue(variableInfo.TypeID);             break;  // 8
                    case Variables.IconRotation:    data.IconRotation = compiler.IconRotationVariables.GetValue(variableInfo.TypeID);       break;  // 9
                    case Variables.Image:           data.ImagePath = compiler.ImageVariables.GetValue(variableInfo.TypeID);                 break;  // 10
                    case Variables.Pivot:           data.Pivot = compiler.PivotVariables.GetValue(variableInfo.TypeID);                     break;  // 11
                    case Variables.Rotation:        data.Rotation = compiler.RotationVariables.GetValue(variableInfo.TypeID);               break;  // 12 
                    case Variables.Scale:           data.Scale = compiler.ScaleVariables.GetValue(variableInfo.TypeID);                     break;  // 13
                    case Variables.ShinyImage:      data.ShinyImagePath = compiler.ShinyImageVariables.GetValue(variableInfo.TypeID);       break;  // 14
                    case Variables.Sound:           data.SoundPath = compiler.SoundVariables.GetValue(variableInfo.TypeID);                 break;  // 15
                    case Variables.State:           State state = compiler.StateVariables.GetValue(variableInfo.TypeID);
                                                    data.IsActive = state.IsActive;
                                                    data.IsSet = state.IsSet;
                                                    data.ReceivesMouseActions = state.ReceivesMouseActions;                                 break;  // 16
                    case Variables.StateImage:      data.StateImagePath = compiler.StateImageVariables.GetValue(variableInfo.TypeID);       break;  // 17
                    case Variables.Text:            text = compiler.TextVariables.GetValue(variableInfo.TypeID);
                                                    compiler.Interpreter.Interpret(ref text, objectName, null, null, null, null);
                                                    data.Text = text;                                                                       break;  // 18
                    case Variables.TextAlignment:   data.TextAlignment = compiler.TextAligmentVariables.GetValue(variableInfo.TypeID);      break;  // 19
                    case Variables.TextColor:       data.TextColor = compiler.TextColorVariables.GetValue(variableInfo.TypeID);             break;  // 20
                    case Variables.TextEffect:      data.TextEffect = compiler.TextEffectVariables.GetValue(variableInfo.TypeID);           break;  // 21
                    case Variables.TextPivot:       data.TextPivot = compiler.TextPivotVariables.GetValue(variableInfo.TypeID);             break;  // 22
                    case Variables.TextRotation:    data.TextRotation = compiler.TextRotationVariables.GetValue(variableInfo.TypeID);       break;  // 23
                    case Variables.TextScale:       data.TextScale = compiler.TextScaleVariables.GetValue(variableInfo.TypeID);             break;  // 24
                    default:                        doneCreation = ReturnValues.Errors;                                                     break;
                }
            }

            return doneCreation;
        }
    }
}
