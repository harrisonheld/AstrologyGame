namespace AstrologyGame.Entities
{
    public class Book : EntityComponent
    {
        public string BookId { get; set; }

        public Book()
        {

        }

        public override bool FireEvent(ComponentEvent cEvent)
        {
            switch(cEvent.EventId)
            {
                case EventId.Read:
                    BookMenu menu = new BookMenu(BookId);
                    Game1.OpenMenu(menu);
                    return true;
            }

            return false;
        }
    }
}
