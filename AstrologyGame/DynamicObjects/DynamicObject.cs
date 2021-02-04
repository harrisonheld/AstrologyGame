﻿using System;
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
        public string SignText { get; set; }

        // the 4 stats
        public DynamicObjectStats Stats { get; set; } = new DynamicObjectStats
        {
            Vigor = 0,
            Prowess = 0,
            Faith = 0,
            Some4thThing = 0
        };

        public List<DynamicObject> Children { get; set; } = new List<DynamicObject>() { };
        // list of things you can do to the object
        protected List<Interaction> interactions = new List<Interaction>();
        public List<Interaction> Interactions
        {
            get
            {
                return interactions;
            }
        }

        public DynamicObject()
        {

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
            if (!interactions.Contains(interaction))
            {
                Debug.WriteLine("You can't use Interaction type " + interaction.ToString() + " on this object.");
                return;
            }

            switch (interaction)
            {
                case Interaction.Read:
                    BeRead(interactor);
                    return;
                case Interaction.Open:
                    BeOpened(interactor);
                    return;
                case Interaction.Attack:
                    BeAttacked(interactor);
                    return;
                case Interaction.Get:
                    BeGot(interactor);
                    return;
                case Interaction.Drop:
                    BeDropped(interactor);
                    return;
            }
        }

        protected virtual void BeRead(DynamicObject reader) { }
        protected virtual void BeOpened(DynamicObject opener) { }
        protected virtual void BeAttacked(DynamicObject attacker)
        {
            Debug.WriteLine("Ouch!");
        }
        protected virtual void BeGot(DynamicObject pickerUpper)
        {
            Zone.RemoveObject(this);
            pickerUpper.Children.Add(this);
        }
        protected virtual void BeDropped(DynamicObject dropper)
        {
            dropper.Children.Remove(this);
            this.X = dropper.X;
            this.Y = dropper.Y;
            Zone.Objects.Add(this);
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