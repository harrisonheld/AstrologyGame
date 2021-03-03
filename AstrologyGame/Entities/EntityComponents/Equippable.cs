namespace AstrologyGame.Entities
{
    public class Equippable : EntityComponent
    {
        public Slot slot;
        public Entity wearer;

        public bool IsEquipped()
        {
            // if it has a wearer, it is equipped
            return wearer != null;
        }
    }
}
