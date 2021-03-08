using System.Collections.Generic;

namespace AstrologyGame.Entities
{
    public class Equipment : EntityComponent
    {
        private Dictionary<Slot, Entity> slotDict  = new Dictionary<Slot, Entity>();

        private void Equip(Entity toEquip)
        {
            Slot slot = toEquip.GetComponent<Equippable>().Slot;
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

        private bool TryUnequip(Entity toUnequip)
        {
            Slot slot = toUnequip.GetComponent<Equippable>().Slot;

            // if we have it equipped
            if (slotDict[slot] == toUnequip)
            {
                // remove it from the SlotDict
                slotDict[slot] = null;
                return true;
            }

            return false; // unequip failed
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
