using Microsoft.Xna.Framework;
using Navi.GUI.Faces.Language;
using Navi.GUI.Faces.Parser;
using Navi.Screen;
using System.Collections.Generic;

namespace Navi.GUI.Faces.Functions
{
    public struct Add
    {
        private Compiler compiler;

        public Add(Compiler compiler)
        {
            this.compiler = compiler;
        }

        public void AddObjects(List<string> commands, Interface surface, ref int line)
        {
            line++;  // skip "Add"-line

            if (line < commands.Count)
            {
                Structs.Object objectData = compiler.ObjectAnalyzer.GetAdditionData(compiler, commands[line]);
                bool doneAdding = true;
                SurfaceWidget widget = null;

                while (Vocabulary.IsDataType(objectData.Type))
                {
                    foreach (string name in objectData.Names)
                    {
                        widget = compiler.GetWidget(objectData.Type, name);

                        if (widget == null)
                        {
                            doneAdding = false;
                            break;
                        }

                        Vector4 vector = objectData.Bounds;
                        surface.Add(widget, vector.X, vector.Y, vector.Z, vector.W);
                        objectData.Bounds += objectData.BoundsTranformation;
                    }

                    if (widget == null) break;  // so if the widget was null in the foreach-loop then we also break in the while-loop

                    compiler.Debugger.Message(commands[line], true);
                    line++;
                    if (line >= commands.Count) break;

                    objectData = objectData.GetAdditionData(compiler, commands[line]);
                }

                if (line < commands.Count)
                    compiler.Debugger.Message(commands[line], doneAdding);
            }
        }
    }
}
