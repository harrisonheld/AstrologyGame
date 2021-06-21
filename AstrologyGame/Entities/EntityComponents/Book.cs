namespace AstrologyGame.Entities
{
    public class Book : EntityComponent
    {
        public string BookId { get; set; }

        public Book()
        {

        }

        public override bool FireEvent(ComponentEvent componentEvent)
        {
            switch(componentEvent)
            {
                case CERead readEvent:
                    BookMenu menu = new BookMenu(BookId);
                    Game1.OpenMenu(menu);
                    return true;

                default:
                    return false;
            }
        }
    }
}
