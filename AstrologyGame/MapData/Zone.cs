using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

using System.Xml;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using AstrologyGame.DynamicObjects;

namespace AstrologyGame.MapData
{
    public static class Zone
    {
        public const int WIDTH = 16;
        public const int HEIGHT = 9;

        //Declare new layers
        public static Tile[,] tiles { get; set; } = new Tile[WIDTH, HEIGHT];
        // list of all objects (that aren't tiles) in the zone
        public static ObservableCollection<DynamicObject> Objects { get; set; } = new ObservableCollection<DynamicObject>() { };
        public static DynamicObject Player { get; set; }

        /// <summary>
        /// Clears all tiles and remove all objects
        /// </summary>
        public static void Clear()
        {
            tiles = new Tile[WIDTH, HEIGHT];
            Objects.Clear();
        }

        /// <summary>
        ///  Save the Zone state as an XML file.
        /// </summary>
        /// <param name="path"></param>
        public static void SaveXml(string path)
        {
            XmlWriter xmlWriter = XmlWriter.Create(path);

            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("tiles");

            for (int y = 0; y < HEIGHT; y++)
            {
                for (int x = 0; x < WIDTH; x++)
                {
                    xmlWriter.WriteStartElement("t");

                    // write the Class Type of the tile
                    string type = (tiles[x, y].GetType().Name);
                    xmlWriter.WriteAttributeString("Type", type);

                    foreach (DynamicObject o in Objects)
                    {
                        if (o.X == x && o.Y == y)
                        {
                            // TODO: save all the objects here
                        }
                    }

                    xmlWriter.WriteEndElement();
                }
            }

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
        }

        /// <summary>
        /// Load a Zone state from a file.
        /// </summary>
        /// <param name="path"></param>
        public static void LoadXml(string path)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);

            Clear();

            // because we know all tiles are in the same assembly as Stone, we use this assembly to get types of other tiles
            System.Reflection.Assembly assembly = typeof(Stone).Assembly;

            foreach (XmlNode itemNode in xmlDoc.ChildNodes)
            {
                if (itemNode.Name == "tiles")
                {
                    int x = 0;
                    int y = 0;

                    foreach (XmlNode tileNode in itemNode.ChildNodes)
                    {
                        Tile tile = (Tile)DynamicObjectFromXmlNode(tileNode, assembly);
                        tiles[x, y] = tile;

                        foreach (XmlNode dynamicObjectNode in tileNode.ChildNodes)
                        {
                            DynamicObject o = DynamicObjectFromXmlNode(dynamicObjectNode, assembly);
                            Objects.Add(o);
                        }

                        x++;
                        if (x > WIDTH - 1)
                        {
                            y++;
                            x = 0;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Take an XML node representing an Dynamic Object and make a Dynamic Object of that type.
        /// Provide the assembly in which that class is found.
        /// </summary>
        /// <param name="typeString"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        private static DynamicObject DynamicObjectFromXmlNode(XmlNode node, System.Reflection.Assembly assembly)
        {
            string typeString = node.Attributes.GetNamedItem("Type").InnerText;
            typeString = "AstrologyGame.DynamicObjects." + typeString;

            Type type = assembly.GetType(typeString);

            return (DynamicObject)Activator.CreateInstance(type);
        }
        /// <summary>
        /// Have all the DynamicObjects in the Zone do their turns.
        /// </summary>
        public static void Tick()
        {
            foreach (DynamicObject o in Objects)
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
        }

        // remove an object, whether its in the zone's objects or if its a descendant of the zone objects
        public static bool RemoveObject(DynamicObject toRemove)
        {
            if (Objects.Remove(toRemove))
                return true;

            foreach(DynamicObject o in Objects)
            {
                if (o.RemoveFromDescendants(toRemove))
                    return true;
            }

            return false;
        }

        public static List<DynamicObject> ObjectsAtPosition(int x, int y)
        {
            List<DynamicObject> objectsAtPos = new List<DynamicObject>();

            foreach (DynamicObject o in Objects)
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
            foreach (DynamicObject o in Objects)
            {
                o.Draw();
            }
        }
    }
}