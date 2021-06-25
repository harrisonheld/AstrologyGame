using System.Collections.Generic;

namespace AstrologyGame.Entities.Components
{
    public class BodyPlan : EntityComponent
    {
        private Dictionary<Slot, Entity> slotDict  = new Dictionary<Slot, Entity>();

        public void Equip(Entity toEquip)
        {
            Slot slot = toEquip.GetComponent<Equippable>().Slot;
            slotDict[slot] = toEquip;
        }
        public void Unequip(Entity toUnequip)
        {
            // get the appropriate slot
            Slot slot = toUnequip.GetComponent<Equippable>().Slot;

            // remove the equipment from the SlotDict
            slotDict[slot] = null;
        }

        public void AddSlot(Slot slotToAdd)
        {
            slotDict.Add(slotToAdd, null);
        }

        public bool HasSlot(Slot slotToCheckFor)
        {
            return slotDict.ContainsKey(slotToCheckFor);
        }
        public bool HasEquipped(Entity equippable)
        {
            return slotDict.ContainsValue(equippable);
        }
    }
}
