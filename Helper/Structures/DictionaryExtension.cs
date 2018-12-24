using System;
using System.Collections.Generic;

namespace Navi.Helper.Structures
{
    [Serializable]
    public static class DictionaryExtension
    {
        /// <remarks>
        /// You can't use TryGetValue(...) for a splitted equivalent string because TryGetValue(...)
        /// is searching for an equal string (so it is searching for the right key reference).
        /// </remarks>
        public static TValue GetValue<TValue>(this Dictionary<string, TValue> dictionary, string equivalentKey)
        {
            int i = 0, j = 0;

            foreach (string key in dictionary.Keys)
            {
                if (key == equivalentKey)
                {
                    foreach (TValue value in dictionary.Values)
                    {
                        if (j == i) return value;
                        j++;
                    }
                }

                i++;
            }

            return default(TValue);
        }
    }
}
