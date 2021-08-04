using System.Collections.Generic;

using AstrologyGame.MapData;
using AstrologyGame.Entities;

namespace AstrologyGame.Components
{
    public class Inventory : Component
    {
        public bool OtherEntitiesCanOpen { get; set; } = false; // can other entities open this inventory?
        public List<Entity> Contents { get; set; } = new List<Entity>();
        public List<Slot> Slots { get; set; } = new List<Slot>();
    }

    public class Slot
    {
        public SlotType Type = SlotType.None; // the type of slot this is
        public Entity Entity; // the entity this slot contains
    }

    public enum SlotType
    { 
        None,
        Head,
        Body,
        Legs,

        // planned
        Socket, // for adding gems?
        Battery, // for adding batteries?
    }
}
