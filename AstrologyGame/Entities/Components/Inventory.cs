using System.Collections.Generic;

using AstrologyGame.MapData;
using AstrologyGame.Entities;

namespace AstrologyGame.Entities.Components
{
    public class Inventory : EntityComponent
    {
        public bool OtherEntitiesCanOpen { get; set; } = false; // can other entities open this inventory?
        public List<Entity> Contents { get; set; } = new List<Entity>();

        /// <summary>Add an item to this inventory.</summary>
        public void AddEntity(Entity entityToAdd)
        {
            Contents.Add(entityToAdd);
        }
        public void RemoveEntity(Entity entityToRemove)
        {
            Contents.Remove(entityToRemove);
        }

        /*
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
        */
    }
}
