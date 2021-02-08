using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using AstrologyGame.MapData;

namespace AstrologyGame.Entities
{
    public abstract class EquippableItem : Item, IEquipment
    {
        public Slot EquipSlot { get; set; }
        List<Interaction> IInteractable.Interactions
        {
            get
            {
                return new List<Interaction>()
                {
                    Interaction.Get,
                    Interaction.Drop,
                    Interaction.Equip,
                    Interaction.DeEquip,
                };
            }
        }

        public EquippableItem()
        {

        }

        public void BeEquipped(Entity equipper)
        {
            equipper.Equip(this);
        }
        public void BeDeEquipped(Entity deEquipper)
        {
            deEquipper.TryDeEquip(this);
        }
    }

    public class CopperArmor : EquippableItem
    {
        public CopperArmor()
        {
            EquipSlot = Slot.Body;
            Name = "Copper Armor";
        }
    }

    public class CopperLeggings : EquippableItem
    { 
        public CopperLeggings()
        {
            EquipSlot = Slot.Legs;
            Name = "Copper Leggings";
        }
    }
}
