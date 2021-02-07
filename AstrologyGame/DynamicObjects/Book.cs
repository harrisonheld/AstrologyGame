using System;
using System.Collections.Generic;
using System.Text;

using System.Xml;

using AstrologyGame.MapData;

namespace AstrologyGame.DynamicObjects
{
    class Book : Item, IReadable
    {
        List<Interaction> IInteractable.Interactions { get { return new List<Interaction>() { Interaction.Get, Interaction.Drop, Interaction.Read }; } }

        private string bookId;
        public Book(string _bookId)
        {
            bookId = _bookId;
            Name = bookId;
            TextureName = "book1";
        }

        public void BeRead(DynamicObject reader)
        {
            if(reader == Zone.Player)
            {
                BookMenu menu = new BookMenu(bookId);
                Game1.OpenMenu(menu);
            }
        }
    }
}
