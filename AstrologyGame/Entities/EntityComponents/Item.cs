using AstrologyGame.Entities.ComponentInteractions;

namespace AstrologyGame.Entities
{
    public class Item : EntityComponent
    {
        public int Count { get; set; } = 1;
        public bool OnGround { get; set; } = true;
        public Inventory ContainingInventory { get; set; }

        public Item() 
        {
            Interaction pickupInteraction = new Interaction();
            pickupInteraction.Perform = (Entity e) => BePickedUp(e);
            pickupInteraction.Condition = (Entity e) => BePickedUpPredicate(e);
            pickupInteraction.Name = "Pick Up";
            interactions.Add(pickupInteraction);

            Interaction dropInteraction = new Interaction();
            dropInteraction.Perform = (Entity e) => BeDropped(e);
            dropInteraction.Condition = (Entity e) => BeDroppedPredicate(e);
            dropInteraction.Name = "Drop";
            interactions.Add(dropInteraction);
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

            // remove the entity from the inventory that contains it
            ContainingInventory.RemoveEntity(ParentEntity);

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
