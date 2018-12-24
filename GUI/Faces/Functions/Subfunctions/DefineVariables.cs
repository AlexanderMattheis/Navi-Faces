using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Navi.Defaults;
using Navi.GUI.Faces.Language;
using Navi.GUI.Faces.Parser;
using Navi.GUI.Faces.Structs;
using Navi.GUI.Widgets;
using Navi.Helper.Structures;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

using Classes = Navi.GUI.Faces.Language.Vocabulary.Classes;
using Commands = Navi.GUI.Faces.Language.Vocabulary.Commands;
using Pattern = Navi.GUI.Faces.Language.Patterns;
using Symbols = Navi.GUI.Faces.Language.Vocabulary.Symbols;
using Variables = Navi.GUI.Faces.Language.Vocabulary.Variables;

namespace Navi.GUI.Faces.Functions.Subfunctions
{
    public struct DefineVariables
    {
        private Compiler compiler;

        public DefineVariables(Compiler compiler)
        {
            this.compiler = compiler;
        }

        public void SetData(Variable variableData)
        {
            switch (variableData.Type)
            {
                case Variables.CheckedColor:    StoreCheckedColors(variableData);   break;  // 1
                case Variables.Color:           StoreColors(variableData);          break;  // 2
                case Variables.Effect:          StoreEffects(variableData);         break;  // 3
                case Variables.Font:            StoreFonts(variableData);           break;  // 4
                case Variables.Highlight:       StoreHighlights(variableData);      break;  // 5
                case Variables.Icon:            StoreIcons(variableData);           break;  // 6
                case Variables.IconEffect:      StoreIconEffects(variableData);     break;  // 7
                // case Variables.IconPivot:       StoreIconPivot(variableData);       break;  // 8
                case Variables.IconRotation:    StoreIconRotation(variableData);    break;  // 9
                case Variables.Image:           StoreImages(variableData);          break;  // 10
                case Variables.Pivot:           StorePivots(variableData);          break;  // 11
                // case Variables.Rotation:        StoreRotation(variableData);        break;  // 12
                case Variables.Scale:           StoreScale(variableData);           break;  // 13
                case Variables.ShinyImage:      StoreShinyImages(variableData);     break;  // 14
                case Variables.Sound:           StoreSound(variableData);           break;  // 15
                case Variables.State:           StoreStates(variableData);          break;  // 16
                case Variables.StateImage:      StoreStateImages(variableData);     break;  // 17
                case Variables.Text:            StoreText(variableData);            break;  // 18
                case Variables.TextAlignment:   StoreTextAligment(variableData);    break;  // 19
                case Variables.TextColor:       StoreTextColors(variableData);      break;  // 20
                case Variables.TextEffect:      StoreTextEffect(variableData);      break;  // 21
                case Variables.TextPivot:       StoreTextPivots(variableData);      break;  // 22
                // case Variables.TextRotation:    StoreTextRotation(variableData);    break;  // 23
                case Variables.TextScale:       StoreTextScale(variableData);       break;  // 24
            }
        }

        private void StoreCheckedColors(Variable variableData)
        {
            StoreColors(variableData, compiler.CheckedColorVariables);
        }

        private void StoreColors(Variable variableData)
        {
            StoreColors(variableData, compiler.ColorVariables);
        }

        private void StoreColors(Variable variableData, Dictionary<string, Color> colorDictionary)
        {
            bool color = IsVariableType(Pattern.Variables.Color, ref variableData);

            if (color) AddColorValue(variableData, colorDictionary);
            compiler.Definition.CompilerMessage(variableData, color);
        }

        private bool IsVariableType(string variablePattern, ref Variable variableData)
        {
            bool variable = false;

            if (variableData.Value.Contains(Symbols.Command))
            {
                string text = variableData.Value;
                string variableValue = (string)compiler.Interpreter.Interpret(ref text, null, null, null, null, null);
                variable = Regex.IsMatch(variableValue, variablePattern);
                variableData.Value = variableValue;
            }
            else
                variable = Regex.IsMatch(variableData.Value, variablePattern);

            return variable;
        }

        private void AddColorValue(Variable variableData, Dictionary<string, Color> colorDictionary)
        {
            string[] segments = variableData.Value.Split(Symbols.LeftBracket[0], Symbols.Comma[0], Symbols.RightBracket[0]);
            
            Color hue = new Color();
            hue.R = Convert.ToByte(segments[segments.Length - 5]);
            hue.G = Convert.ToByte(segments[segments.Length - 4]);
            hue.B = Convert.ToByte(segments[segments.Length - 3]);
            hue.A = Convert.ToByte(segments[segments.Length - 2]);

            colorDictionary.Add(variableData.TypeID, hue);
        }

