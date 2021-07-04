using System.Collections.Generic;

using AstrologyGame.Entities;
using AstrologyGame.Systems;

namespace AstrologyGame.Components
{
    public class Item : Component
    {
        public int Count { get; set; } = 1;
        public bool OnGround { get; set; } = true;
        public Inventory ContainingInventory { get; set; }

        public Item()
        {
            interactions = new List<Interaction>()
            {
                new Interaction()
                {
                    Name = "Pick Up",
                    Perform = (Entity e) => InventoryFunctions.PutInInventory(this, e.GetComponent<Inventory>()),
                    Condition = (Entity e) => BePickedUpPredicate(e)
                },
                new Interaction()
                {
                    Name = "Drop",
                    Perform = (Entity e) => InventoryFunctions.DropFromInventory(this),
                    Condition = (Entity e) => BeDroppedPredicate(e)
                },
            };
        }

        private bool BePickedUpPredicate(Entity pickerUpper)
        {
            // can only be picked up if its on the ground
            return OnGround;
        }

        private bool BeDroppedPredicate(Entity dropper)
        {
            // can only be dropped if it is in an inventory (ie, not on the ground)
            return !OnGround;
        }
    }
}
