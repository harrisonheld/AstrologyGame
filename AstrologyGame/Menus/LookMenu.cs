
using AstrologyGame.Entities;
using AstrologyGame.Components;
using AstrologyGame.MapData;

namespace AstrologyGame.Menus
{
    public class LookMenu : Menu
    {
        public LookMenu(Entity o)
        {
            PauseWhenOpened = false;

            Display d = o.GetComponent<Display>();

            Text = d.Name + " - " + d.Lore + "\n";

            if (Game1.CursorPosition.X < Zone.WIDTH / 2)
                Position = new OrderedPair(Game1.ScreenSize.X - Size.X, Position.Y);
            else
                Position = new OrderedPair(0, Position.Y);
        }
    }
}
