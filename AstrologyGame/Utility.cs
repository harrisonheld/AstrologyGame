using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.IO;
using System.Globalization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using AstrologyGame.MapData;
using AstrologyGame.Entities;

namespace AstrologyGame
{
    // this is a helper class. it contains miscellaneous functions that are helpful everywhere
    public static class Utility
    {
        public const string BOOK_PATH = @"C:\Users\Held\source\repos\AstrologyGame\AstrologyGame\books.xml";
        public const string ABILITIES_PATH = @"C:\Users\Held\source\repos\AstrologyGame\AstrologyGame\abilities.xml";
        const string ERROR_TEXTURE_NAME = "error";

        public const int SCALE = 32 * 1; // how many pixels high and wide sprites should be drawn as

        private static ContentManager content;
        private static GraphicsDevice graphics;
        private static SpriteBatch spriteBatch;

        public static ContentManager Content { get { return content; } }
        public static GraphicsDevice Graphics { get { return graphics; } }
        public static SpriteBatch SpriteBatch { get { return spriteBatch; } }

        public static SpriteFont Font { get; set; }

        // go from string to texture2d
        private static Dictionary<string, Texture2D> textureDict { get; set; } = new Dictionary<string, Texture2D>() { };

        static string[] DELIMITERS = { " " }; // for finding spaces in strings

        /// <summary>
        /// Load an entire folder of content. Got this from
        /// https://danielsaidi.com/blog/2010/01/26/load-all-assets-in-a-folder-in-xna
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="contentFolder"></param>
        /// <returns></returns>
        public static Dictionary<string, T> LoadContent<T>(string contentFolder)
        {
            //Load directory info, abort if none
            DirectoryInfo dir = new DirectoryInfo(Content.RootDirectory + "\\" + contentFolder);
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException();
            }

            //Init the resulting list
            Dictionary<string, T> result = new Dictionary<string, T>();

            //Load all files that matches the file filter
            FileInfo[] files = dir.GetFiles("*.*");
            foreach (FileInfo file in files)
            {
                string key = Path.GetFileNameWithoutExtension(file.Name);
                result[key] = Content.Load<T>(contentFolder + "/" + key);
            }

            //Return the result
            return result;
        }
        public static void Initialize(ContentManager _content, GraphicsDevice _graphics, SpriteBatch _spriteBatch)
        {
            content = _content;
            graphics = _graphics;
            spriteBatch = _spriteBatch;

            textureDict = LoadContent<Texture2D>("textures");
        }
        public static Texture2D GetTexture(string textureName)
        {
            Texture2D tex;

            if(textureDict.ContainsKey(textureName))
            {
                tex = textureDict[textureName];
            }
            else
            {
                tex = textureDict[ERROR_TEXTURE_NAME];
            }

            return tex;
        }

        /// <summary>
        /// Puts line breaks in <paramref name="text"/> so it won't go off screen. Not written by Zodiac Dev. See 
        /// <see href="https://stackoverflow.com/questions/15986473/how-do-i-implement-word-wrap"/>.
        /// </summary>
        public static string WrapText(SpriteFont spriteFont, string text, float maxLineWidth)
        {
            string[] words = text.Split(DELIMITERS, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sb = new StringBuilder();
            float lineWidth = 0f;
            float spaceWidth = spriteFont.MeasureString(" ").X;

            for (int i = 0; i < words.Length; i++)
            {
                Vector2 size = spriteFont.MeasureString(words[i]);

                if (lineWidth + size.X < maxLineWidth)
                {
                    sb.Append(words[i] + " ");
                    lineWidth += size.X + spaceWidth;
                }
                else
                {
                    sb.Append("\n" + words[i] + " ");
                    lineWidth = size.X + spaceWidth;
                }

                // if the word contains a newline and isnt the last word in the string
                if (words[i].Contains("\n") && i != (words.Length - 1) )
                    lineWidth = words[i+1].Length;
            }

            return sb.ToString();
        }

        public static void DrawEntity(Entity o, int x, int y)
        {
            if (!o.ShouldRender)
                return;

            int drawX = x * SCALE;
            int drawY = y * SCALE;

            Rectangle destinationRectangle = new Rectangle(drawX, drawY, SCALE, SCALE);
            spriteBatch.Draw(GetTexture(o.TextureName), destinationRectangle, o.Color);
        }

        // Using .GetHashCode() is inconsistent across application restarts.
        // The following will yield the same number for the same string every time.
        public static int SHA1Hash(string str)
        {
            using var algo = System.Security.Cryptography.SHA1.Create();
            return BitConverter.ToInt32(algo.ComputeHash(System.Text.Encoding.UTF8.GetBytes(str)));
        }

        public static void RenderMarkupText(string markup, Vector2 position)
        {
            Color defaultColor = Color.White;

            // only bother if we have color commands involved
            if (markup.Contains("<c:"))
            {
                // how far in x to offset from position
                int currentOffset = 0;

                // example:
                // string.Format("You attempt to hit the [color:#FFFF0000]{0}[/color] but [color:{1}]MISS[/color]!",
                // currentMonster.Name, Color.Red.ToHex(true));
                string[] splits = markup.Split(new string[] { "<c:" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var str in splits)
                {
                    // if this section starts with a color
                    if (str.StartsWith("#"))
                    {
                        // #123456789
                        string hexString = str.Substring(0, 7);

                        // any subsequent msgs after the [/color] tag are defaultColor
                        string[] msgs = str.Substring(8).Split(new string[] { "</c>" }, StringSplitOptions.RemoveEmptyEntries);

                        // always draw [0] there should be at least one
                        spriteBatch.DrawString(Font, msgs[0], position + new Vector2(currentOffset, 0), ColorFromString(hexString));
                        currentOffset += (int)Font.MeasureString(msgs[0]).X;

                        // there should only ever be one other string or none
                        if (msgs.Length == 2)
                        {
                            spriteBatch.DrawString(Font, msgs[1], position + new Vector2(currentOffset, 0), defaultColor);
                            currentOffset += (int)Font.MeasureString(msgs[1]).X;
                        }
                    }
                    else
                    {
                        spriteBatch.DrawString(Font, str, position + new Vector2(currentOffset, 0), defaultColor);
                        currentOffset += (int)Font.MeasureString(str).X;
                    }
                }
            }
            else
            {
                // just draw the string as ordered
                spriteBatch.DrawString(Font, markup, position, defaultColor);
            }
        }
        private static Color ColorFromString(string str)
        {
            // if its a hex code
            if(str[0] == '#')
            {
                int r = int.Parse(str.Substring(1, 2), NumberStyles.HexNumber);
                int g = int.Parse(str.Substring(3, 2), NumberStyles.HexNumber);
                int b = int.Parse(str.Substring(5, 2), NumberStyles.HexNumber);

                return new Color(r, g, b);
            }

            return Color.White;
        }

        // used to log info for the player to look at
        public static void Log(string str)
        {
            // TODO: make a log menu
            Debug.WriteLine(str);
        }
        public static void Log(object o)
        {
            Log(o.ToString());
        }
    }
}
