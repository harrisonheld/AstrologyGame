using System.Collections.Generic;

using System.Text;

using AstrologyGame.Entities;
using AstrologyGame.MapData;

namespace AstrologyGame
{
    class GetMenu : SelectMenu
    {
        private List<Entity> entities;

        public GetMenu(List<Entity> _entities)
        {
            this.entities = _entities;
            Refresh();
        }
        public override void Refresh()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[Get]\n");

            foreach (Entity e in entities)
            {
                Display d = e.GetComponent<Display>();
                Item i = e.GetComponent<Item>();

                sb.Append($"{d.name}");
                if (i.count > 1)
                    sb.Append($" x{i.count}");
                sb.Append("\n");
            }

            Text = sb.ToString();

            // update maxIndex in case item count changed
            selectionCount = entities.Count;

            base.Refresh();
        }

        public override void HandleInput(List<Control> controls)
        {
            base.HandleInput(controls);

            // if player hits tab, get all items
            if(controls.Contains(Control.Tab))
            {
                foreach (Entity e in entities)
                {
                    ComponentEvent getItemEvent = new ComponentEvent(EventId.AddItem);
                    getItemEvent[ParameterId.Target] = e;
                    Zone.Player.FireEvent(getItemEvent);
                }

                Game1.CloseMenu(this);
            }
        }

        public override void SelectionMade()
        {
            ComponentEvent getItemEvent = new ComponentEvent(EventId.AddItem);
            getItemEvent[ParameterId.Target] = entities[selectedIndex];
            Zone.Player.FireEvent(getItemEvent);

            Refresh();
        }
    }
}
