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
                wrappedText = Utility.WrapText(font, text, Size.X);
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
        public virtual void RegenerateText()
        {
            // do nothing. menus that are procedurally generated (like inventories) should override this to refresh their text
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

    // a menu where you can select one thing in a vertical list
    abstract class SelectMenu : Menu
    {
        protected int selectedIndex = 0;
        protected int maxIndex = -1;
        // what controls are used to move the selection
        public Control IncrementControl { get; set; } = Control.Down;
        public Control DecrementControl { get; set; } = Control.Up;
        public Control SelectControl { get; set; } = Control.Enter;

        // the user has pressed enter (or whatever key is used to signify selecting)
        public abstract void SelectionMade();

        public override void HandleInput(List<Control> controls)
        {
            base.HandleInput(controls);

            // if maxIndex hasn't been reassigned yet
            if (maxIndex == -1)
                return;

            // increment or decrement index
            if (controls.Contains(IncrementControl))
                selectedIndex++;
            else if (controls.Contains(DecrementControl))
                selectedIndex--;
            // clamp index in case it went negative or went too high
            selectedIndex = Math.Clamp(selectedIndex, 0, maxIndex);

            if (controls.Contains(SelectControl))
                SelectionMade();
        }

        public override void Draw()
        {
            base.Draw();

            // coords to draw the select cursor at
            OrderedPair coords = IndexToCoords(selectedIndex);
            spriteBatch.Draw(cursorTexture, new Rectangle(coords.X, coords.Y, 20, 20), Color.White);
        }

        protected OrderedPair IndexToCoords(int index)
        {
            int x = Position.X + 160;
            int y = Position.Y + index * 22 + 28;
            return (x, y);
        }
    }

    class InventoryMenu : SelectMenu
    {
        private DynamicObject container; // the object whose inventory we are examining

        public InventoryMenu(DynamicObject _container)
        {
            container = _container;
            maxIndex = _container.Children.Count - 1;

            RegenerateText();
        }
        public override void RegenerateText()
        {
            // add all the item names to the text
            StringBuilder sb = new StringBuilder();
            sb.Append($"[{container.Name}]\n");
            foreach (Item item in container.Children)
            {
                sb.Append(item.Name);
                // add item count if its more than 1
                if (item.Count > 1)
                    sb.Append($" (x{item.Count})");

                sb.Append("\n");
            }

            Text = sb.ToString();
        }
        public override void SelectionMade()
        {
            List<Interaction> forbiddenInteractions = new List<Interaction>();
            // if this is the players inventory...
            if(container == Zone.Player)
            {
                // ...forbid getting the item. It makes no sense for the player to get an item from their OWN inventory.
                forbiddenInteractions.Add(Interaction.Get);
            }

            InteractionMenu menu = new InteractionMenu(container.Children[selectedIndex], forbiddenInteractions);
            OrderedPair coords = IndexToCoords(selectedIndex);
            menu.Position = coords;

            Game1.OpenMenu(menu);
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
    class InteractionMenu : SelectMenu
    {
        DynamicObject objectToInteractWith;
        List<Interaction> interactions;

        /// <param name="forbiddenInteraction">A list of interactions that will not be included in this menu, even if the object to interact with has them available.</param>
        public InteractionMenu(DynamicObject _objectToInteractWith, List<Interaction> forbiddenInteraction = null)
        {
            BackgroundColor = Color.Black;
            objectToInteractWith = _objectToInteractWith;
            /* make a copy of the list so we can mess with it and not worry about messing up 
             * the interactions inside objectToInteractWith.Interactions */
            interactions = new List<Interaction>(objectToInteractWith.Interactions);

            // if there are any forbidden interactions
            if(forbiddenInteraction != null)
            {
                // remove them from this menu's interactions
                foreach (Interaction forbidden in forbiddenInteraction)
                    interactions.Remove(forbidden);
            }

            // add interactions to the text
            StringBuilder sb = new StringBuilder();
            sb.Append("[Interactions]\n");
            foreach(Interaction i in interactions)
            {
                sb.Append($"{i}\n");
            }
            Text = sb.ToString();

            maxIndex = interactions.Count - 1;
        }

        public override void SelectionMade()
        {
            // close this menu when a selection is made
            Game1.CloseMenu(this);
            // use Zone.Player as interactor because only a Player could have opened an InteractionMenu
            objectToInteractWith.Interact(interactions[selectedIndex], Zone.Player);
            // regenerate the menus incase this interaction changed them
            Game1.RegenerateMenus();
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

            Size = new OrderedPair(500, 500);

            Text = GetPageText(0);
        }

        public override void Draw()
        {
            base.Draw();

            // draw the page number in the top right
            string pageNumString = (currentPageIdx + 1).ToString() + " / " + pageCount;
            OrderedPair pageNumPadding = new OrderedPair(5, 5);

            OrderedPair pageNumSize = (OrderedPair)font.MeasureString(pageNumString);
            OrderedPair pageNumPos = Position + new OrderedPair(Size.X, Size.Y) - pageNumSize;
            pageNumPos -= pageNumPadding;
            spriteBatch.DrawString(font, pageNumString, pageNumPos, Color.White);

        }
        public override void HandleInput(List<Control> controls)
        {
            base.HandleInput(controls);

            // change the page if the player presses the right controls
            //currentPageIdx = Input.PickIndex(controls, currentPageIdx, pageCount, Control.Right, Control.Left);
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
