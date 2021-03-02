using System.Collections.Generic;

using System.Xml;

using Microsoft.Xna.Framework;

namespace AstrologyGame
{
    class BookMenu : SelectMenu
    {
        private readonly string bookId;

        public BookMenu(string _bookId)
        {
            DrawCursor = false;
            IncrementControl = Control.Right;
            DecrementControl = Control.Left;

            bookId = _bookId;
            selectionCount = GetPageCount(); // how many pages there are

            Size = new OrderedPair(500, 500);

            Text = GetPageText(0);
        }

        public override void Draw()
        {
            base.Draw();

            // draw the page number in the top right
            string pageNumString = (selectedIndex + 1).ToString() + " / " + selectionCount;
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
            Text = GetPageText(selectedIndex);
        }
        public override void SelectionChanged()
        {
            Text = GetPageText(selectedIndex);
        }
        private int GetPageCount()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Utility.BOOK_PATH);

            XmlNode bookNode = xmlDoc.GetElementById(bookId);
            int pageCount = bookNode.ChildNodes.Count;

            return pageCount;
        }
        private string GetPageText(int pageNum)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Utility.BOOK_PATH);

            XmlNode bookNode = xmlDoc.GetElementById(bookId);
            XmlNode page = bookNode.ChildNodes[pageNum];
            string text = page.InnerText;

            return text;
        }
    }
}
