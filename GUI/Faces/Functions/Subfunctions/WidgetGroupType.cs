using Navi.GUI.Faces.Parser;
using Navi.GUI.Faces.Structs;
using Navi.GUI.Widgets.Groups;
using Navi.Helper.Structures;
using System.Collections.Generic;

using ObjectTypes = Navi.GUI.Faces.Language.Vocabulary.ObjectTypes;

namespace Navi.GUI.Faces.Functions.Subfunctions
{
    public struct WidgetGroupType
    {
        public static WidgetGroup Create(string type)
        {
            WidgetGroup group = null;

            switch (type)
            {
                case ObjectTypes.RadioButton:   group = new RadioGroup();       break;
                case ObjectTypes.Checkbox:      group = new CheckBoxGroup();    break;
            }

            return group;
        }
    }
}
