using System.Collections.Generic;

using AstrologyGame.MapData;

namespace AstrologyGame.Entities
{
    public class Inventory : EntityComponent
    {
        public bool OtherEntitiesCanOpen { get; set; } = false; // can other entities open this inventory?
        public List<Entity> Contents { get; set; } = new List<Entity>();

        public override bool FireEvent(ComponentEvent cEvent)
        {
            // fire the event for all the items in this inventory.
            // This is recursive... not good...
            foreach(Entity e in Contents)
            {
                e.FireEvent(cEvent);
            }

            switch (cEvent.EventId)
            {
                case EventId.OpenInventory:
                    InventoryMenu iMenu = new InventoryMenu(ParentEntity);
                    Game1.OpenMenu(iMenu);
                    return true;
                    
                case EventId.RemoveItem:
                    Entity entityToRemove = cEvent[ParameterId.Target] as Entity;
                    Stack<Inventory> stack = new Stack<Inventory>();
                    stack.Push(this);

                    while (stack.Count > 0)
                    {
                        Inventory node = stack.Pop();
                        if (node.RemoveEntity(entityToRemove))
                            return true;

                        foreach (Entity child in node.Contents)
                        {
                            // get the child's inventory
                            Inventory childInv = child.GetComponent<Inventory>();
                            // if child has no inventory, continue
                            if (childInv == null)
                                continue;

                            if (childInv.RemoveEntity(entityToRemove))
                                return true;
                            else
                                stack.Push(childInv);
                        }
                    }

                    return false; // the removal failed. return false

                default:
                    return false;
            }
        }
        /// <summary>
        /// Add an item to this inventory and remove it from where it came from.
        /// </summary>
        public void AddEntity(Entity entityToAdd)
        {
            Zone.RemoveObject(entityToAdd); // remove it from the zone
            Contents.Add(entityToAdd); // and put it in this inventory   
        }

        public bool RemoveEntity(Entity entityToRemove)
        {
            return Contents.Remove(entityToRemove);
        }
    }
}
