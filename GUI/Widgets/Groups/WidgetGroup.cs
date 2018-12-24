using Navi.Defaults;
using Navi.Screen;
using System;
using System.Collections.Generic;

namespace Navi.GUI.Widgets.Groups
{
    /// <remarks>
    /// Some source code is taken from the WidgetGroup class of Navi (2014/2015).
    /// </remarks>
    public class WidgetGroup
    {
        protected List<SurfaceWidget> elements;
        /// <summary>
        /// Stores the state of the group members, so if they are set.
        /// </summary>
        protected List<bool> active;

        /// <param name="active">Array that tells which element is active.</param>
        public delegate void Action(bool[] active);
        
        /// <summary>
        /// Function that has to be defined before you add an element to the group.
        /// </summary>

        public event Action OnChange;  // this function can be defined everywhere else

        public WidgetGroup()
        {
            elements = new List<SurfaceWidget>();
            active = new List<bool>();
        }

        public int Count
        {
            get
            {
                return elements.Count;
            }
        }

        public bool GroupActionIsSet
        {
            get { return OnChange != null; }
        }

        protected void GroupChange()
        {
            try
            {
                StateChange();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void StateChange()
        {
            if (OnChange != null)
                OnChange(active.ToArray());
            else
                throw new Exceptions.NoOnChangeActionException();
        }
    }
}
