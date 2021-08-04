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
            items.AddRange(entities);
            Refresh();
        }

        public override void HandleInput(List<Control> controls)
        {
            base.HandleInput(controls);

            // if player hits tab, get all items
            if(controls.Contains(Control.Tab))
            {
                foreach (Entity e in items)
                {
                    // TODO: PICKUP ALL THE ITEMS
                }

                Game1.RemoveMenu(this);
            }
        }

        public override void SelectionMade()
        {
            Entity selected = items[selectedIndex] as Entity;
            InteractionMenu interactionMenu = new InteractionMenu(interactor, selected);
            Game1.AddMenu(interactionMenu);
        }
    }
}
