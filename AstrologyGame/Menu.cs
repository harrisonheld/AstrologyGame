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

        public const string BOOK_PATH = @"C:\Users\Held\source\repos\AstrologyGame\AstrologyGame\books.xml";
        public const string BACKGROUND_TEXTURE_NAME = "black";
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

    // a menu where you can select one thing in a vertical list
    abstract class SelectMenu : Menu
    {
        protected int selectedIndex = 0;
        protected int selectionCount = 1; // how many things are there to select?

        protected OrderedPair cursorCoords;

        // what controls are used to move the selection
        public Control IncrementControl { get; set; } = Control.Down;
        public Control DecrementControl { get; set; } = Control.Up;
        public Control SelectControl { get; set; } = Control.Enter;

        // the user has pressed enter (or whatever key is used to signify selecting)
        public abstract void SelectionMade();

        public override void HandleInput(List<Control> controls)
        {
            base.HandleInput(controls);

            // increment or decrement index
            if (controls.Contains(IncrementControl))
                selectedIndex++;
            else if (controls.Contains(DecrementControl))
                selectedIndex--;
            // clamp index in case it went negative or went too high
            ClampSelection();
            // make a selection if user hits select, and there is something to select
            if (controls.Contains(SelectControl) && selectionCount != 0)
                SelectionMade();
        }
        private void ClampSelection()
        {
            if (selectionCount != 0)
                selectedIndex = Math.Clamp(selectedIndex, 0, selectionCount - 1);
        }
        public override void Refresh()
        {
            ClampSelection();
            base.Refresh();
        }
        public override void Draw()
        {
            base.Draw();

            if(selectionCount != 0)
            {
                CalculateCursorCoords();
                Utility.SpriteBatch.Draw(cursorTexture, new Rectangle(cursorCoords.X, cursorCoords.Y, 20, 20), Color.White);
            }
        }

        protected void CalculateCursorCoords()
        {
            int x = Position.X + 160;
            int y = Position.Y + selectedIndex * 22 + 28;
            cursorCoords = new OrderedPair(x, y);
        }
    }

    class InventoryMenu : SelectMenu
    {
        private DynamicObject container; // the object whose inventory we are examining

        public InventoryMenu(DynamicObject _container)
        {
            container = _container;
            selectionCount = _container.Children.Count;

            Refresh();
        }
        public override void Refresh()
        {
            // add all the item names to the text
            StringBuilder sb = new StringBuilder();
            sb.Append($"[{container.Name}]\n");
            foreach (Item item in container.Children)
            {

                sb.Append(item.Name);

                // add equip slot if its equipped
                if (item is IEquipment)
                {
                    IEquipment e = item as IEquipment;
                    if (container.HasEquipped(e))
                    {
                        string slot = e.EquipSlot.ToString();
                        sb.Append($" ({slot})");
                    }
                }

                // add item count if its more than 1
                if (item.Count > 1)
                    sb.Append($" (x{item.Count})");

                sb.Append("\n");
            }

            selectionCount = container.Children.Count;
            Text = sb.ToString();
            base.Refresh();
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
            else
            {
                // otherwise, forbid dropping, as it is someone or something else's inventory
                forbiddenInteractions.Add(Interaction.Drop);
            }

            InteractionMenu menu = new InteractionMenu(container.Children[selectedIndex], forbiddenInteractions);
            menu.Position = cursorCoords;

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

            Size = (OrderedPair)Font.MeasureString(wrappedText);
            Position = new OrderedPair(Game1.ScreenSize.X - (Size.X + 5), Game1.ScreenSize.Y - (Size.Y + 5) );
        }
    }
    class GetMenu : SelectMenu
    {
        private List<Item> items;

        public GetMenu(List<Item> _items)
        {
            items = _items;
            Refresh();
        }
        public override void Refresh()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[Get]\n");

            foreach (Item i in items)
            {
                sb.Append($"{i.Name}");
                if (i.Count > 1)
                    sb.Append($" x{i.Count}");
                sb.Append("\n");
            }

            Text = sb.ToString();

            // update maxIndex in case item count changed
            selectionCount = items.Count;

            base.Refresh();
        }

        public override void HandleInput(List<Control> controls)
        {
            base.HandleInput(controls);

            // if player hits tab, get all items
            if(controls.Contains(Control.Tab))
            {
                foreach (Item i in items)
                    i.Interact(Interaction.Get, Zone.Player);

                Game1.CloseMenu(this);
            }
        }

        public override void SelectionMade()
        {
            items[selectedIndex].Interact(Interaction.Get, Zone.Player);
            items.RemoveAt(selectedIndex);
            Refresh();
        }
    }
    class InteractionMenu : SelectMenu
    {
        DynamicObject objectToInteractWith;
        List<Interaction> interactions;

        /// <param name="forbiddenInteraction">A list of interactions that will not be included in this menu, even if the object to interact with has them available.</param>
        public InteractionMenu(DynamicObject _objectToInteractWith, List<Interaction> forbiddenInteractions)
        {
            BackgroundColor = Color.Black;
            objectToInteractWith = _objectToInteractWith;

            // make a copy of the items interactions so we don't have to worry about changing things in the object itself
            interactions = new List<Interaction>((objectToInteractWith as IInteractable).Interactions);

            // if there are any forbidden interactions
            if (forbiddenInteractions != null)
            {
                // remove them from this menu's interactions
                foreach (Interaction forbidden in forbiddenInteractions)
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

            selectionCount = interactions.Count;
        }

        public override void SelectionMade()
        {
            // close this menu when a selection is made
            Game1.CloseMenu(this);
            // use Zone.Player as interactor because only a Player could have opened an InteractionMenu
            objectToInteractWith.Interact(interactions[selectedIndex], Zone.Player);
            // refresh the menus incase this interaction changed them
            Game1.QueueRefreshAllMenus();
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

            OrderedPair pageNumSize = (OrderedPair)Font.MeasureString(pageNumString);
            OrderedPair pageNumPos = Position + new OrderedPair(Size.X, Size.Y) - pageNumSize;
            pageNumPos -= pageNumPadding;
            Utility.SpriteBatch.DrawString(Font, pageNumString, pageNumPos, Color.White);

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
