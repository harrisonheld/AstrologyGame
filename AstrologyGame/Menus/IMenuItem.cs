using System;
using System.Collections.Generic;
using System.Text;

namespace AstrologyGame.Menus
{
    /// <summary>
    /// An struct containing an item to be listed in a menu.
    /// </summary>
    public struct MenuItem
    {
        public object Item { get; set; }
        public string Text { get; set; }
    }
}
