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

        //Declare new layers
        public static Tile[,] tiles { get; set; } = new Tile[WIDTH, HEIGHT];
        // list of all objects (that aren't tiles) in the zone
        public static ObservableCollection<Entity> Objects { get; set; } = new ObservableCollection<Entity>() { };
        public static Entity Player { get; set; }

        /// <summary>
        /// Clears all tiles and remove all objects
        /// </summary>
        public static void Clear()
        {
            tiles = new Tile[WIDTH, HEIGHT];
            Objects.Clear();
        }

        /// <summary>
        /// Have all the Entities in the Zone do their turns.
        /// </summary>
        public static void Tick()
        {
            foreach (Entity o in Objects)
            {
                if (o is Creature)
                {
                    Creature c = o as Creature;
                    if (c == Player)
                    {
                        c.RechargeAP();
                    }
                    else
                    {
                        c.AiTurn();
                    }
                }
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
            Biome biome = BiomeInfo.CydonianSands;
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
                            newTile.X = x;
                            newTile.Y = y;
                            tiles[x, y] = newTile;
                            break;
                        }
                    }
                }
            }

            // if there was a player in the zone prior, include him in the new one
            if (Player != null)
            {
                Objects.Add(Player);
            }

            Pisces p = new Pisces() { X = 5, Y = 5 };
            Objects.Add(p);

            Container container = new Container() { X = 4, Y = 6 };
            Flintlock gun = new Flintlock();
            container.Children.Add(gun);
            Objects.Add(container);

            ChildOfAbhoth c = new ChildOfAbhoth() { X = 3, Y = 3 };
            Objects.Add(c);
        }

        // remove an object, whether its in the zone's objects or if its a descendant of the zone objects
        public static bool RemoveObject(Entity toRemove)
        {
            if (Objects.Remove(toRemove))
                return true;

            foreach(Entity o in Objects)
            {
                if (o.RemoveFromDescendants(toRemove))
                    return true;
            }

            return false;
        }

        public static List<Entity> ObjectsAtPosition(int x, int y)
        {
            List<Entity> objectsAtPos = new List<Entity>();

            foreach (Entity o in Objects)
            {
                if (o.X == x && o.Y == y)
                {
                    objectsAtPos.Add(o);
                }
            }

            return objectsAtPos;
        }

        public static void Draw()
        {
            // draw the tiles
            for (int y = 0; y < HEIGHT; y++)
            {
                for (int x = 0; x < WIDTH; x++)
                {
                    tiles[x, y].Draw();
                }
            }

            // draw the objects
            foreach (Entity o in Objects)
            {
                o.Draw();
            }
        }
    }
}