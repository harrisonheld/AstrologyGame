using System.Collections.Generic;

using System.Xml;
using System.Text;

using Microsoft.Xna.Framework;

namespace AstrologyGame.Menus
{
    public class BookMenu : SelectMenu
    {
        public override bool DrawCursor => false;

        private readonly string bookId;

        public BookMenu(string _bookId)
        {
            IncrementControl = Control.Right;
            DecrementControl = Control.Left;

            bookId = _bookId;

            Size = new OrderedPair(500, 500);

            // add two blank lines
            Text =  GetPageText(0);
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
            xmlDoc.Load(GameManager.BOOK_PATH);

            XmlNode bookNode = xmlDoc.GetElementById(bookId);
            int pageCount = bookNode.ChildNodes.Count;

            return pageCount;
        }
        private string GetPageText(int pageNum)
        {
            StringBuilder sb = new StringBuilder(); // for building the page text

            // load the book from xml
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(GameManager.BOOK_PATH);
            XmlNode bookNode = xmlDoc.GetElementById(bookId);

            // append the title in brackets and add some whitespace
            string title = bookNode.Attributes.GetNamedItem("title").Value;
            sb.Append($"[{title}]\n\n");

            // append the book text
            string pageText = bookNode.ChildNodes[pageNum].InnerText;
            sb.Append(pageText);

            return sb.ToString();
        }
    }
}
