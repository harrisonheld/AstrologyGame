using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace AstrologyGame
{
    // this is a helper class. it contains miscellaneous functions that are helpful everywhere
    public static class Utility
    {
        const string ERROR_TEXTURE_NAME = "error";
        static string[] DELIMITERS = { " " };

        private static ContentManager Content;
        public static void Initialize(ContentManager _Content)
        {
            Content = _Content;
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

            foreach (string word in words)
            {
                Vector2 size = spriteFont.MeasureString(word);

                if (lineWidth + size.X < maxLineWidth)
                {
                    sb.Append(word + " ");
                    lineWidth += size.X + spaceWidth;
                }
                else
                {
                    sb.Append("\n" + word + " ");
                    lineWidth = size.X + spaceWidth;
                }
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
    }
}
