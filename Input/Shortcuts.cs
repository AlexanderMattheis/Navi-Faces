using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Navi.Input
{
    public struct Shortcuts
    {
        private const string Exception = "Tuples";

        static Shortcuts()
        {
            Keys[] keys;

            MainMenu = new List<Keys[]>();
            keys = new Keys[] { Keys.Escape };
            MainMenu.Add(keys);

            ScrollTop = new List<Keys[]>();
            keys = new Keys[] { Keys.W };
            ScrollTop.Add(keys);

            ScrollLeft = new List<Keys[]>();
            keys = new Keys[] { Keys.A };
            ScrollLeft.Add(keys);

            ScrollBottom = new List<Keys[]>();
            keys = new Keys[] { Keys.S };
            ScrollBottom.Add(keys);

            ScrollRight = new List<Keys[]>();
            keys = new Keys[] { Keys.D };
            ScrollRight.Add(keys);
        }

        public static List<Keys[]> MainMenu { get; private set; }

        public static List<Keys[]> ScrollTop { get; private set; }

        public static List<Keys[]> ScrollLeft { get; private set; }

        public static List<Keys[]> ScrollBottom { get; private set; }

        public static List<Keys[]> ScrollRight { get; private set; }

        public static Dictionary<string, List<Keys[]>> Tuples
        {
            get
            {
                Dictionary<string, List<Keys[]>> tuples = new Dictionary<string, List<Keys[]>>();

                Type type = new Shortcuts().GetType();

                PropertyInfo[] properties = type.GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (property.Name != Exception)
                    {
                        List<Keys[]> shortcuts = (List<Keys[]>)property.GetValue(type);
                        tuples.Add(property.Name, shortcuts);
                    }
                }

                return tuples;
            }
        }
    }
}
