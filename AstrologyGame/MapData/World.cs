﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using System.Xml;

namespace AstrologyGame.MapData
{
    public static class World
    {
        const int HEIGHT = 3;
        const int WIDTH = 3;

        public static int ZoneX { get; set; }
        public static int ZoneY { get; set; }

        private static string worldSeedString = Environment.TickCount.ToString(); // a string who's hash will be used to generate the World
        private static int worldSeedInt = GameManager.SHA1Hash(worldSeedString); // the true integer seed of the World
        public static string Seed
        {
            get
            {
                return worldSeedString;
            }
            set
            {
                worldSeedString = value;
                worldSeedInt = GameManager.SHA1Hash(worldSeedString);
            }
        }

        public static int GetZoneSeed()
        {
            /* TODO: REPLACE THIS MATH LATER
                Ideally, you would want even a small change in the World Seed to drastically change the world. With the current math,
                small changes in the World Seed will cause identical Zone Seeds to appear, just in different places. 
                For example, assuming WIDTH = 5,
                if worldSeed = 100, ZoneX = 1, and ZoneY = 1, then the Zone Seed will be 106.
                if worldSeed = 101, ZoneX = 0, and ZoneY = 1, then the Zone Seed will be 106.
                If the user decides to test numeric seeds sequentially (for some reason), they are likely to notice this.

                ALSO: Increasing the seed by only one cases strange behavior when moving between zones. With two possible 
                biome options, is extremely noticible that the biome just alternates back and forth as 
                you move across zones. I guess this is just a consequence of incrementing seeds by 1?
            */

            // temporary solution is to just take the hash
            string s = (ZoneY * WIDTH + ZoneX + worldSeedInt).ToString();
            return GameManager.SHA1Hash(s);
        }
        public static void GenerateCurrentZone()
        {
            int zoneSeed = GetZoneSeed();
            Zone.Generate(zoneSeed);
        }
        public static void Save(string path)
        {
            using XmlWriter xmlWriter = XmlWriter.Create(path);
            xmlWriter.WriteStartDocument();

            xmlWriter.WriteStartElement("world");
            xmlWriter.WriteAttributeString("seed", worldSeedString);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
        }
    }
}