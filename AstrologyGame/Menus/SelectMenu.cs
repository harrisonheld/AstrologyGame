using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace AstrologyGame.Menus
{
    // a menu where you can select one thing in a list
    public abstract class SelectMenu : Menu
    {
        public bool DrawCursor = true; // should the menu draw a cursor next to the selected item?

        protected int selectedIndex = 0;
        protected int selectionCount = 1; // how many things are there to select?

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

            // increment or decrement index
            if (controls.Contains(IncrementControl))
            {
                selectedIndex++;
            }
            else if (controls.Contains(DecrementControl))
            {
                selectedIndex--;
            }
            // clamp index in case it went negative or went too high
            ClampSelection();

            // make a selection if user hits select, and there is something to select
            if (controls.Contains(SelectControl) && selectionCount != 0)
                SelectionMade();
        }
        private void ClampSelection()
        {
            int before = selectedIndex; // we will check if the selected index changed

            if (selectionCount != 0)
                selectedIndex = Math.Clamp(selectedIndex, 0, selectionCount - 1);

            // the selectedIndex changed
            if (before != selectedIndex)
            {
                SelectionChanged();
            }
        }
        public override void Refresh()
        {
            ClampSelection();
            base.Refresh();
        }
        public override void Draw()
        {
            base.Draw();

            if(DrawCursor && selectionCount != 0)
            {
                CalculateCursorCoords();
                Utility.SpriteBatch.Draw(cursorTexture, new Rectangle(cursorCoords.X, cursorCoords.Y, 20, 20), Color.White);
            }
        }

        protected void CalculateCursorCoords()
        {
            int x = Position.X + 240;
            int y = Position.Y + (selectedIndex * 22) + 2;
            cursorCoords = new OrderedPair(x, y);
        }
    }
}
