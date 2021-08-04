using System;
using System.Collections.Generic;
using System.Text;

using AstrologyGame.Systems;
using AstrologyGame.Entities;

namespace AstrologyGame.Components
{
    public class Equippable : Component
    {
        public SlotType SlotType { get; set; }

        public Equippable()
        {
            interactions = new List<Interaction>()
            {
                new Interaction()
                {
                    Name = "Equip",
                    Perform = (Entity equipper) => InventoryFunctions.Equip(Owner, equipper),
                    Condition = (Entity equipper) => InventoryFunctions.CanEquip(Owner, equipper)
                }
            };
        }
    }
}
