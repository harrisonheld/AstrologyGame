using System;
using System.Collections.Generic;
using System.Text;

using AstrologyGame.Entities;
using AstrologyGame.MapData;

using Microsoft.Xna.Framework;


/* This file contains unique handcrafted NPC's that generally only appear once in the game */
namespace AstrologyGame.Entities
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
            Quickness = 70;

            Equip(new PiscesDress());
            Equip(new PiscesSocks());
        }

        public override void AiTurn()
        {
            Seek(Zone.Player);
            base.AiTurn();
        }
    }

    public class PiscesSocks : EquippableItem
    {
        public PiscesSocks()
        {
            EquipSlot = Slot.Legs;
             
            TextureName = "socks";
            Name = "Pisces' socks";
            Lore = "White and light blue horizontal stripes cover the socks at equal intervals. " +
                "Pisces would insist they strengthen his magic. " +
                "The veracity of this is up for debate, to say the least.";
            Color = Color.LightSkyBlue;
        }
    }

    public class PiscesDress : EquippableItem
    {
        public PiscesDress()
        {
            EquipSlot = Slot.Body;

            TextureName = "dress";
            Name = "Pisces' dress";
            Lore = "It's a sky blue flare dress lined with white.";
            Color = Color.LightSkyBlue;
        }
    }
}
