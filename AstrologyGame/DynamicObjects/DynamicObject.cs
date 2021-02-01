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
        public string TextureName { get; set; } //file name of the texture it should use
        public string Name { get; set; } // is the thing named, ie Orion, Harrison, for creatures or Sand, Stone, for tiles
        public string Lore { get; set; } // the item description, creature description, etc.
        // x and y position of the object
        public int X { get; set; }
        public int Y { get; set; }
        public bool Solid { get; set; } // can you walk through the thing
        public Color Color { get; set; } = Color.White;
        public string SignText { get; set; }

        public List<DynamicObject> Children { get; set; } = new List<DynamicObject>() { };
        // list of things you can do to the object
        protected readonly List<Interaction> interactions = new List<Interaction>();
        public List<Interaction> Interactions
        {
            get
            {
                return interactions;
            }
        }

        public DynamicObject()
        {
            TextureName = "";
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
                    Read(interactor);
                    return;
                case Interaction.Open:
                    Open(interactor);
                    return;
                case Interaction.Attack:
                    Attack(interactor);
                    return;
                case Interaction.Get:
                    Get(interactor);
                    return;
                case Interaction.Drop:
                    Drop(interactor);
                    return;
            }
        }

        protected virtual void Read(DynamicObject reader) { }
        protected virtual void Open(DynamicObject opener) { }
        protected virtual void Attack(DynamicObject attacker) { }
        protected virtual void Get(DynamicObject pickerUpper)
        {
            pickerUpper.Children.Add(this);
            Zone.Objects.Remove(this);
        }
        protected virtual void Drop(DynamicObject dropper)
        {
            Debug.WriteLine("dropped");
            dropper.Children.Remove(this);
            this.X = dropper.X;
            this.Y = dropper.Y;
            Zone.Objects.Add(this);
        }
        public virtual void AnimationTurn()
        {
            // things like changing the texture, changing color, etc.
        }
    }
}