using System;
using System.Collections.Generic;
using System.Text;

namespace AstrologyGame.Entities
{
    public class Attributes : EntityComponent
    {
        // base stats
        public int Prowess { get; set; } = 10;
        public int Vigor { get; set; } = 10;
        public int Faith { get; set; } = 10;
        public int TheFourthStat { get; set; } = 10;

        // health
        public int MaxHealth { get; set; } = 10;
        public int Health { get; set; } = 10;

        public int Quickness { get; set; } = 100;
    }
}
