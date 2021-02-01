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
            TextureName = "pisces";
            Lore = "He wears a sky-blue flare dress lined with white. Mirrored on both hands, each nail is painted a different pastel color. " +
                "Mascaraed lashes frame his doe eyes. Most notably, white cat-ears protrude from his blonde hair.";
            color = Color.LightSkyBlue;

            Quickness = 7;
        }

        public override void AiTurn()
        {
            base.AiTurn();
            Seek(Zone.Player);
        }
    }

    public class Scorpio : Humanoid
    { 
        public Scorpio()
        {
            Name = "Scorpio";
            TextureName = "scorpio";
            Lore = "Piracy is a very serious crime. Please close the game immediately and report this stolen copy at https://zodiac.com/piracy";
        }
    }
}
