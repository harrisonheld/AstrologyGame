using System.Collections.Generic;

namespace AstrologyGame.Entities.Components
{
    public class Equipment : Component
    {
        public Dictionary<Slot, Entity> SlotDict  = new Dictionary<Slot, Entity>();
    }
}
