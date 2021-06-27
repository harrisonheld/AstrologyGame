using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AstrologyGame.Menus
{
    public class Menu
    {
        public const string BACKGROUND_TEXTURE_NAME = "blank";
        private const string CURSOR_TEXTURE_NAME = "marble";

        public static SpriteFont Font { get; set; }

        protected static Texture2D cursorTexture;
        protected static Texture2D backgroundTexture;
        public Color BackgroundColor { get; set; } = Color.Black;

        private Rectangle rect;
        public OrderedPair Position
        {
            get
            {
                return new OrderedPair(rect.X, rect.Y);
            }
            set
            {
                rect.X = (int)value.X;
                rect.Y = (int)value.Y;
            }
        }
        public OrderedPair Size
        {
            get
            {
                return new OrderedPair(rect.Width, rect.Height);
            }
            set
            {
                rect.Width = (int)value.X;
                rect.Height = (int)value.Y;
            }
        }

        protected string wrappedText;
        private string text;
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
                wrappedText = Utility.WrapText(Font, text, Size.X);
            }
        }

        // should opening this menu cause the GameState to change to InMenu
        public bool PauseWhenOpened { get; set; } = true;

        public static void Initialize()
        {
            cursorTexture = Utility.GetTexture(CURSOR_TEXTURE_NAME);
            backgroundTexture = Utility.GetTexture(BACKGROUND_TEXTURE_NAME);
        }
        public Menu()
        {
            PauseWhenOpened = true;
            rect = new Rectangle(5, 5, 300, 9*64); // also an arbitrary size
        }
        public virtual void Refresh()
        {
            // do nothing. menus that are procedurally generated (like inventories) should override this to refresh their text
        }

        public virtual void Draw()
        {
            // draw the background
            Utility.SpriteBatch.Draw(backgroundTexture, rect, BackgroundColor);
            // draw the text
            Vector2 textPos = new Vector2(rect.X, rect.Y);
            Utility.SpriteBatch.DrawString(Font, wrappedText, textPos, Color.White);
        }

        public void Center()
        {
            // change the position so the menu is in the center of the screen.
            throw new NotImplementedException();
        }

        public virtual void HandleInput(List<Control> controls)
        {
            if (controls.Contains(Control.Back))
                Game1.CloseMenu(this);
        }
    }
}
