using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AstrologyGame.Menus
{
    public class Menu
    {
        private Rectangle rect;
        public Rectangle Rectangle 
        { 
            get
            {
                return rect;
            }
        }
        public OrderedPair Position
        {
            get
            {
                return new OrderedPair(rect.X, rect.Y);
            }
            set
            {
                rect.X = (int)value.X;
                rect.Y = (int)value.Y;
            }
        }
        public OrderedPair Size
        {
            get
            {
                return new OrderedPair(rect.Width, rect.Height);
            }
            set
            {
                rect.Width = (int)value.X;
                rect.Height = (int)value.Y;
            }
        }

        public string Text { get; set; } = "";

        // should opening this menu cause the GameState to change to InMenu
        public virtual bool TakesInput { get; } = true;

        public Menu()
        {
            rect = new Rectangle(5, 5, 18*64, 9*64); // an arbitrary size
        }
        public virtual void Refresh()
        {
            // do nothing. menus that are procedurally generated (like inventories) should override this to refresh their text if data changes
        }

        public void Center()
        {
            // change the position so the menu is in the center of the screen.
            throw new NotImplementedException();
        }

        public virtual void HandleInput(List<Control> controls)
        {
            if (controls.Contains(Control.Back))
                Game1.RemoveMenu(this);
        }
    }
}
