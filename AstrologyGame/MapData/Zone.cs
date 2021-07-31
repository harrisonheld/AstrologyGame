using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

using System.Xml;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using AstrologyGame.Entities;
using AstrologyGame.Entities.Factories;
using AstrologyGame.Components;
using AstrologyGame.Systems;

namespace AstrologyGame.MapData
{
    public static class Zone
    {
        public const int WIDTH = 32;
        public const int HEIGHT = 18;

        // array of all the tiles
        private static Tile[,] tiles { get; set; } = new Tile[WIDTH, HEIGHT];
        // list of all entities in the zone
        private static List<Entity> entities { get; set; } = new List<Entity>() { };
        public static List<Entity> Entities { get { return entities; } }

        public static int tickCount = 0;

        // all the systems. they are in the order they will be run
        private static ISystem[] systems { get; set; } = new ISystem[5]
        {
            new EnergyRechargingSystem(),
            new HealthSystem(),
            new AISystem(),
            new PlayerInputSystem(),
            new GasSystem()
        };
        // index of the system we are running
        private static int systemIndex = 0;

        public static void Initialize()
        {
            // TODO: use reflection to automatically register all the systems so we don't have that big array defining them
        }

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
        }

        public static void Update()
        {
            // when this loop is exited, a full Tick will have been completed.
            while (systemIndex < systems.Length)
            {
                ISystem system = systems[systemIndex];
                system.Run();

                // if system isn't done, return so we can pick up work on the next Update
                if (!system.Finished)
                    return;
                // else
                    system.Reset();
                    systemIndex++;
            }

            tickCount++;

            systemIndex = 0;
        }

        public static void AddEntity(Entity e)
        {
            // only add the entity if it's not already in the zone
            if(!entities.Contains(e))
                entities.Add(e);
        }
        public static void RemoveEntity(Entity e)
        {
            Entities.Remove(e);
        }
        public static void RemoveEntityAt(int index)
        {
            RemoveEntity(entities[index]);
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