using System.Collections.Generic;

using System.Text;

using AstrologyGame.Entities;

namespace AstrologyGame
{
    class InventoryMenu : SelectMenu
    {
        private Entity container; // the object whose inventory we are examining
        private List<Entity> inventoryContents
        {
            get
            {
                return container.GetComponent<Inventory>().Contents;
            }
        }

        public InventoryMenu(Entity _container)
        {
            container = _container;
            selectionCount = inventoryContents.Count;

            Refresh();
        }

        public override void Refresh()
        {
            // add all the item names to the text
            StringBuilder sb = new StringBuilder();
            sb.Append($"[{container.GetComponent<Display>().name}]\n");
            foreach (Entity e in inventoryContents)
            {
                sb.Append(e.GetComponent<Display>().name);

                if(container.HasComponent<Equipment>())
                {
                    Equipment equipment = container.GetComponent<Equipment>();
                    Equippable equippable = e.GetComponent<Equippable>();
                    if (equipment != null && equippable != null)
                    {
                        if (equipment.HasEquipped(e))
                        {
                            string slot = equippable.slot.ToString();
                            sb.Append($" ({slot})");
                        }
                    }
                }

                // add item count if its more than 1
                int count = e.GetComponent<Item>().count;
                if (count > 1)
                    sb.Append($" (x{count})");

                sb.Append("\n");
            }

            selectionCount = inventoryContents.Count;
            Text = sb.ToString();
            base.Refresh();
        }

        public override void SelectionMade()
        {
            InteractionMenu menu = new InteractionMenu(inventoryContents[selectedIndex]);
            menu.Position = cursorCoords;

            Game1.OpenMenu(menu);
        }
    }
}
