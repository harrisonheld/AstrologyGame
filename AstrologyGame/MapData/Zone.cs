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
        public static Entity Player { get; set; }

        // Clears all tiles and remove all objects
        public static void Clear()
        {
            tiles = new Tile[WIDTH, HEIGHT];
            entities.Clear();
        }
  
        /// <summary>
        /// Have all the DynamicObjects in the Zone do their turns.
        /// </summary>
        public static void Tick()
        {
            foreach (Entity e in entities)
            {
                e.UpdateAllComponents();
            }

            for (int y = 0; y < HEIGHT; y++)
                for (int x = 0; x < WIDTH; x++)
                    tiles[x, y].AnimationTurn();
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
                            OrderedPair pos = newTile.GetComponent<Position>().Pos;
                            pos.X = x;
                            pos.Y = y;
                            tiles[x, y] = newTile;
                            break;
                        }
                    }
                }
            }

            Entity chest = new Entity();
            chest.AddComponent(new Inventory() { interactable = true });
            chest.GetComponent<Position>().Pos = new OrderedPair(5, 5);
            AddEntity(chest);

            Entity book = new Entity();
            book.AddComponent(new Book() { bookId = "MasterOfTheMoon" });
            chest.GetComponent<Position>().Pos = new OrderedPair(3, 0);
            AddEntity(book);

            // if there was a player in the zone prior, include him in the new one
            if (Player != null)
            {
                entities.Add(Player);
            }
        }

        // remove an object, whether its in the zone's objects or if its a descendant of the zone objects
        public static bool RemoveObject(Entity toRemove)
        {
            if (entities.Remove(toRemove))
                return true;

            foreach(Entity o in entities)
            {
                if (o.RemoveFromDescendants(toRemove))
                    return true;
            }

            return false;
        }

        public static void AddEntity(Entity e)
        {
            entities.Add(e);
        }

        public static List<Entity> GetEntitiesAtPosition(OrderedPair p)
        {
            List<Entity> objectsAtPos = new List<Entity>();

            foreach (Entity o in entities)
            {
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

        public static void Draw()
        {
            // TODO: draw tiles
            for (int y = 0; y < HEIGHT; y++)
                for (int x = 0; x < WIDTH; x++)
                    Utility.DrawEntity(tiles[x, y], x, y);

            // draw the objects
            foreach (Entity o in entities)
            {
                Position p = o.GetComponent<Position>();
                Utility.DrawEntity(o, p.x, p.y);
            }
        }
    }
}