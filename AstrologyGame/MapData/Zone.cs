using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

using System.Xml;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using AstrologyGame.Entities;

namespace AstrologyGame.MapData
{
    public static class Zone
    {
        public const int WIDTH = 16;
        public const int HEIGHT = 9;

        // array of all the tiles
        private static Tile[,] tiles { get; set; } = new Tile[WIDTH, HEIGHT];
        // list of all entities in the zone
        private static List<Entity> entities { get; set; } = new List<Entity>() { };
        public static List<Entity> Entities { get { return entities; } }
        public static Entity Player { get; set; }

        // Clears all tiles and remove all objects
        public static void Clear()
        {
            tiles = new Tile[WIDTH, HEIGHT];
            entities.Clear();
        }

        public static void Generate(int seed)
        {
            Random rand = new Random(seed);
            Clear();

            // what biome should this zone generate as
            Biome biome = BiomeInfo.DebugLand;
            /*double s = rand.NextDouble();
            Biome biome;
            if (s > 0.5)
                biome = BiomeInfo.CydonianSands;
            else
                biome = BiomeInfo.FontOfMiscreation;*/

            for (int y = 0; y < HEIGHT; y++)
            {
                for (int x = 0; x < WIDTH; x++)
                {
                    double r = rand.NextDouble();
                    double chance = 0.0;

                    for (int i = 0; i < biome.TileWeights.Length; i++)
                    {
                        chance += biome.TileWeights[i];

                        if (r < chance)
                        {
                            Tile newTile = (Tile)Activator.CreateInstance(biome.TileTypes[i]);
                            tiles[x, y] = newTile;
                            break;
                        }
                    }
                }
            }

            Entity book = EntityFactory.EntityFromId("booktest", 10, 4);
            AddEntity(book);

            Entity pisces = EntityFactory.EntityFromId("pisces", 12, 5);
            AddEntity(pisces);

            Entity gun = EntityFactory.EntityFromId("flintlock", 9, 6);
            AddEntity(gun);

            // if there was a player in the zone prior, include him in the new one
            if (Player != null)
            {
                entities.Add(Player);
            }
        }

        public static void AddEntity(Entity e)
        {
            // only add the entity if it's not already in the zone
            if(!entities.Contains(e))
                entities.Add(e);
        }

        public static List<Entity> GetEntitiesAtPosition(OrderedPair p)
        {
            List<Entity> objectsAtPos = new List<Entity>();

            foreach (Entity o in entities)
            {
                // if the entity doesn't have a position, continue
                if (!o.HasComponent<Position>())
                    continue;

                if (o.GetComponent<Position>().Pos.Equals(p))
                {
                    objectsAtPos.Add(o);
                }
            }

            return objectsAtPos;
        }
        public static Tile GetTileAtPosition(OrderedPair p)
        {
            return tiles[p.X, p.Y];
        }
    }
}