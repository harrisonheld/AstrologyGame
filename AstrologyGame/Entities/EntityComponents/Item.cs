using AstrologyGame.Entities.ComponentInteractions;

namespace AstrologyGame.Entities
{
    public class Item : EntityComponent
    {
        public int Count { get; set; } = 1;

        public Item() 
        {
            Interaction pickupInteraction = new Interaction();
            pickupInteraction.Perform = (Entity e) => BePickedUp(e);
            pickupInteraction.Name = "Pick Up";
            interactions.Add(pickupInteraction);
        }

        private void BePickedUp(Entity pickerUpper)
        {
            Inventory inv = pickerUpper.GetComponent<Inventory>();

            inv.AddEntity(ParentEntity);
            ParentEntity.AddComponent(new InInventory() { ContainingInventory = inv });

            // remove un needed components since the item has been picked up
            ParentEntity.RemoveComponentsOfType<Position>();
        }
    }
}
