using System.Collections.Generic;

using AstrologyGame.Entities.ComponentInteractions;
using AstrologyGame.MapData;

namespace AstrologyGame.Entities
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
                    Perform = (Entity e) => BePickedUp(e),
                    Condition = (Entity e) => BePickedUpPredicate(e)
                },
                new Interaction()
                {
                    Name = "Drop",
                    Perform = (Entity e) => BeDropped(e),
                    Condition = (Entity e) => BeDroppedPredicate(e)
                }
            };
        }

        private void BePickedUp(Entity pickerUpper)
        {
            Inventory inv = pickerUpper.GetComponent<Inventory>();
            inv.AddEntity(ParentEntity);
            ContainingInventory = inv;
            OnGround = false;

            // remove un needed components since the item has been picked up
            ParentEntity.RemoveComponentsOfType<Position>();
        }
        private bool BePickedUpPredicate(Entity pickerUpper)
        {
            // can only be picked up if its on the ground
            return OnGround;
        }

        private void BeDropped(Entity dropper)
        {
            // clone the dropping entity's position
            Position dropperPos = dropper.GetComponent<Position>();
            Position thisPos = new Position();
            thisPos.Pos = dropperPos.Pos;
            ParentEntity.AddComponent(thisPos);

            // remove the entity from the inventory that contains it
            ContainingInventory.RemoveEntity(ParentEntity);

            // add it to the zone if the zone does not already contain it
            if (!Zone.Entities.Contains(ParentEntity))
                Zone.AddEntity(ParentEntity);

            ContainingInventory = null;
            OnGround = true;
        }
        private bool BeDroppedPredicate(Entity dropper)
        {
            // can only be dropped if it is in an inventory (ie, not on the ground)
            return !OnGround;
        }
    }
}
