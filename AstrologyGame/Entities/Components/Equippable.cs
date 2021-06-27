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
                    Perform = (Entity equipper) => InventorySystem.Equip(this, equipper),
                    Condition = (Entity equipper) => InventorySystem.CanEquip(this, equipper)
                }
            };
        }
    }
}
