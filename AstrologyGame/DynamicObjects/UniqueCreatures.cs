using System;
using System.Collections.Generic;
using System.Text;

using AstrologyGame.DynamicObjects;
using AstrologyGame.MapData;

using Microsoft.Xna.Framework;


/* This file contains unique handcrafted NPC's that generally only appear once in the game */
namespace AstrologyGame.DynamicObjects
{
    public class Pisces : Humanoid
    {
        public Pisces()
        {
            Name = "Pisces";
            TextureName = "pisces2";
            Lore = "He wears a sky-blue flare dress lined with white. Mirrored on both hands, each of five nails is painted a different pastel color. " +
                "Mascaraed lashes frame his doe eyes. Most notably, white cat-ears protrude from his blonde hair.";
            Color = Color.LightSkyBlue;
            MaxHealth = 100;
            Health = MaxHealth;
            Quickness = 70;

            Children.Add(new PiscesDress());
            Children.Add(new PiscesBoots());
        }

        public override void AiTurn()
        {
            Seek(Zone.Player);
            base.AiTurn();
        }
    }
    public class PiscesDress : Item
    {
        public PiscesDress()
        {
            TextureName = "dress";
            Name = "Pisces' dress";
            Lore = "A sky-blue flare dress lined with white.";
            Color = Color.LightSkyBlue;
        }
    }
    public class PiscesBoots : Item
    {
        public PiscesBoots()
        {
            TextureName = "boots";
            Name = "Pisces' socks";
            Lore = "White and light blue horizontal stripes cover the socks at equal intervals. " +
                "Pisces would insist they strengthen his magic. " +
                "The veracity of this is up for debate, to say the least.";
            Color = Color.LightSkyBlue;
        }
    }
}
