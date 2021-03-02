namespace AstrologyGame.Entities
{
    public class Book : EntityComponent
    {
        public string bookId;

        public Book()
        {

        }

        public override bool FireEvent(ComponentEvent cEvent)
        {
            if(cEvent.EventId == EventId.Read)
            {
                BookMenu menu = new BookMenu(bookId);
                Game1.OpenMenu(menu);
                return true;
            }   

            return false;
        }
    }
}