        private void StoreEffects(Variable variableData)
        {
            StoreEffects(variableData, compiler.EffectVariables);
        }

        private void StoreEffects(Variable variableData, Dictionary<string, SpriteEffects> effectDictionary)
        {
            bool effect = IsVariableType(Pattern.Variables.Effect, ref variableData);

            if (effect) AddEffectValue(variableData, effectDictionary);
            compiler.Definition.CompilerMessage(variableData, effect);
        }

        private void AddEffectValue(Variable variableData, Dictionary<string, SpriteEffects> effectDictionary)
        {
            string[] valueSplit = (from split in variableData.Value.Split(Symbols.Dot[0]) select split.Trim()).ToArray();
            string className = valueSplit[0];
            string value = valueSplit[1];

            if (className == Vocabulary.Classes.Mirror.Name)
                switch (value)
                {
                    case Classes.Mirror.ValueX: effectDictionary.Add(variableData.TypeID, SpriteEffects.FlipVertically); break;
                    case Classes.Mirror.ValueY: effectDictionary.Add(variableData.TypeID, SpriteEffects.FlipHorizontally); break;
                }
        }

        private void StoreFonts(Variable variableData)
        {
            StorePaths(variableData, compiler.FontVariables, Paths.Fonts);
        }

        private void StorePaths(Variable variableData, Dictionary<string, string> Dictionary, string datatypePath)
        {
            bool path = IsVariableType(Pattern.Variables.Path, ref variableData);

            if (path) AddPathValue(variableData, Dictionary, datatypePath);
            compiler.Definition.CompilerMessage(variableData, path);
        }

        private void AddPathValue(Variable variableData, Dictionary<string, string> Dictionary, string datatypePath)
        {
            string value = variableData.Value;
            value = datatypePath + value.Replace(Symbols.Dot[0], Symbols.Comment[0]);

            Dictionary.Add(variableData.TypeID, value);
        }

        private void StoreHighlights(Variable variableData)
        {
            StoreColors(variableData, compiler.HighlightColorVariables);
        }

        private void StoreIcons(Variable variableData)
        {
            StorePaths(variableData, compiler.IconVariables, Paths.Images);
        }

        private void StoreIconEffects(Variable variableData)
        {
            StoreEffects(variableData, compiler.IconEffectVariables);
        }

        private void StoreIconPivot(Variable variableData)
        {
            StorePivots(variableData, compiler.IconPivotVariables);
        }

        private void StoreIconRotation(Variable variableData)
        {
            StoreRotation(variableData, compiler.IconRotationVariables);
        }

        private void StoreImages(Variable variableData)
        {
            StorePaths(variableData, compiler.ImageVariables, Paths.Images);
        }

        private void StorePivots(Variable variableData)
        {
            StorePivots(variableData, compiler.PivotVariables);
        }

        private void StorePivots(Variable variableData, Dictionary<string, Vector2> vectorDictionary)
        {
            bool pivot = IsVariableType(Pattern.Variables.Pivot, ref variableData);

            if (pivot) AddPivotValue(variableData, vectorDictionary);
            compiler.Definition.CompilerMessage(variableData, pivot);
        }

        private void AddPivotValue(Variable variableData, Dictionary<string, Vector2> vectorDictionary)
        {
            string[] segments = variableData.Value.Split(Symbols.LeftBracket[0], Symbols.Comma[0], Symbols.RightBracket[0]);
            int lastPosition = segments.Length - 1;  // to skip empty space right after the ")"

            Vector2 vector = new Vector2();
            IFormatProvider culture = CultureInfo.InvariantCulture;
            vector.X = float.Parse(segments[lastPosition - 2], culture);
            vector.Y = float.Parse(segments[lastPosition - 1], culture);

            vectorDictionary.Add(variableData.TypeID, vector);
        }

        private void StoreRotation(Variable variableData)
        {
            StoreRotation(variableData, compiler.RotationVariables);
        }

        private void StoreRotation(Variable variableData, Dictionary<string, float> rotationDictionary)
        {
            bool rotation = Regex.IsMatch(variableData.Value, Pattern.Variables.Rotation);

            if (rotation)
            {
                float value = float.Parse(variableData.Value);
                rotationDictionary.Add(variableData.TypeID, value);
            }

            compiler.Definition.CompilerMessage(variableData, rotation);
        }

