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

        public StatusMenu(Entity e)
        {
            Attributes attributes = e.GetComponent<Attributes>();
            Text = $"P: {attributes.Prowess}\n" +
                $"F: {attributes.Concentration}\n" +
                $"C: {attributes.Concentration}\n" +
                $"V: {attributes.Vigor}\n\n" +
                
                $"Health: {attributes.Health}/{attributes.MaxHealth}";
        }
    }
}
