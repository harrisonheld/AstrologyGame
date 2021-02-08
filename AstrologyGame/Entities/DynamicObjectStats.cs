using System;
using System.Collections.Generic;
using System.Text;

namespace AstrologyGame.Entities
{
    public struct PrimaryAttributes
    {
        // max HP, ability cool down, speed
        public int Vigor { get; set; }
        // damage
        public int Prowess { get; set; }
        // magic ability, luck (i.e. loot find), accuracy
        public int Faith { get; set; } 
        // makes you have more swag
        public int Some4thThing { get; set; }

        public PrimaryAttributes(int _vigor, int _prowess, int _faith, int _some4thThing)
        {
            Vigor = _vigor;
            Prowess = _prowess;
            Faith = _faith;
            Some4thThing = _some4thThing;
        }
    }
}
