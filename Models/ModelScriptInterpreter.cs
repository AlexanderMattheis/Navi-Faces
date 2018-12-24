using Navi.Defaults;
using Navi.Programming;
using System;
using System.Collections.Generic;

using Symbols = Navi.Models.Script.Vocabulary.Symbols;
using Values = Navi.Models.Script.VariableValues;
using Variables = Navi.Models.Script.Vocabulary.Variables;

namespace Navi.Models
{
    public class ModelScriptInterpreter
    {
        public Model Create(string scriptPath)
        {
            if (scriptPath != null)
            {
                ScriptReader reader = new ScriptReader();

                string comment = ScriptSymbols.Comment;
                char variableAssignment = ScriptSymbols.Rubric[0];

                List<string> scriptCommands = reader.Commands(scriptPath, comment, variableAssignment);
                Model model = Interpret(scriptCommands, variableAssignment);

                return model;
            }

            return new Static();
        }

        private Model Interpret(List<string> scriptCommands, char variableSeperation)
        {
            Model model = new Model();

            foreach (string command in scriptCommands)
            {
                string[] variableSplit = command.Split(variableSeperation);
                string variableName = variableSplit[0].Trim();
                string values = variableSplit[1].Trim();

                string[] valuesSplit = values.Split(Symbols.Comma[0]);

                switch (variableName) 
                {
                    case Variables.Friction:    SetFriction(model, valuesSplit);    break;
                    case Variables.Size:        SetSize(model, valuesSplit);        break;
                    case Variables.SubType:     break;
                    case Variables.Type:        SetType(ref model, valuesSplit);    break;
                    case Variables.UpdateTime:  break;
                }
            }

            return model;
        }

        private void SetFriction(Model model, string[] values)
        {
            int value = int.Parse(values[0]);

            if (value == 100) model.IsBlocked = true;
        }

        private void SetSize(Model model, string[] values)
        {
            model.PercentScale = float.Parse(values[0]);
        }

        private void SetType(ref Model model, string[] values)
        {
            string value = values[0];

            switch (value)
            {
                case Values.Type.Static:    model = new Static();  break;
                case Values.Type.Dynamic:   model = new Dynamic(); break;
            }
        }
    }
}
