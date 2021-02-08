using System;
using System.Collections.Generic;
using System.Text;

using AstrologyGame.MapData;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AstrologyGame.DynamicObjects
{
    public abstract class Item : DynamicObject, IGettable, IDroppable
    {
        List<Interaction> IInteractable.Interactions {
            get 
            { 
                return new List<Interaction>() 
                { 
                    Interaction.Get, 
                    Interaction.Drop
                };
            }
        }

        public int Count { get; set; } = 1;

        public Item()
        {

        }

        public void BeGot(DynamicObject getter)
        {
            if(!getter.Children.Contains(this))
            {
                Zone.RemoveObject(this);
                getter.Children.Add(this);
            }
        }
        public void BeDropped(DynamicObject dropper)
        {
            if(this is IEquipment)
            {
                dropper.TryDeEquip(this as IEquipment);
            }
            dropper.RemoveFromDescendants(this);
            this.X = dropper.X;
            this.Y = dropper.Y;
            Zone.Objects.Add(this);
        }
    }

    public class Flintlock : Item
    {
        public Flintlock()
        {
            TextureName = "flintlock";
            Name = "flintlock";
            Lore = "a gunny!!";
        }
    }
}