using Microsoft.Xna.Framework;
using Navi.GUI.Faces.Parser;
using Navi.Helper.Structures;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

using Pattern = Navi.GUI.Faces.Language.Patterns;
using Symbols = Navi.GUI.Faces.Language.Vocabulary.Symbols;
using Variables = Navi.GUI.Faces.Language.Vocabulary.Variables;

namespace Navi.GUI.Faces.Structs
{
    public struct Object
    { 
        public Vector4 Bounds { get; set; }

        public Vector4 BoundsTranformation { get; set; }

        public Dictionary<string, string[]> Keys { get; set; }

        public string Link { get; set; }

        public string LinkParameter { get; set; }

        public List<string> Names { get; set; }

        public string[] Parameters { get; set; }

        public string Type { get; set; }

        public Structs.Object GetData(string command)
        {
            bool isObjectInstation = Regex.IsMatch(command.Trim(), Pattern.ObjectInstantion);
            bool isMultipleObjectInstation = Regex.IsMatch(command.Trim(), Pattern.MultipleObjectInstantion);

            Names = new List<string>();
            Parameters = new string[0];
            Type = string.Empty;

            string[] splits;

            if (isObjectInstation)
            {
                // example: imgLogo(image, pivot)
                splits = command.Split(Symbols.LeftBracket[0], Symbols.RightBracket[0]);
                Type = splits[0].Trim().Substring(0, 3);
                Names.Add(splits[0].Remove(0, 3).Trim());

                bool splitHaveParamters = splits.Length > 1;
                if (splitHaveParamters) Parameters = (from parameter in splits[1].Split(Symbols.Comma[0]) select parameter.Trim()).ToArray();
            }
            else if (isMultipleObjectInstation)
            {
                // example: btn{New, Controls, Credits, Exit}(text)
                splits = command.Split(Symbols.LeftCurlyBracket[0], 
                    Symbols.RightCurlyBracket[0], 
                    Symbols.LeftBracket[0],
                    Symbols.RightBracket[0]);

                Type = splits[0].Trim();
                Names = (from name in splits[1].Split(Symbols.Comma[0]) select name.Trim()).ToList<string>();
                Parameters = (from parameter in splits[3].Split(Symbols.Comma[0]) select parameter.Trim()).ToArray();
            }

            return this;
        }

        public Structs.Object GetLinkData(string command)
        {
            bool isLinkCreation = Regex.IsMatch(command.Trim(), Pattern.LinkCreation);
            bool isMultipleLinkCreation = Regex.IsMatch(command.Trim(), Pattern.MultipleLinkCreation);

            Type = string.Empty;
            Names = new List<string>();
            Link = string.Empty;
            Keys = new Dictionary<string, string[]>();

            string[] splits;

            if (isLinkCreation)
            {
                // example: btnNew[N]->MapLoading;
                splits = command.Split(Symbols.Link[0], Symbols.Link[1]);  // "->"
                Link = splits[splits.Length - 1].Trim();

                splits = splits[0].Split(Symbols.LeftSquareBracket[0], Symbols.RightSquareBracket[0]);  // "[" and "]"
                Type = splits[0].Trim().Substring(0, 3);
                Names.Add(splits[0].Remove(0, 3).Trim());  // deleting prefix

                bool containsKeys = splits.Length > 1;
                if (containsKeys)
                {
                    string[] keys = (from key in splits[1].Split(Symbols.Comma[0]) select key.Trim()).ToArray();
                    Keys.Add(Names[0], keys);
                }
            }
            else if (isMultipleLinkCreation)
            {
                // example btn{Controls[C], Credits}->#post;
                splits = command.Split(Symbols.LeftCurlyBracket[0], Symbols.RightCurlyBracket[0]);
                Type = splits[0];
                Link = splits[2].Replace(Symbols.Link, string.Empty).Trim();

                string[] names = (from name in splits[1].Split(Symbols.Comma[0]) select name.Trim()).ToArray();  // Controls[C], Credits
                foreach (string element in names)  // iterating through the name list
                {
                    string[] elementSplit = element.Split(Symbols.LeftSquareBracket[0], Symbols.RightSquareBracket[0]);
                    string name = elementSplit[0].Trim();
                    Names.Add(name);

                    bool containsKeys = elementSplit.Length > 1;
                    if (containsKeys)
                    {
                        string[] keys = (from key in elementSplit[1].Split(Symbols.Comma[0]) select key.Trim()).ToArray();
                        Keys.Add(name, keys);
                    }
                }
            }

            return this;
        }

