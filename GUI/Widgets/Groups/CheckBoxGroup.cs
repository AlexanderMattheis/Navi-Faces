using Microsoft.Xna.Framework;
using Navi.Screen;

namespace Navi.GUI.Widgets.Groups
{
    public sealed class CheckBoxGroup : WidgetGroup
    {
        public void Add(CheckBox checkbox)
        {
            checkbox.OnClick += ChangeBoxState;

            elements.Add(checkbox);
            active.Add(checkbox.IsSet);
        }

        private void ChangeBoxState(SurfaceWidget element, Vector2 mousePos)
        {
            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[i].Equals(element))
                {
                    active[i] = ((CheckBox)elements[i]).IsSet;
                    break;
                }
            }

            GroupChange();
        }

        public void Deactivate(int elementNumber)
        {
            CheckBox chb = elements[elementNumber] as CheckBox;
            chb.IsSet = false;
        }
    }
}
