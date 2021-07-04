using System.Collections.Generic;

using AstrologyGame.MapData;
using AstrologyGame.Entities;

namespace AstrologyGame.Components
{
    public class Inventory : Component
    {
        public bool OtherEntitiesCanOpen { get; set; } = false; // can other entities open this inventory?
        public List<Entity> Contents { get; set; } = new List<Entity>();
    }
}
