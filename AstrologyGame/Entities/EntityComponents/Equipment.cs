using System.Collections.Generic;

namespace AstrologyGame.Entities
{
    public class Equipment : EntityComponent
    {
        private Dictionary<Slot, Entity> slotDict  = new Dictionary<Slot, Entity>();

        public void Equip(Entity toEquip)
        {
            Slot slot = toEquip.GetComponent<Equippable>().slot;
            // if we don't have the appropriate equip slot
            if (!slotDict.ContainsKey(slot))
            {
                Utility.Log("You can't equip this item.");
                return;
            }
            // if it's not in our inventory, add it
            Inventory inv = ParentEntity.GetComponent<Inventory>();
            if (!inv.Contents.Contains(toEquip))
            {
                inv.Contents.Add(toEquip);
            }

            slotDict[slot] = toEquip;
        }

        public void TryDeEquip(Entity toDeEquip)
        {
            Slot slot = toDeEquip.GetComponent<Equippable>().slot;

            // if we have it equipped
            if (slotDict[slot] == toDeEquip)
            {
                // remove it from the SlotDict
                slotDict[slot] = null;
            }
        }

        public bool HasEquipped(Entity equippable)
        {
            return slotDict.ContainsValue(equippable);
        }

        public void AddSlot(Slot slotToAdd)
        {
            slotDict.Add(slotToAdd, null);
        }
    }
}
