using System;
using System.Collections.Generic;
using System.Text;

using System.Xml;

using AstrologyGame.MapData;

namespace AstrologyGame.DynamicObjects
{
    class Book : Item
    {
        public string bookId;
        public Book(string _bookId)
        {
            interactions.Add(Interaction.Read);

            bookId = _bookId;
            Name = bookId;
            TextureName = "book1";
        }

        protected override void Read(DynamicObject reader)
        {
            if(reader == Zone.Player)
            {
                BookMenu menu = new BookMenu(bookId);
                Game1.OpenMenu(menu);
            }
        }
    }
}