        private void StoreScale(Variable variableData)
        {
            StoreScale(variableData, compiler.ScaleVariables);
        }

        private void StoreSound(Variable variableData)
        {
            StorePaths(variableData, compiler.SoundVariables, Paths.Sounds);
        }

        private void StoreScale(Variable variableData, Dictionary<string, float> scaleDictionary)
        {
            bool scale = IsVariableType(Pattern.Variables.Scale, ref variableData);

            if (scale) AddScaleValue(variableData, scaleDictionary);
            compiler.Definition.CompilerMessage(variableData, scale);
        }

        private void AddScaleValue(Variable variableData, Dictionary<string, float> scaleDictionary)
        {
            IFormatProvider culture = CultureInfo.InvariantCulture;
            float value = (float)Math.Sqrt(float.Parse(variableData.Value, culture)) * 10.0f;  // x10 to get 100% if you type in 100%
            scaleDictionary.Add(variableData.TypeID, value);
        }

        private void StoreShinyImages(Variable variableData)
        {
            StorePaths(variableData, compiler.ShinyImageVariables, Paths.Images);
        }

        private void StoreStateImages(Variable variableData)
        {
            StorePaths(variableData, compiler.StateImageVariables, Paths.Images);
        }

        private void StoreStates(Variable variableData)
        {
            bool state = IsVariableType(Pattern.Variables.State, ref variableData);

            if (state) AddStateValue(variableData);
            compiler.Definition.CompilerMessage(variableData, state);
        }

        private void AddStateValue(Variable variableData)
        {
            string[] states = variableData.Value.Split('&');
            Datatypes.State stateData = new Datatypes.State();

            Create.VariableData data = new Create.VariableData();

            stateData.IsSet = data.IsSet;
            stateData.ReceivesMouseActions = data.ReceivesMouseActions;
            stateData.IsActive = data.IsActive;

            foreach (string stateValue in states)
            {
                switch (stateValue)
                {
                    case Datatypes.State.Set:               stateData.IsSet = true;                 break;
                    case Datatypes.State.NoMouseActions:    stateData.ReceivesMouseActions = false; break;
                    case Datatypes.State.Inactive:          stateData.IsActive = false;             break;
                }
            }

            compiler.StateVariables.Add(variableData.TypeID, stateData);
        }

        private void StoreText(Variable variableData)
        {
            bool text = false;

            // "post" command can be only be evaluated during object creation
            if (variableData.Value.Contains(Symbols.Command) && !variableData.Value.Contains(Commands.Post))
            {
                string currentText = variableData.Value;
                compiler.Interpreter.Interpret(ref currentText, null, null, null, null, null);
                text = Regex.IsMatch(currentText, Pattern.Variables.Text);
                variableData.Value = currentText;
            }
            else
                text = Regex.IsMatch(variableData.Value, Pattern.Variables.Text);

            if (text)
                compiler.TextVariables.Add(variableData.TypeID, variableData.Value);

            compiler.Definition.CompilerMessage(variableData, text);
        }

        private void StoreTextAligment(Variable variableData)
        {
            bool textAlignment =  IsVariableType(Pattern.Variables.TextAligment, ref variableData);

            if (textAlignment) AddAligmentValue(variableData, compiler.TextAligmentVariables);
            compiler.Definition.CompilerMessage(variableData, textAlignment);
        }

        private void AddAligmentValue(Variable variableData, Dictionary<string, Label.HorizontalAlignments> Dictionary)
        {
            string variableName = variableData.TypeID;
            Label.HorizontalAlignments alignment = (Label.HorizontalAlignments)Enum.Parse(typeof(Label.HorizontalAlignments), variableData.Value, true);
            Dictionary.Add(variableName, alignment);
        }

        private void StoreTextColors(Variable variableData)
        {
            StoreColors(variableData, compiler.TextColorVariables);
        }

        private void StoreTextEffect(Variable variableData)
        {
            StoreEffects(variableData, compiler.TextEffectVariables);
        }

        private void StoreTextPivots(Variable variableData)
        {
            StorePivots(variableData, compiler.TextPivotVariables);
        }

        private void StoreTextRotation(Variable variableData)
        {
            StoreRotation(variableData, compiler.TextRotationVariables);
        }

        private void StoreTextScale(Variable variableData)
        {
            StoreScale(variableData, compiler.TextScaleVariables);
        }
    }
}
