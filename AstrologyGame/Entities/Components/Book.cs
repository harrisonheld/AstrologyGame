using System.Collections.Generic;

using AstrologyGame.Menus;

namespace AstrologyGame.Entities.Components
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
            Game1.OpenMenu(menu);
        }
    }
}
