using System.Collections.Generic;

using System.Text;

using AstrologyGame.Entities;
using AstrologyGame.Components;
using AstrologyGame.MapData;

namespace AstrologyGame.Menus
{
    public class ItemMenu : SelectMenu
    {
        private Entity interactor;

        public ItemMenu(Entity interactor, List<Entity> entities)
        {
            this.interactor = interactor;

            foreach(Entity entity in entities)
            {
                // create the menu item
                MenuItem menuItem = new MenuItem();
                menuItem.Item = entity;
                menuItem.Text = entity.ToString(); //TODO: get name from display component
                items.Add(menuItem);
            }

            Refresh();
        }

        public override void HandleInput(List<Control> controls)
        {
            base.HandleInput(controls);

            // if player hits tab, get all items
            if(controls.Contains(Control.Tab))
            {
                foreach (MenuItem e in items)
                {
                    // TODO: PICKUP ALL THE ITEMS
                }

                Game1.RemoveMenu(this);
            }
        }

        public override void SelectionMade()
        {
            Entity selected = items[selectedIndex].Item as Entity;
            InteractionMenu interactionMenu = new InteractionMenu(interactor, selected);
            Game1.AddMenu(interactionMenu);
        }
    }
}
