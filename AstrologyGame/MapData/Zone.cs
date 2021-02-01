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

        // go from string to texture2d
        public static Dictionary<string, Texture2D> textureDict { get; set; } = new Dictionary<string, Texture2D>() { };

        //Declare new layers
        public static Tile[,] tiles { get; set; } = new Tile[WIDTH, HEIGHT];
        // list of all objects (that aren't tiles) in the zone
        public static ObservableCollection<DynamicObject> objects { get; set; } = new ObservableCollection<DynamicObject>() { };
        private static DynamicObject player;
        public static DynamicObject Player
        {
            get
            {
                return player;
            }
            set
            {
                player = value;
            }
        }

        /// <summary>
        /// Clears all tiles and remove all objects
        /// </summary>
        public static void Clear()
        {
            tiles = new Tile[WIDTH, HEIGHT];
            objects.Clear();
            textureDict.Clear();
        }

        public static void Initialize()
        {
            objects.CollectionChanged += ObjectsChanged;
        }

        private static void AddStringToTextureDict(string texName)
        {
            // if key not in the dictionary, add it
            if (!textureDict.ContainsKey(texName))
            {
                textureDict.Add(texName, Utility.TryLoadTexture(texName));
            }
        }
        /// <summary>
        /// Removes a string-Texture2D pair from the texture dictionary ONLY if no tiles or objects use it.
        /// </summary>
        /// <param name="texName"></param>
        private static void RemoveStringFromTextureDict(string texName)
        {
            // TODO: IMPLEMENT THIS
            // if no objects use this texture, remove it
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

                    foreach (DynamicObject o in objects)
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
                            objects.Add(o);
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
            foreach (DynamicObject o in objects)
            {
                if (o is Creature)
                {
                    Creature c = o as Creature;
                    if (c == player)
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
        // the main Generate method
        public static void Generate(int seed)
        {
            Random rand = new Random(seed);
            Clear();

            // what biome should this zone generate as
            Biome biome = BiomeInfo.FontOfMiscreation;

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
                            tiles[x, y] = (Tile)Activator.CreateInstance(biome.TileTypes[i]);
                            break;
                        }
                    }

                    AddStringToTextureDict(tiles[x, y].TextureName);
                }
            }

            // if there was a player in the zone prior, include him in the new one
            if (player != null)
            {
                objects.Add(player);
            }

            Chest chest = new Chest();
            chest.X = 1;
            chest.Y = 5;
            objects.Add(chest);

            chest.Children.Add(new TeaPot() { Count = 4 });
            chest.Children.Add(new MaidDress());
            chest.Children.Add(new CatEars());

            Book b = new Book("test");
            objects.Add(b);

            objects.Add(new TeaPot() { X = 5, Y = 5 });
            objects.Add(new Flintlock() { X = 5, Y = 5 });
            objects.Add(new MaidDress() { X = 5, Y = 5 });

            /*
            Sign sign = new Sign();
            sign.signText = "Abhoth, the ultimate source of all miscreation and abomination";
            sign.x = 3;
            sign.y = 1;
            sign.color = Color.Gray;
            objects.Add(sign);
            */
        }
        public static List<DynamicObject> ObjectsAtPosition(int x, int y)
        {
            List<DynamicObject> objectsAtPos = new List<DynamicObject>();

            foreach (DynamicObject o in objects)
            {
                if (o.X == x && o.Y == y)
                {
                    objectsAtPos.Add(o);
                }
            }

            return objectsAtPos;
        }
        private static void ObjectsChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            // add textures for new objects
            if (args.NewItems != null)
                foreach (object o in args.NewItems)
                    AddStringToTextureDict((o as DynamicObject).TextureName);

            // remove textures for objects removed
            if (args.OldItems != null)
                foreach (object o in args.OldItems)
                    RemoveStringFromTextureDict(o.ToString());
        }
    }
}