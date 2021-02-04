using System;
using System.Collections.Generic;
using System.Text;

using AstrologyGame.DynamicObjects;

namespace AstrologyGame.MapData
{
    public static class BiomeInfo
    {
        public readonly static Biome DebugLand = new Biome()
        {
            TileTypes = new Type[] { typeof(DebugTile) },
            TileWeights = new double[] { 1.0 }
        };

        public readonly static Biome FontOfMiscreation = new Biome()
        {
            TileTypes = new Type[] { typeof(Offal), typeof(Stone) },
            TileWeights = new double[] { 0.05, 0.95 }
        };

        public readonly static Biome CydonianSands = new Biome()
        {
            TileTypes = new Type[] { typeof(Sand), typeof(BleachedSand), typeof(Stone) },
            TileWeights = new double[] {0.8, 0.05, 0.15}
        };

        /*
         * "What's my new biome called?"
         * "The Abyss."
         * "What do you do in The Abyss?"
         * "Get foched."
         */
        public readonly static Biome TheAbyss = new Biome()
        {

        };
    }

    public struct Biome
    {
        public Type[] TileTypes { get; set; }
        public double[] TileWeights { get; set; } // should add up to 1.00;
    }
}