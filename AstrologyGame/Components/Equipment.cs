using System.Collections.Generic;
using AstrologyGame.Entities;

namespace AstrologyGame.Components
{
    public class Equipment : Component
    {
        public SerializableDictionary<Slot, Entity> SlotDict  = new SerializableDictionary<Slot, Entity>();
    }
}