        public Structs.Object GetAdditionData(Compiler compiler, string command)
        {
            bool isAddition = Regex.IsMatch(command.Trim(), Pattern.SurfaceAddition);
            bool isMultipleAddition = Regex.IsMatch(command.Trim(), Pattern.MultipleSurfaceAddition);
            bool isPartialAddition = Regex.IsMatch(command.Trim(), Pattern.PartialSurfaceAddition);
            bool isPositioning = Regex.IsMatch(command.Trim(), Pattern.Positioning);

            Type = string.Empty;
            Names = new List<string>();
            if (!isPartialAddition) Bounds = new Vector4();
            BoundsTranformation = new Vector4();

            if (isAddition) SetAdditionValues(command);
            else if (isMultipleAddition) SetMultipleAdditionValues(compiler, command);
            else if (isPartialAddition) SetPartialAdditionValues(command);
            else if (isPositioning) SetPositionValues(command);

            return this;
        }

        private void SetAdditionValues(string command)
        {
            // example: (btnExit, 50.0, 83.3, 36.5, 7.29)
            SetValues(command);
        }

        private void SetValues(string command)
        {
            string[] splits = command.Split(Symbols.LeftBracket[0], Symbols.Comma[0], Symbols.RightBracket[0]);
            SetIdentifier(splits);
            int lastPosition = splits.Length - 1;
            bool threeParameters = splits.Length == 5;

            Vector4 vector = new Vector4();
            IFormatProvider culture = CultureInfo.InvariantCulture;  // so read data without converting "." to ","
            vector.X = threeParameters ? float.Parse(splits[lastPosition - 2], culture) : float.Parse(splits[lastPosition - 4], culture);
            vector.Y = threeParameters ? float.Parse(splits[lastPosition - 1], culture) : float.Parse(splits[lastPosition - 3], culture);
            vector.Z = threeParameters ? 0.0f : float.Parse(splits[lastPosition - 2], culture);
            vector.W = threeParameters ? 0.0f : float.Parse(splits[lastPosition - 1], culture);
            Bounds = vector;
        }

        private void SetIdentifier(string[] splits)
        {
            Type = splits[1].Trim().Substring(0, 3);
            Names.Add(splits[1].Remove(0, 3));
        }

        private void SetMultipleAdditionValues(Compiler compiler, string command)
        {
            // example: (btn{New, Controls, Credits}, 50.0, 39.2[+10.0], 36.5, 7.29)
            // example2: (lblTasks{}, 31.7, 50.4[+5.69])
            string[] splits = command.Split(Symbols.LeftCurlyBracket[0], Symbols.RightCurlyBracket[0]);

            Type = splits[0].Replace(Symbols.LeftBracket, string.Empty).Trim().Substring(0, 3);  // lbl
            Names = (from name in splits[1].Split(Symbols.Comma[0]) select name.Trim()).ToList();

            if (Names.Count == 1)
            {
                string objectName = splits[0].Replace(Symbols.LeftBracket, string.Empty).Trim().Remove(0, 3);  // Tasks{}
                Names.RemoveAt(0);

                foreach (string name in compiler.Labels.Keys)
                    if (name.Contains(objectName))  // adding widgets with names like lblTasks{}0, lblTasks{}1, ...
                        Names.Add(name);
            }

            float[] vector = new float[4];
            float[] transformation = new float[4];
            IFormatProvider culture = CultureInfo.InvariantCulture;  // so read data without converting "." to ","

            splits = (from value in splits[2].Split(Symbols.Comma[0], Symbols.RightBracket[0]) select value.Trim()).ToArray();

            for (int i = 1; i < splits.Length - 1; i++)  // "+1" and "-1" to skip the left "," and the right ")"
            {
                string[] valueSplit = splits[i].Split(Symbols.LeftSquareBracket[0], Symbols.RightSquareBracket[0]);
                vector[i - 1] = float.Parse(valueSplit[0], culture);
                transformation[i - 1] = float.Parse(valueSplit.Length == 3 ? valueSplit[1].Trim() : "0.0", culture);
            }

            Bounds = new Vector4(vector[0], vector[1], vector[2], vector[3]);
            BoundsTranformation = new Vector4(transformation[0], transformation[1], transformation[2], transformation[3]);
        }

        private void SetPartialAdditionValues(string command)
        {
            // example: (btnExit, y = 83.3)
            string[] splits = command.Split(Symbols.LeftBracket[0], Symbols.Comma[0], Symbols.RightBracket[0]);

            SetIdentifier(splits);

            Vector4 vector = Bounds;
            IFormatProvider culture = CultureInfo.InvariantCulture;  // so read data without converting "." to ","
            for (int i = 2; i < splits.Length - 1; i++)
            {
                string[] variableSplit = (from data in splits[i].Split(Symbols.Equal[0]) select data.Trim()).ToArray();
                string variable = variableSplit[0];
                string value = variableSplit[1].Trim();

                switch (variable)
                {
                    case Variables.PositionX:   vector.X = float.Parse(value, culture); break;
                    case Variables.PositionY:   vector.Y = float.Parse(value, culture); break;
                    case Variables.Width:       vector.Z = float.Parse(value, culture); break;
                    case Variables.Height:      vector.W = float.Parse(value, culture); break;
                }
            }

            Bounds = vector;
        }

        private void SetPositionValues(string command)
        {
            // example: (lblMessage, 50.0, 50.0)
            SetValues(command);
        }
    }
}
