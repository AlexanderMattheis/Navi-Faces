using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Navi.Audio.Player;
using Navi.GUI;
using Navi.Screen;
using Navi.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Patterns = Navi.GUI.Faces.Language.Patterns;
using Shortcuts = Navi.Input.Shortcuts;
using Symbols = Navi.GUI.Faces.Language.Vocabulary.Symbols;

namespace Navi.Content.Faces
{
    public sealed class Controls
    {
        private Interface face;

        public Controls(ContentManagement content, GraphicsDevice graphicsDevice, SoundPlayer soundPlayer, SurfaceManager surfaceManager, Interface face)
        {
            this.face = face;
        }

        public List<string> Tasks()
        {
            List<string> names = Shortcuts.Tuples.Keys.ToList();
            List<string> functions = new List<string>();

            foreach (string namePart in names)
            {
                string[] splittedName = Regex.Split(namePart, Patterns.CapitalLetters);

                StringBuilder builder = new StringBuilder();
                foreach (string split in splittedName)
                {
                    builder.Append(split);
                    builder.Append(" ");
                }

                functions.Add(builder.ToString().Trim());
            }

            return functions;
        }

        public List<string> Hotkeys()
        {
            List<List<Keys[]>> shortcutsLists = Shortcuts.Tuples.Values.ToList();  // shortcutsLists of functions
            List<string> shortcuts = new List<string>();

            // example: Keys[] looks like "Ctrl + F" and List<Keys> like { "Alt + F4", "Esc" } for one function like "Exit"
            foreach (List<Keys[]> shortcutList in shortcutsLists)
            {
                for (int i = 0; i < shortcutList.Count; i++)
                {
                    Keys[] shortcutKeys = shortcutList[i];

                    string hotkeys = string.Empty;
                    for (int j = 0; j < shortcutKeys.Length; j++)
                        hotkeys += Enum.GetName(typeof(Keys), shortcutKeys[j]) + (j < shortcutKeys.Length - 1 ? Symbols.Plus : string.Empty);

                    shortcuts.Add(hotkeys);
                }
            }

            return shortcuts;
        }
    }
}
