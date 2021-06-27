namespace AstrologyGame.Entities.Components
{
    public class Position : Component
    {
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;

        public OrderedPair Pos
        {
            get
            {
                return new OrderedPair(X, Y);
            }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }
    }
}
