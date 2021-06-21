using System.Collections.Generic;

using AstrologyGame.MapData;
using AstrologyGame.Entities;

namespace AstrologyGame.Entities
{
    public class Inventory : EntityComponent
    {
        public bool OtherEntitiesCanOpen { get; set; } = false; // can other entities open this inventory?
        public List<Entity> Contents { get; set; } = new List<Entity>();

        public override bool FireEvent(ComponentEvent componentEvent)
        {
            switch (componentEvent)
            {
                // open the item menu for the player
                case CEOpenItemMenu openItemMenuEvent:
                    ItemMenu itemMenu = new ItemMenu(Contents);
                    Game1.OpenMenu(itemMenu);
                    return true;

                case CEDropItem dropItemEvent:
                    bool success = DropEntity(dropItemEvent.EntityToDrop);
                    return success;

                case CEPickupItem addItemEvent:
                    AddEntity(addItemEvent.EntityToPickup);
                    return true;

                default:
                    return false;
            }
        }
        /// <summary>Add an item to this inventory.</summary>
        private void AddEntity(Entity entityToAdd)
        {
            // remove the position component if it has one, as it's in an inventory now
            entityToAdd.RemoveComponentsOfType<Position>();
            entityToAdd.AddComponent(new InInventory() { ContainingInventory = this });

            // put a reference to the entity in our list of items
            Contents.Add(entityToAdd);

            // add the entity to the zone in case it's not already in the main list of entities
            Zone.AddEntity(entityToAdd);
        }

        public bool DropEntity(Entity entityToDrop)
        {
            // if its not in this inventory, the drop fails. return false
            if (!Contents.Contains(entityToDrop))
                return false;

            // if this container is on the world map, drop the item on the ground
            if(ParentEntity.HasComponent<Position>())
            {
                // because the item is being dropped on the ground, it should no longer have an InInventory
                entityToDrop.RemoveComponentsOfType<InInventory>();

                // make a shallow copy of the parent's position
                Position parentPositionComp = ParentEntity.GetComponent<Position>();
                Position itemPositionComp = new Position() { X = parentPositionComp.X, Y = parentPositionComp.Y };
                // add it to the item
                entityToDrop.AddComponent(itemPositionComp);
            }
            else // otherwise, this container is inside another container. move this item up a level
            {
                // get the grandparent inventory and add this as a child
                InInventory parentInInventory = ParentEntity.GetComponent<InInventory>();
                Inventory grandparentInventory = parentInInventory.ContainingInventory;
                grandparentInventory.AddEntity(entityToDrop);
            }

            // in any case, remove the entity from this inventory
            Contents.Remove(entityToDrop);
            return true;
        }

        // remove an item from this inventory, or any of the inventories in its children entities
        public bool RemoveEntity(Entity entityToRemove)
        {
            Stack<Inventory> stack = new Stack<Inventory>();
            stack.Push(this);

            while (stack.Count > 0)
            {
                Inventory node = stack.Pop();
                if (node.DropEntity(entityToRemove))
                    return true;

                foreach (Entity child in node.Contents)
                {
                    // get the child's inventory
                    Inventory childInv = child.GetComponent<Inventory>();
                    // if child has no inventory, continue
                    if (childInv == null)
                        continue;

                    if (childInv.DropEntity(entityToRemove))
                        return true;
                    else
                        stack.Push(childInv);
                }
            }

            return false; // the removal failed. return false
        }
    }
}
