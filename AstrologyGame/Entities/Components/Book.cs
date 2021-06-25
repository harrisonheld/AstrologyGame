using System.Collections.Generic;

namespace AstrologyGame.Entities.Components
{
    public class Book : EntityComponent
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
