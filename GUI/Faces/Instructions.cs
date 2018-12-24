using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Navi.Audio.Player;
using Navi.Defaults;
using Navi.GUI.Faces.Parser;
using Navi.GUI.Faces.Structs;
using Navi.Screen;
using Navi.System;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

using KeyWords = Navi.GUI.Faces.Language.Vocabulary.KeyWords;
using Pattern = Navi.GUI.Faces.Language.Patterns;
using Symbols = Navi.GUI.Faces.Language.Vocabulary.Symbols;
using Variables = Navi.GUI.Faces.Language.Vocabulary.Variables;

namespace Navi.GUI.Faces
{
    public struct Instructions
    {
        private Compiler compiler;

        public Instructions(Compiler compiler)
        {
            this.compiler = compiler;
        }

        public void Import(List<string> commands, ContentManagement content, GraphicsDevice graphicsDevice, SoundPlayer soundPlayer, SurfaceManager surfaceManager, ref int line)
        {
            if (line < commands.Count)
            {
                bool import = Regex.IsMatch(commands[line].Trim(), Pattern.KeyWords.Import);
                bool importWithParameters = Regex.IsMatch(commands[line].Trim(), Pattern.KeyWords.ImportWithParameters);

                string[] segments;

                if (import || importWithParameters)
                    while (import || importWithParameters)
                    {
                        if (import)  // example: import(HudPages.Options)
                        {
                            compiler.Debugger.Message(KeyWords.Import, import);
                            segments = commands[line].Split(Symbols.LeftBracket[0], Symbols.RightBracket[0]);
                            compiler.Face.Subsurfaces.Add(new Interface(content, graphicsDevice, soundPlayer, surfaceManager, compiler.Face.PixelResolution, GetPath(segments), compiler.Face).Instance());

                            line++;
                            import = line < commands.Count && Regex.IsMatch(commands[line].Trim(), Pattern.KeyWords.Import);
                        }
                        else if (importWithParameters)  // example: import(X.Y){ text = WAIT... }
                        {
                            compiler.Debugger.Message(KeyWords.Import, importWithParameters);
                            segments = commands[line].Split(Symbols.LeftBracket[0], Symbols.RightBracket[0]);
                            Interface subSurface = new Interface(content, graphicsDevice, soundPlayer, surfaceManager, compiler.Face.PixelResolution, GetPath(segments), compiler.Face);
                            string[] parameters = segments[segments.Length - 1].Split(Symbols.LeftCurlyBracket[0], Symbols.Comma[0], Symbols.RightCurlyBracket[0]);
                            parameters = (from parameter in parameters where parameter != string.Empty select parameter.Trim()).ToArray();
                            SetParameters(subSurface, parameters);
                            subSurface = subSurface.Instance();
                            compiler.Face.Subsurfaces.Add(subSurface);

                            line++;
                            importWithParameters = line < commands.Count && Regex.IsMatch(commands[line].Trim(), Pattern.KeyWords.Import);
                        }
                    }
                else
                    compiler.Debugger.Message(KeyWords.Import, import || importWithParameters);
            }
        }

        private string GetPath(string[] segments)
        {
            int lastPosition = segments.Length - 1;
            return segments[lastPosition - 1].Trim().Replace(Symbols.Dot, Paths.XnaDirectorySymbols);  // the next-to-last element is the path    
        }

        private void SetParameters(Interface face, string[] parameters)
        {
            foreach (string parameter in parameters)
            {
                Variable variable = face.Compiler.VariableAnalyzer.GetData(parameter);
                face.Compiler.Definition.SetData(variable);
            }
        }

        public void SetBounds(List<string> commands, ref int line)
        {
            Vector4 percentBoundingBox = new Vector4();
            bool dimensions = line < commands.Count && Regex.IsMatch(commands[line].Trim(), Pattern.Variables.Dimensions);  // example: dimensions = (0.0, 0.0, 100.0, 100.0)

            if (dimensions)  // RegEx: ^dimensions\s*=\s*\((\s*[0-9.]+\s*,){3}\s*[0-9.]+\s*\)
                percentBoundingBox = Dimensions(commands, compiler.Face, ref line);
            else
                percentBoundingBox = new Vector4(0.0f, 0.0f, 100.0f, 100.0f);

            compiler.Debugger.Message(Variables.Dimensions, dimensions);

            float percentPosX = percentBoundingBox.X;
            float percentPosY = percentBoundingBox.Y;

            bool alignment = line < commands.Count && Regex.IsMatch(commands[line].Trim(), Pattern.Variables.Alignment);  // example: alignment = Center

            if (alignment && commands[line].Contains(Variables.Alignment))  // RegEx: ^alignment\s*=\s*(Left|Center|Right)$
                Align(commands, ref percentPosX, compiler.Face.AspectRatio, ref line);
            else
                percentPosX += ((compiler.Face.AspectRatio - 1) / 2) * 100;  // centered is default

            compiler.Debugger.Message(Variables.Alignment, alignment);

            compiler.Face.PercentOrigin = new Vector2(percentPosX, percentPosY);
            compiler.Face.PercentSize = new Vector2(percentBoundingBox.Z, percentBoundingBox.W);
        }

        private Vector4 Dimensions(List<string> commands, Surface surface, ref int line)
        {
            string[] segments = commands[line].Split(Symbols.LeftBracket[0], Symbols.Comma[0], Symbols.RightBracket[0]);
            int lastPosition = segments.Length - 1;

            IFormatProvider culture = CultureInfo.InvariantCulture;  // so read data without converting "." to ","

            Vector4 boundingBox = new Vector4();
            boundingBox.X = float.Parse(segments[lastPosition - 4], culture);
            boundingBox.Y = float.Parse(segments[lastPosition - 3], culture);
            boundingBox.Z = float.Parse(segments[lastPosition - 2], culture);
            boundingBox.W = float.Parse(segments[lastPosition - 1], culture);

            line++;
            return boundingBox;
        }

        private void Align(List<string> commands, ref float percentPosX, float aspectRatio, ref int line)
        {
            string[] segments = commands[line].Split(Symbols.Equal[0]);
            string element = segments[segments.Length - 1].Trim();
            Surface.HorizontalAlignments alignment = (Surface.HorizontalAlignments)Enum.Parse(typeof(Surface.HorizontalAlignments), element, true);

            #region example
            // 16/9 = ~1.77
            // 1.77-1 = 0.77;
            // 0.77 / 2 = ~0.38
            // 0.38 * 100 = 38
            // so if you have an Surface centered in the middle of the screen
            // with an aspect ratio of 1:1
            // and you scale it to 16:9, then you have to move 
            // your origin ~38% to the right to make the Surface be placed in the
            // middle of the screen;
            //
            //                   177% (16:9)
            // |---------------------------------------------|
            //   38%             100% (1:1)            38%
            // |-------|-----------------------------|-------|
            //          _____________________________
            //         |                             |
            //         |                             |
            //         |                             |
            //         |                             |
            //         |                             |
            //         |                             |
            //         |                             |
            //         |                             |
            //         |                             |
            //         |                             |
            //         |                             |
            //         |_____________________________|
            #endregion
            if (alignment == Surface.HorizontalAlignments.Center)
                percentPosX += ((aspectRatio - 1) / 2) * 100;
            else if (alignment == Surface.HorizontalAlignments.Right)
                percentPosX += (aspectRatio - 1) * 100;

            line++;
        }
    }
}
