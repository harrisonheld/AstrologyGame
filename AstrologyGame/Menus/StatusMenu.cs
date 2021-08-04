using System;
using System.Collections.Generic;
using System.Text;

using AstrologyGame.Entities;
using AstrologyGame.Components;

namespace AstrologyGame.Menus
{
    public class StatusMenu : Menu
    {
        public override bool TakesInput => false;

        private Entity entity;

        public StatusMenu(Entity _entity)
        {
            Size = new OrderedPair(200, 150);
            Position = new OrderedPair(400, 200);

            entity = _entity;
            Refresh();
        }

        public override void Refresh()
        {
            Attributes attributes = entity.GetComponent<Attributes>();
            Text = $"P: {attributes.Prowess}\n" +
                $"F: {attributes.Concentration}\n" +
                $"C: {attributes.Concentration}\n" +
                $"V: {attributes.Vigor}\n\n" +

                $"Health: {attributes.Health}/{attributes.MaxHealth}";

            Random rand = new Random();
            Position += new OrderedPair(rand.Next(-2, 3), rand.Next(-2, 3));
        }
    }
}
