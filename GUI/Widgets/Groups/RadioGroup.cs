using Microsoft.Xna.Framework;
using Navi.Screen;

namespace Navi.GUI.Widgets.Groups
{
    /// <remarks>
    /// Some source code is taken from the WidgetGroup class of Navi (2014/2015).
    /// </remarks>
    public sealed class RadioGroup : WidgetGroup
    {
        public void Add(RadioButton radioButton)
        {
            if (elements.Count == 0)
                radioButton.IsSet = true;

            radioButton.OnClick += ChangeGroupStates;

            elements.Add(radioButton);
            active.Add(radioButton.IsSet);
        }

        private void ChangeGroupStates(SurfaceWidget element, Vector2 mousePos)
        {
            for (int i = 0; i < elements.Count; i++)
            {
                if (!elements[i].Equals(element)) 
                    active[i] = ((RadioButton)elements[i]).IsSet = false;
                else  // if (options[i].Equals(element))
                    active[i] = true;
            }

            GroupChange();
        }

        public void Activate(int elementNumber)
        {
            RadioButton rad;

            for (int i = 0; i < elements.Count; i++)
            {
                rad = elements[i] as RadioButton;
                rad.IsSet = false;
            }

            rad = elements[elementNumber] as RadioButton;
            rad.IsSet = true;
        }
    }
}
