using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AstrologyGame.DynamicObjects
{
    public abstract class Item : DynamicObject
    {
        public int Count { get; set; } = 1;

        public Item()
        {
            interactions.Add(Interaction.Get);
            interactions.Add(Interaction.Drop);
        }
    }

    public class Flintlock : Item
    {
        public Flintlock()
        {
            Name = "flintlock";
            Lore = "a gunny";
            TextureName = "flintlock";
        }
    }
}