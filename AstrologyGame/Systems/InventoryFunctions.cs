using System;
using System.Collections.Generic;
using System.Text;

using AstrologyGame.Entities;
using AstrologyGame.Entities.Components;

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

        public static void Equip(Equippable equippable, Entity equipper)
        {
            // put the equippable entity into the equipment slot
            Entity equippableEntity = equippable.Owner;
            Slot slot = equippable.Slot;

            Equipment equipment = equipper.GetComponent<Equipment>();

            // if there is already an equippable in this slot, unequip it
            if (equipment.SlotDict[slot] != null)
                UnEquip(equipment.SlotDict[slot].GetComponent<Equippable>());

            // put the equippable in the slot
            equipment.SlotDict[slot] = equippableEntity;
            equippable.ContainingEquipment = equipment;

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
        public static bool CanEquip(Equippable equippable, Entity equipper)
        {
            // if the equipper does not have an Equipment component
            if(!equipper.HasComponent<Equipment>() )
                return false;

            // if the Equipment does not have the appropriate slot
            Equipment equipment = equipper.GetComponent<Equipment>();
            if (!equipment.SlotDict.ContainsKey(equippable.Slot))
                return false;

            return true;
        }

        public static void UnEquip(Equippable equippable)
        {
            Slot slot = equippable.Slot;
            Equipment containingEquipment = equippable.ContainingEquipment;
            Entity equippableEntity = equippable.Owner;

            containingEquipment.SlotDict[slot] = null;
            Entity entityWhoHadThisEquipped = containingEquipment.Owner;

            // put this back in the inventory if this has one, otherwise drop it
            if (entityWhoHadThisEquipped.HasComponent<Inventory>())
                PutInInventory(equippableEntity.GetComponent<Item>(), entityWhoHadThisEquipped.GetComponent<Inventory>());
            else
            {
                Position dropperPos = entityWhoHadThisEquipped.GetComponent<Position>();
                Position thisPos = new Position();
                thisPos.Pos = dropperPos.Pos;
                equippableEntity.AddComponent(thisPos);
            }
                
        }
    }
}
