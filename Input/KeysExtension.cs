using Microsoft.Xna.Framework.Input;

namespace Navi.Input
{
    public static class KeysExtension
    {
        public static bool ContainsShortcut(this Keys[] container, Keys[] shortcut)
        {
            bool isContained = false;

            #region example
            ////    shortcut         container
            // i:     0    1     j:    0    1     2    3   4
            //      Strg + A         Strg + S + Strg + A + B
            //
            // comments
            // first time  (j = 0):  i++ because (Strg == Strg) -> i = 1
            // second time (j = 1):  i=0 because (A != S)       -> i = 0 
            // third time  (j = 2):  i++ because (Strg == Strg) -> i = 1 
            // fourth time (j = 3):  i++ because (A == A)       -> i = 2 
            // break because i > shortcut.Length
            #endregion

            int i = 0;
            for (int j = 0; j < container.Length; j++)
            {
                if (container[j] == shortcut[i])
                {
                    i++;
                    if (shortcut.Length >= i)
                    {
                        isContained = true;
                        break;
                    }
                }
                else
                    i = 0;
            }

            return isContained;
        }
    }
}
