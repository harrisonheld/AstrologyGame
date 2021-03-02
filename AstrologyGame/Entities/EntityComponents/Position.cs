namespace AstrologyGame.Entities
{
    public class Position : EntityComponent
    {
        public int x = 0;
        public int y = 0;

        public OrderedPair Pos
        {
            get
            {
                return new OrderedPair(x, y);
            }
            set
            {
                x = value.X;
                y = value.Y;
            }
        }
    }
}
