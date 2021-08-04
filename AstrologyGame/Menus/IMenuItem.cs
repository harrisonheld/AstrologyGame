using System;
using System.Collections.Generic;
using System.Text;

namespace AstrologyGame.Menus
{
    /// <summary>
    /// An interface representing the ability to be listed and selected in a menu.
    /// </summary>
    public interface IMenuItem
    {
        public string GetText() { return "[THIS ITEM DOES NOT IMPLEMENT GetText()]"; }
    }
}
