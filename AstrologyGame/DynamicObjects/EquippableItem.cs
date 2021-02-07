using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using AstrologyGame.MapData;

namespace AstrologyGame.DynamicObjects
{
    public abstract class EquippableItem : Item, IEquippable
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
                    Interaction.Equip
                };
            }
        }

        public EquippableItem()
        {

        }

        public void BeEquipped(DynamicObject equipper)
        {
            // the equipper picks up the item if it doesnt already have it
            this.BeGot(equipper);

            // if the equipper has the appropriate slot
            if(equipper.SlotDict.ContainsKey(this.EquipSlot))
            {
                equipper.SlotDict[EquipSlot] = this;
            }
            else
            {
                Debug.WriteLine("You don't have the slot '{0}'.", EquipSlot);
            }
        }
    }
}
