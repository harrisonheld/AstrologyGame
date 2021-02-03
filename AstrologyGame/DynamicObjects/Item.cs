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
    public class TeaPot : Item
    {
        public TeaPot()
        {
            Name = "tea pot";
            Lore = "You can have a tea party!";
            TextureName = "teacup";
        }
    }
    public class Teacup : Item
    {
        public Teacup()
        {
            Name = "tea cup";
            Lore = "You can have a tea party!";
            TextureName = "teacup";
        }
    }

    public class MaidDress : Item
    {
        public MaidDress()
        {
            Name = "maid outfit";
            Lore = "You can have a tea party!";
            TextureName = "teacup";
        }
    }

    public class CatEars : Item
    {
        public CatEars()
        {
            Name = "cat ears";
            Lore = "You can have a tea party!";
            TextureName = "teacup";
        }
    }
}