using System;
using System.Collections.Generic;

using System.Text;

using System.Xml;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using AstrologyGame.DynamicObjects;
using AstrologyGame.MapData;

namespace AstrologyGame
{
    public class Menu
    {
        // TODO: make a list of menu elements, mostly text, then when its time to draw, iterate over them and draw them.

        public const string BOOK_PATH = @"./books.xml";
        public const string BACKGROUND_TEXTURE_NAME = "black";
        private const string CURSOR_TEXTURE_NAME = "marble";

        public static GraphicsDevice graphicsDevice { get; set; }
        public static SpriteFont font { get; set; }
        public static SpriteBatch spriteBatch { get; set; }

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
                wrappedText = Utility.WrapText(font, text, rect.Width);
            }
        }

        // should opening this menu cause the GameState to change to InMenu
        public bool PauseWhenOpened { get; set; } = true;

        public static void Initalize(GraphicsDevice _graphicsDevice, SpriteBatch _spriteBatch, SpriteFont _font)
        {
            graphicsDevice = _graphicsDevice;
            spriteBatch = _spriteBatch;
            font = _font;

            cursorTexture = Utility.TryLoadTexture(CURSOR_TEXTURE_NAME);
            backgroundTexture = Utility.TryLoadTexture(BACKGROUND_TEXTURE_NAME);
        }
        public Menu()
        {
            PauseWhenOpened = true;
            rect = new Rectangle(5, 5, 300, 9*64); // also an arbitrary size
        }

        public virtual void Draw()
        {
            // draw the background
            spriteBatch.Draw(backgroundTexture, rect, BackgroundColor);
            // draw the text
            Vector2 textPos = new Vector2(rect.X, rect.Y);
            spriteBatch.DrawString(font, wrappedText, textPos, Color.White);
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

    class LookMenu : Menu
    {
        public LookMenu(DynamicObject o)
        {
            PauseWhenOpened = false;

            Text = o.Name + " - " + o.Lore + "\n";

            if (Game1.CursorPosition.X < Zone.WIDTH / 2)
                Position = new OrderedPair(Game1.ScreenSize.X - Size.X, Position.Y);
            else
                Position = new OrderedPair(0, Position.Y);
        }
    }
    class InventoryMenu : Menu
    {
        private int selectedIdx = 0;
        private readonly int itemCount; // how many Item() objects

        private DynamicObject container; // the object whose inventory we are examining
        public InventoryMenu(DynamicObject _container)
        {
            container = _container;

            StringBuilder sb = new StringBuilder();
            sb.Append("[" + container.Name + "]\n");

            foreach (Item item in container.Children)
            {
                itemCount++;

                sb.Append(item.Name);
                if (item.Count > 1)
                    sb.Append(" x" + item.Count);

                sb.Append("\n");
            }

            Text = sb.ToString();
        }

        public override void HandleInput(List<Control> controls)
        {
            base.HandleInput(controls);

            selectedIdx = Input.PickIndex(controls, selectedIdx, itemCount);

            if(controls.Contains(Control.Enter))
            {
                /* CODE TO TRANSFER AN ITEM TO THE OTHER CONTAINER
                DynamicObject item = container1.children[selectedIdx];
                container1.children.RemoveAt(selectedIdx);
                container2.children.Add(item);
                */

                // CODE TO OPEN INTERACTIONS MENU
                InteractionMenu m = new InteractionMenu(container.Children[selectedIdx]);
                (int, int) coords = IndexToCoords(selectedIdx);
                m.Position = new OrderedPair(coords.Item1, coords.Item2);
                
                Game1.OpenMenu(m);
            }
        }

        public override void Draw()
        {
            base.Draw();

            (int, int) coords = IndexToCoords(selectedIdx);
            spriteBatch.Draw(cursorTexture, new Rectangle(coords.Item1, coords.Item2, 20, 20), Color.White);
        }

        private (int, int) IndexToCoords(int index)
        {
            int x = (int)Position.X + 160;
            int y = (int)Position.Y + selectedIdx * 22 + 28;
            return (x, y);
        }
    }
    class NearbyObjectsMenu : Menu
    {
        public NearbyObjectsMenu(List<DynamicObject> nearbyObjects)
        {
            PauseWhenOpened = false;

            string t = "[Nearby]\n";

            foreach(DynamicObject o in nearbyObjects)
            {
                t += o.Name + "\n";
            }

            Text = t;

            Size = (OrderedPair)font.MeasureString(wrappedText);
            Position = new OrderedPair(Game1.ScreenSize.X - (Size.X + 5), Game1.ScreenSize.Y - (Size.Y + 5) );
        }
    }
    class GetMenu : Menu
    {
        public GetMenu(List<Item> items)
        {
            string t = "Get\n";

            foreach (DynamicObject o in items)
            {
                t += o.Name + "\n";
            }

            Text = t;
        }
    }
    class InteractionMenu : Menu
    {
        DynamicObject objectToInteractWith;
        public InteractionMenu(DynamicObject _objectToInteractWith)
        {
            BackgroundColor = Color.DarkGreen;
            objectToInteractWith = _objectToInteractWith;

            foreach(Interaction i in objectToInteractWith.Interactions)
            {
                Text += i.ToString() + "\n";
            }
        }
    }
    class BookMenu : Menu
    {
        private int currentPageIdx = 0; // what page the player is looking at
        private readonly string bookId;
        private readonly int pageCount;
        public BookMenu(string _bookId)
        {
            bookId = _bookId;
            pageCount = GetPageCount();
            Text = GetPageText(0);
        }

        public override void Draw()
        {
            base.Draw();

            // draw the page number in the top right
            string pageNumString = (currentPageIdx + 1).ToString() + " / " + pageCount;

            OrderedPair pageNumSize = (OrderedPair)font.MeasureString(pageNumString);
            OrderedPair pageNumPos = Position + new OrderedPair(Size.X, 0) - pageNumSize;
            spriteBatch.DrawString(font, pageNumString, pageNumPos, Color.White);
        }
        public override void HandleInput(List<Control> controls)
        {
            base.HandleInput(controls);

            // change the page if the player presses the right controls
            currentPageIdx = Input.PickIndex(controls, currentPageIdx, pageCount, Control.Right, Control.Left);
            Text = GetPageText(currentPageIdx);
        }
        private int GetPageCount()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(BOOK_PATH);

            XmlNode bookNode = xmlDoc.GetElementById(bookId);
            int pageCount = bookNode.ChildNodes.Count;

            return pageCount;
        }
        private string GetPageText(int pageNum)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(BOOK_PATH);

            XmlNode bookNode = xmlDoc.GetElementById(bookId);
            XmlNode page = bookNode.ChildNodes[pageNum];
            string text = page.InnerText;

            return text;
        }
    }
}
