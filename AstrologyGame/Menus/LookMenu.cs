
using AstrologyGame.Entities;
using AstrologyGame.Components;
using AstrologyGame.MapData;

namespace AstrologyGame.Menus
{
    public class LookMenu : Menu
    {
        public override bool TakesInput { get; } = false;

        public LookMenu(Entity o)
        {
            Display d = o.GetComponent<Display>();

            Text = d.Name + " - " + d.Lore + "\n";
        }
    }
}
