using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace AstrologyGame.Menus
{
    // a menu where you can select one thing in a list
    public abstract class SelectMenu : Menu
    {
        // should the menu draw a cursor next to the selected item?
        public virtual bool DrawCursor => true;

        protected int selectedIndex = 0;
        protected int selectionCount = 1; // how many things are there to select?

        public int SelectedIndex { get { return selectedIndex; } }

        protected OrderedPair cursorCoords;

        // what controls are used to move the selection
        protected Control IncrementControl { get; set; } = Control.Down;
        protected Control DecrementControl { get; set; } = Control.Up;
        protected Control SelectControl { get; set; } = Control.Enter;

        // the user has pressed enter (or whatever key is used to signify selecting)
        public virtual void SelectionMade() { }
        // the user has hit the increment or decrement control
        public virtual void SelectionChanged() { }

        public override void HandleInput(List<Control> controls)
        {
            base.HandleInput(controls);

            // if no options, theres nothing to do
            if (selectionCount == 0)
                return;

            // increment or decrement index
            if (controls.Contains(IncrementControl))
            {
                selectedIndex++;
            }
            else if (controls.Contains(DecrementControl))
            {
                selectedIndex--;
            }

            // use modulo to wrap around if the index is too high
            selectedIndex %= selectionCount;
            // if the index is negative, add the selectionCount to wrap it
            if (selectedIndex < 0)
                selectedIndex += selectionCount;

            // make a selection if user hits select, and there is something to select
            if (controls.Contains(SelectControl))
                SelectionMade();
        }
    }
}
