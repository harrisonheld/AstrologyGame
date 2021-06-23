using System.Collections.Generic;

using System.Text;

using AstrologyGame.Entities;
using AstrologyGame.MapData;

namespace AstrologyGame
{
    class ItemMenu : SelectMenu
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

                    if (equippable.IsEquipped());
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
            Entity entityToGet = entities[selectedIndex];

            // if the player already has this item, do nothing and return
            if (Zone.Player.GetComponent<Inventory>().Contents.Contains(entityToGet))
                return;

            // TODO: PICKIP THE ITEM
            bool success = false;

            // if the players gets the item, remove it from the list and refresh the text. This will also fix the cursor if it needs to move.
            if(success)
            {
                entities.Remove(entityToGet);
                Refresh();
            }
        }
    }
}
