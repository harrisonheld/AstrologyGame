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


namespace AstrologyGame.DynamicObjects
{
    public abstract class DynamicObject
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
        public Dictionary<Slot, IEquippable> SlotDict { get; set; } = new Dictionary<Slot, IEquippable>();

        // the 4 stats
        public DynamicObjectStats Stats { get; set; } = new DynamicObjectStats
        {
            Vigor = 0,
            Prowess = 0,
            Faith = 0,
            Some4thThing = 0
        };

        public List<DynamicObject> Children { get; set; } = new List<DynamicObject>() { };

        public DynamicObject()
        {

        }

        public bool HasEquipped(IEquippable equippable)
        {
            return SlotDict.ContainsValue(equippable);
        }

        /// <summary>
        /// Using this DynamicObject as a root node, remove the given DynamicObject from any of it's descendants.
        /// </summary>
        /// <param name="toRemove">The object to remove from the tree.</param>
        /// <returns>True if the removal was successful, False if the given object wasn't in the tree to begin with.</returns>
        public bool RemoveFromDescendants(DynamicObject toRemove)
        {
            Stack<DynamicObject> stack = new Stack<DynamicObject>();
            stack.Push(this);

            while(stack.Count > 0)
            {
                DynamicObject node = stack.Pop();

                if (node.Children.Remove(toRemove))
                    return true;

                foreach(DynamicObject child in node.Children)
                {
                    if (child.Children.Remove(toRemove))
                        return true;
                    else
                        stack.Push(child);
                }
            }

            return false; 
        }

        public void Interact(Interaction interaction, DynamicObject interactor)
        {
            // TODO: this is very repetetive, see what else can be done
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
                    if (this is IEquippable)
                    {
                        (this as IEquippable).BeEquipped(interactor);
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
            Utility.DrawDynamicObject(this, this.X, this.Y);
        }
    }
}