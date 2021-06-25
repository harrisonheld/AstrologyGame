using System;
using System.Collections.Generic;
using System.Text;

using AstrologyGame.Entities.Components;

using AstrologyGame.MapData;

namespace AstrologyGame.Entities.Systems
{
    public static class InventorySystem
    {
        public static void PutEntityInInventory(Entity entity, Inventory inventory)
        {
            inventory.AddEntity(entity);

            Item itemComp = entity.GetComponent<Item>();
            itemComp.ContainingInventory = inventory;
            itemComp.OnGround = false;

            entity.RemoveComponentsOfType<Position>();
            Zone.RemoveEntity(entity);
        }

        public static void DropEntityFromInventory(Entity toBeDropped)
        {
            Item itemComp = toBeDropped.GetComponent<Item>();
            Inventory inventory = itemComp.ContainingInventory;

            // clone the dropping entity's position
            Position dropperPos = inventory.ParentEntity.GetComponent<Position>();
            Position thisPos = new Position();
            thisPos.Pos = dropperPos.Pos;
            toBeDropped.AddComponent(thisPos);

            // remove the entity from the inventory that contains it
            inventory.RemoveEntity(toBeDropped);

            // add it to the zone if the zone does not already contain it
            if (!Zone.Entities.Contains(toBeDropped))
                Zone.AddEntity(toBeDropped);

            itemComp.ContainingInventory = null;
            itemComp.OnGround = true;
        }
    }
}
