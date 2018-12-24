using Navi.Defaults;
using System;
using System.Reflection;

namespace Navi.GUI.Faces.Language
{
    public struct Vocabulary
    {
        public static bool IsDataType(string dataType)
        {
            return IsInStructure(dataType, new Vocabulary.ObjectTypes().GetType());
        }

        private static bool IsInStructure(string element, Type structure)
        {
            FieldInfo[] fields = structure.GetFields();

            for (int i = 0; i < fields.Length; i++)
            {
                string value = fields[i].GetValue(structure).ToString();
                if (value == element)
                    return true;
            }

            return false;
        }

        public static bool IsVariableType(string variableType)
        {
            return IsInStructure(variableType, new Vocabulary.Variables().GetType());
        }

        public struct Classes
        {
            public struct Mirror
            {
                public const string Name = "Mirror";
                public const string ValueX = "X";
                public const string ValueY = "Y";
            }
        }

        public struct Commands
        {
            public const string Align = "align";
            public const string Back = "back";
            public const string Big = "big";
            public const string Color = "color";
            public const string Effect = "effect";
            public const string Exit = "exit";
            public const string Font = "font";
            public const string Function = "fx";
            public const string GroupFunction = "gfx";
            public const string Image = "image";
            public const string Input = "input";
            public const string Database = "db";
            public const string NewLine = "ln";
            public const string Pivot = "pivot";
            public const string Post = "post";
            public const string Rotation = "rotation";
            public const string Scale = "scale";
            public const string Sound = "sound";
            public const string Split = "split";
            public const string State = "state";
        }

        public struct ObjectTypes
        {
            public const string Button = "btn";
            public const string Checkbox = "chb";
            public const string IconButton = "ibt";
            public const string Image = "img";
            public const string Label = "lbl";
            public const string RadioButton = "rad";
        }

        public struct Functions
        {
            public const string Define = "Define";
            public const string Create = "Create";
            public const string Link = "Link";
            public const string Add = "Add";
        }

        public struct KeyWords
        {
            public const string Import = "import";
        }

        public struct Symbols
        {
            public const string Comma = ",";
            public const string Command = "#";
            public const string Comment = ScriptSymbols.Comment;
            public const string Delimeter = ScriptSymbols.Delimeter;
            public const string Dot = ".";
            public const string Equal = "=";
            public const string Escape = ScriptSymbols.Escape;
            public const string FunctionMarker = ScriptSymbols.Rubric;
            public const string GroupBrackets = LeftCurlyBracket + RightCurlyBracket;
            public const string LeftAngleBracket = "<";
            public const string LeftBracket = "(";
            public const string LeftCurlyBracket = "{";
            public const string LeftSquareBracket = "[";
            public const string Link = "->";
            public const string Minus = "-";
            public const string Plus = "+";
            public const string RightAngleBracket = ">";
            public const string RightBracket = ")";
            public const string RightCurlyBracket = "}";
            public const string RightSquareBracket = "]";
        }

        /// <summary>
        /// Predefined variables.
        /// </summary>
        public struct Variables
        {
            public const string Alignment = "alignment";
            public const string CheckedColor = "checkedColor";      // 1
            public const string Color = "color";                    // 2
            public const string Dimensions = "dimensions";
            public const string Effect = "effect";                  // 3
            public const string Font = "font";                      // 4
            public const string Height = "h";
            public const string Highlight = "highlight";            // 5
            public const string Icon = "icon";                      // 6
            public const string IconEffect = "iconEffect";          // 7
            public const string IconPivot = "iconPivot";            // 8
            public const string IconRotation = "iconRotation";      // 9
            public const string Image = "image";                    // 10
            public const string Pivot = "pivot";                    // 11
            public const string Rotation = "rotation";              // 12
            public const string PositionX = "x";
            public const string PositionY = "y";
            public const string Scale = "scale";                    // 13
            public const string ShinyImage = "shinyImage";          // 14
            public const string Sound = "sound";                    // 15
            public const string State = "state";                    // 16
            public const string StateImage = "stateImage";          // 17
            public const string Text = "text";                      // 18
            public const string TextAlignment = "textAlignment";    // 19
            public const string TextColor = "textColor";            // 20
            public const string TextEffect = "textEffect";          // 21
            public const string TextPivot = "textPivot";            // 22
            public const string TextRotation = "textRotation";      // 23
            public const string TextScale = "textScale";            // 24
            public const string Width = "w";
        }
    }
}
