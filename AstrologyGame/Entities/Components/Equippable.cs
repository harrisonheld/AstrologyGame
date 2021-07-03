using System;
using System.Collections.Generic;
using System.Text;

using AstrologyGame.Systems;

namespace AstrologyGame.Entities.Components
{
    public class Equippable : Component
    {
        public Equipment ContainingEquipment { get; set; }
        public Slot Slot { get; set; }

        public Equippable()
        {
            interactions = new List<Interaction>()
            {
                new Interaction()
                {
                    Name = "Equip",
                    Perform = (Entity equipper) => InventoryFunctions.Equip(this, equipper),
                    Condition = (Entity equipper) => InventoryFunctions.CanEquip(this, equipper)
                }
            };
        }
    }
}
