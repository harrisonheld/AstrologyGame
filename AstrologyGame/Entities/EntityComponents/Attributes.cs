using System;
using System.Collections.Generic;
using System.Text;

namespace AstrologyGame.Entities
{
    public class Attributes : EntityComponent
    {
        public int Prowess { get; set; } = 10;
        public int Vigor { get; set; } = 10;
        public int Faith { get; set; } = 10;
        public int TheFourthStat { get; set; } = 10;

        public int Quickness { get; set; } = 100;
    }
}
