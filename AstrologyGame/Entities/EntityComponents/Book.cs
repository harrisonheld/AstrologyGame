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

            Interaction fuckYouInteraction = new Interaction();
            fuckYouInteraction.Perform = (Entity e) => FuckYou();
            fuckYouInteraction.Name = "Cool Easter Egg!";
            interactions.Add(fuckYouInteraction);

            Interaction kickInteraction = new Interaction();
            kickInteraction.Perform = (Entity e) => Kick();
            kickInteraction.Name = "Kick";
            interactions.Add(kickInteraction);
        }

        private void Read()
        {
            BookMenu menu = new BookMenu(BookId);
            Game1.OpenMenu(menu);
        }

        private void FuckYou()
        {
            Menu fuckYouMenu = new Menu();
            fuckYouMenu.Text = "you found the easter egg! FUCK YOU";
            fuckYouMenu.BackgroundColor = Microsoft.Xna.Framework.Color.DeepPink;
            Game1.OpenMenu(fuckYouMenu);
        }

        private void Kick()
        {
            ParentEntity.GetComponent<Position>().X += 1;
        }
    }
}
