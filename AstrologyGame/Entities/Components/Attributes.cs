using System;
using System.Collections.Generic;
using System.Text;

namespace AstrologyGame.Entities.Components
{
    public class Attributes : Component
    {
        // base stats
        // comments are just hypothetical ideas of what they do
        public int Prowess { get; set; } = 10; // damage
        public int Vigor { get; set; } = 10; // max HP, recovery rate, dodging, 
        public int Faith { get; set; } = 10; // mana / mana regen rate, magic damage
        public int Concentration { get; set; } = 10; // ability cooldown, crit chance

        public int Quickness { get; set; } = 100;
    }
}
