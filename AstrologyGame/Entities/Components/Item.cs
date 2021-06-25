using System.Collections.Generic;

using AstrologyGame.Entities.Components;
using AstrologyGame.Entities.Systems;
using AstrologyGame.MapData;

namespace AstrologyGame.Entities.Components
{
    public class Item : EntityComponent
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
                    Perform = (Entity e) => InventorySystem.PutEntityInInventory(ParentEntity, e.GetComponent<Inventory>()),
                    Condition = (Entity e) => BePickedUpPredicate(e)
                },
                new Interaction()
                {
                    Name = "Drop",
                    Perform = (Entity e) => InventorySystem.DropEntityFromInventory(ParentEntity),
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
