using System.Collections.Generic;

namespace AstrologyGame.Entities
{
    public class ComponentEvent
    {
        
    }

    /// <summary>Read something such as a sign or a book.</summary>
    public class CERead : ComponentEvent
    {
        
    }

    /// <summary>Opens a GetMenu for this entity's inventory.</summary>
    public class CEOpenItemMenu : ComponentEvent
    {

    }

    /// <summary>Add an item to the Inventory.</summary>
    public class CEPickupItem : ComponentEvent
    {
        public Entity EntityToPickup { get; set; }
    }

    /// <summary>Drop an item from the Inventory.</summary>
    public class CEDropItem : ComponentEvent
    {
        public Entity EntityToDrop { get; set; }
    }
}
