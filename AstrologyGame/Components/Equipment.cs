using System.Collections.Generic;
using AstrologyGame.Entities;

namespace AstrologyGame.Components
{
    public class Equipment : Component
    {
        public Dictionary<Slot, Entity> SlotDict  = new Dictionary<Slot, Entity>();
    }
}
