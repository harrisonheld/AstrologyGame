using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using System.Xml;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using AstrologyGame.MapData;


namespace AstrologyGame.Entities
{
    public abstract class Entity
    {
        public bool ShouldRender { get; set; } = true; // should we try to render this?

        public string TextureName { get; set; } = "";
        public string Name { get; set; } // is the thing named, ie Orion, Harrison, Spider, Demon for creatures or Sand, Stone, for tiles
        public string Lore { get; set; } // the item description, creature description, etc.
        // x and y position of the object
        public int X { get; set; }
        public int Y { get; set; }
        public int MaxHealth { get; set; }
        public int Health { get; set; }
        public bool Solid { get; set; } // can you walk through the thing
        public Color Color { get; set; } = Color.White;
        // dictionary that maps slots to equipment
        protected Dictionary<Slot, IEquipment> slotDict { get; set; } = new Dictionary<Slot, IEquipment>();

        // the 4 stats
        public PrimaryAttributes Stats { get; set; } = new PrimaryAttributes
        {
            Vigor = 10,
            Prowess = 10,
            Faith = 10,
            Some4thThing = 10
        };

        public List<Entity> Children { get; set; } = new List<Entity>() { };

        public Entity()
        {

        }

        public void Equip(IEquipment toEquip)
        {
            // if we don't have the appropriate equip slot
            if (!slotDict.ContainsKey(toEquip.EquipSlot))
            {
                Utility.Log("You can't equip this item.");
                return;
            }
            // if it's not in our inventory, add it
            if (!Children.Contains(toEquip as Entity))
            {
                Children.Add(toEquip as Entity);
            }

            slotDict[toEquip.EquipSlot] = toEquip;
        }
        public void TryDeEquip(IEquipment toDeEquip)
        {
            Slot slot = toDeEquip.EquipSlot;

            // if we have it equipped
            if(slotDict[slot] == toDeEquip)
            {
                // remove it from the SlotDict
                slotDict[slot] = null;
            }
        }
        public bool HasEquipped(IEquipment equippable)
        {
            return slotDict.ContainsValue(equippable);
        }

        /// <summary>
        /// Using this Entity as a root node, remove the given Entity from any of it's descendants.
        /// </summary>
        /// <param name="toRemove">The object to remove from the tree.</param>
        /// <returns>True if the removal was successful, False if the given object wasn't in the tree to begin with.</returns>
        public bool RemoveFromDescendants(Entity toRemove)
        {
            Stack<Entity> stack = new Stack<Entity>();
            stack.Push(this);

            while(stack.Count > 0)
            {
                Entity node = stack.Pop();

                if (node.Children.Remove(toRemove))
                    return true;

                foreach(Entity child in node.Children)
                {
                    if (child.Children.Remove(toRemove))
                        return true;
                    else
                        stack.Push(child);
                }
            }

            return false; 
        }

        public void Interact(Interaction interaction, Entity interactor)
        {
            // TODO: this code sucks
            switch (interaction)
            {
                case Interaction.Attack:
                    if(this is IAttackable)
                    {
                        (this as IAttackable).BeAttacked(interactor);
                        return;
                    }
                    break;

                case Interaction.Get:
                    if(this is IGettable)
                    {
                        (this as IGettable).BeGot(interactor);
                        return;
                    }
                    break;

                case Interaction.Drop:
                    if(this is IDroppable)
                    {
                        (this as IDroppable).BeDropped(interactor);
                        return;
                    }
                    break; 

                case Interaction.Open:
                    if (this is IOpenable)
                    {
                        (this as IOpenable).BeOpened(interactor);
                        return;
                    }
                    break;

                case Interaction.Read:
                    if (this is IReadable)
                    {
                        (this as IReadable).BeRead(interactor);
                        return;
                    }
                    break;

                case Interaction.Equip:
                    if (this is IEquipment)
                    {
                        (this as IEquipment).BeEquipped(interactor);
                        return;
                    }
                    break;

                case Interaction.DeEquip:
                    if( this is IEquipment)
                    {
                        (this as IEquipment).BeDeEquipped(interactor);
                        return;
                    }
                    break;
            }

            Debug.WriteLine("Could not do interaction type {0}.", interaction);
        }

        public virtual void AnimationTurn()
        {
            // things like changing the texture, changing color, etc.
        }

        public virtual void Draw()
        {
            Utility.DrawEntity(this, this.X, this.Y);
        }
    }
}