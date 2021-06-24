using AstrologyGame.Entities.ComponentInteractions;

namespace AstrologyGame.Entities
{
    public class Book : EntityComponent
    {
        public string BookId { get; set; }

        public Book()
        {
            Interaction readInteraction = new Interaction();
            readInteraction.Perform = (Entity e) => Read();
            readInteraction.Name = "Read";
            interactions.Add(readInteraction);
        }

        private void Read()
        {
            BookMenu menu = new BookMenu(BookId);
            Game1.OpenMenu(menu);
        }
    }
}
