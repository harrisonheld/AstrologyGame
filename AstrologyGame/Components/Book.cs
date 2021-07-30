using System.Collections.Generic;

using AstrologyGame.Menus;
using AstrologyGame.Entities;

namespace AstrologyGame.Components
{
    public class Book : Component
    {
        public string BookId { get; set; }

        public Book()
        {
            interactions = new List<Interaction>()
            {
                new Interaction()
                {
                    Name = "Read",
                    Perform = (Entity e) => Read()
                }
            };
        }

        private void Read()
        {
            BookMenu menu = new BookMenu(BookId);
            Game1.AddMenu(menu);
        }
    }
}
