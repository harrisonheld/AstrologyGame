using System.Collections.Generic;

using System.Text;

using AstrologyGame.Entities;
using AstrologyGame.Entities.Components;
using AstrologyGame.MapData;

namespace AstrologyGame.Menus
{
    public class ItemMenu : SelectMenu
    {
        private List<Entity> entities;

        public ItemMenu(List<Entity> _entities)
        {
            this.entities = _entities;
            Refresh();
        }
        public override void Refresh()
        {
            // add all the item names to the text
            StringBuilder sb = new StringBuilder();

            foreach (Entity entity in entities)
            {
                sb.Append(entity.GetComponent<Display>().Name);

                if (entity.HasComponent<Equippable>())
                {
                    Equippable equippable = entity.GetComponent<Equippable>();

                    if (equippable.IsEquipped())
                    {
                        string slot = equippable.Slot.ToString();
                        sb.Append($" (Worn on {slot})");
                    }
                }

                // add item count if its more than 1
                int count = entity.GetComponent<Item>().Count;
                if (count > 1)
                    sb.Append($" (x{count})");

                sb.Append("\n");
            }

            selectionCount = entities.Count;
            Text = sb.ToString();
            base.Refresh();
        }

        public override void HandleInput(List<Control> controls)
        {
            base.HandleInput(controls);

            // if player hits tab, get all items
            if(controls.Contains(Control.Tab))
            {
                foreach (Entity e in entities)
                {
                    // TODO: PICKUP ALL THE ITEMS
                }

                Game1.CloseMenu(this);
            }
        }

        public override void SelectionMade()
        {
            Entity selected = entities[selectedIndex];
            InteractionMenu interactionMenu = new InteractionMenu(Zone.Player, selected);
            // put the menu at the place of the cursor
            interactionMenu.Position = cursorCoords;
            Game1.OpenMenu(interactionMenu);
        }
    }
}
