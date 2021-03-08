namespace AstrologyGame.Entities
{
    public class Equippable : EntityComponent
    {
        public Slot Slot { get; set; }
        public Entity Wearer { get; set; }

        public bool IsEquipped()
        {
            // if it has a wearer, it is equipped
            return Wearer != null;
        }
    }
}
