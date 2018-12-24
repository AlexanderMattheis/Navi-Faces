using Microsoft.Xna.Framework.Input;
using Navi.GUI.Faces.Debugging;
using Navi.GUI.Faces.Language;
using Navi.GUI.Faces.Parser;
using Navi.GUI.Widgets.Groups;
using Navi.Helper.Structures;
using Navi.Screen;
using System;
using System.Collections.Generic;

using ObjectTypes = Navi.GUI.Faces.Language.Vocabulary.ObjectTypes;

namespace Navi.GUI.Faces.Functions
{
    public class Link
    {
        private Compiler compiler;

        public Link(Compiler compiler)
        {
            this.compiler = compiler;
        }

        public void SetLinks(List<string> commands, Interface surface, ref int line)
        {
            WidgetGroup group = null;

            if (line < commands.Count)
            {
                if (commands[line].Trim() == Vocabulary.Functions.Link + ":")
                {
                    line++;  // skip "Link"-line

                    Structs.Object linkData = compiler.ObjectAnalyzer.GetLinkData(commands[line]);
                    bool doneLinking = true;

                    while (Vocabulary.IsDataType(linkData.Type))
                    {
                        group = Group(linkData);
                        foreach (string name in linkData.Names)
                        {
                            string link = linkData.Link;
                            string[] keys = linkData.Keys.GetValue(name);
                            SurfaceWidget widget = compiler.GetWidget(linkData.Type, name);

                            if (widget != null)
                            {
                                bool keysPassed = linkData.Keys.Count > 0;
                                if (keysPassed)
                                    widget.EventKeys = new Keys[linkData.Keys.Count];

                                for (int i = 0; i < linkData.Keys.Count; i++)
                                {
                                    Keys key = (Keys)Enum.Parse(typeof(Keys), keys[i], true);
                                    widget.EventKeys[i] = key;
                                }

                                surface.Parent = surface.Parent == null ? surface : surface.Parent;
                                compiler.Interpreter.Interpret(ref link, name, group, widget, surface, surface.Parent);
                            }
                            else
                            {
                                doneLinking = false;
                                compiler.Debugger.Comment(linkData.Type + name + Errors.ObjectNotExits);
                            }
                        }

                        compiler.Debugger.Message(commands[line], doneLinking);

                        line++;
                        linkData = compiler.ObjectAnalyzer.GetLinkData(commands[line]);
                    }
                }

                compiler.Debugger.Message(string.Empty);
            }
        }

        private WidgetGroup Group(Structs.Object linkData)
        {
            string name = string.Join(string.Empty, linkData.Names);

            switch (linkData.Type)
            {
                case ObjectTypes.RadioButton: return compiler.RadioGroup.GetValue(name);
            }

            return null;
        }
    }
}
