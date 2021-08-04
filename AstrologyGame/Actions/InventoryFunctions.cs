using System;
using System.Collections.Generic;
using System.Text;

using AstrologyGame.Entities;
using AstrologyGame.Components;

using AstrologyGame.MapData;

namespace AstrologyGame.Systems
{
    public static class InventoryFunctions
    {
        public static void PutInInventory(Item item, Inventory inventory)
        {
            Entity entity = item.Owner;

            inventory.Contents.Add(entity);

            Item itemComp = entity.GetComponent<Item>();
            itemComp.ContainingInventory = inventory;
            itemComp.OnGround = false;

            entity.RemoveComponentsOfType<Position>();
            Zone.RemoveEntity(entity);
        }
        public static void DropFromInventory(Item itemComp)
        {
            Entity entityToBeDropped = itemComp.Owner;
            Inventory inventory = itemComp.ContainingInventory;

            // clone the dropping entity's position
            Position dropperPos = inventory.Owner.GetComponent<Position>();
            Position thisPos = new Position();
            thisPos.Pos = dropperPos.Pos;
            entityToBeDropped.AddComponent(thisPos);

            // remove the entity from the inventory that contains it
            inventory.Contents.Remove(entityToBeDropped);

            // add it to the zone if the zone does not already contain it
            if (!Zone.Entities.Contains(entityToBeDropped))
                Zone.AddEntity(entityToBeDropped);

            itemComp.ContainingInventory = null;
            itemComp.OnGround = true;
        }

        public static void Equip(Entity equippableEntity, Entity equipper, Slot preferredSlot = null)
        {
            // put the equippable entity into the equipment slot
            Equippable equippableComp = equippableEntity.GetComponent<Equippable>();
            SlotType slotType = equippableComp.SlotType;

            Inventory inventory = equipper.GetComponent<Inventory>();

            if (inventory.Contents.Contains(equippableEntity))
                PutInInventory(equippableEntity.GetComponent<Item>(), inventory);

            // if a preferred slot was picked
            if(preferredSlot != null)
            {
                /*// if something is already in it, remove it
                if (preferredSlot.Entity != null)
                    UnEquip(preferredSlot.Entity);*/

                preferredSlot.Entity = equippableEntity;
            }
            else
            {
                // find the first appropriate slot
            }

            Inventory owningInventory = equippableEntity.GetComponent<Item>().ContainingInventory;
            if (owningInventory == null)
            {
                // remove position so the item isnt on the ground anymore
                equippableEntity.RemoveComponentsOfType<Position>();
            }
            else
            {
                // remove the equipment from an inventory if its in one
                owningInventory.Contents.Remove(equippableEntity);
            }
        }
        public static bool CanEquip(Entity toEquip, Entity equipper)
        {
            // check for necessary components
            if(!(equipper.HasComponent<Inventory>() && toEquip.HasComponent<Equippable>()))
                return false;

            Inventory inventory = equipper.GetComponent<Inventory>();
            Equippable equippableComp = toEquip.GetComponent<Equippable>();

            // if the Equipment does not have the appropriate slot
            foreach (Slot slot in inventory.Slots)
            {
                if (slot.Type == equippableComp.SlotType)
                    return true;
            }

            return false;
        }
    }
}
