using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using AstrologyGame.MapData;
using AstrologyGame.DynamicObjects;

namespace AstrologyGame
{
    // this is a helper class. it contains miscellaneous functions that are helpful everywhere
    public static class Utility
    {
        public const int SCALE = 32 * 2; // how many pixels high and wide sprites should be drawn as

        private static ContentManager content;
        private static GraphicsDevice graphics;
        private static SpriteBatch spriteBatch;

        public static ContentManager Content { get { return content; } }
        public static GraphicsDevice Graphics { get { return graphics; } }
        public static SpriteBatch SpriteBatch { get { return spriteBatch; } }

        const string ERROR_TEXTURE_NAME = "error";
        static string[] DELIMITERS = { " " };

        public static void Initialize(ContentManager _content, GraphicsDevice _graphics, SpriteBatch _spriteBatch)
        {
            content = _content;
            graphics = _graphics;
            spriteBatch = _spriteBatch;
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

        public static Texture2D TryLoadTexture(string textureName)
        {
            Texture2D tex;

            try
            {
                tex = Content.Load<Texture2D>(textureName);
            }
            catch
            {
                // if the texture fails to load, load the error texture
                // if that fails, oh well i guess
                tex = Content.Load<Texture2D>(ERROR_TEXTURE_NAME);
                Debug.WriteLine("Failed to load a texture!");
            }

            return tex;
        }

        public static void DrawDynamicObject(DynamicObject o, int x, int y)
        {
            int drawX = x * SCALE;
            int drawY = y * SCALE;

            Rectangle destinationRectangle = new Rectangle(drawX, drawY, SCALE, SCALE);
            spriteBatch.Draw(Zone.textureDict[o.TextureName], destinationRectangle, o.Color);
        }

        public static int SHA1Hash(string str)
        {
            using var algo = System.Security.Cryptography.SHA1.Create();
            return BitConverter.ToInt32(algo.ComputeHash(System.Text.Encoding.UTF8.GetBytes(str)));
        }
    }
}
