using System.Collections.Generic;
using Microsoft.Xna.Framework;

using AstrologyGame.Entities.ComponentInteractions;

namespace AstrologyGame.Entities
{
    public class Display : EntityComponent
    {
        public bool ShouldRender { get; set; } = true;
        public string Name { get; set; } = "default";
        public string Lore { get; set; } = "default";
        public string TextureName { get; set; } = "default";
        public Color Color { get; set; } = Color.White;

        public Display()
        {
            Interaction infoInteraction = new Interaction();
            infoInteraction.Perform = (Entity e) => Info();
            infoInteraction.Name = "Info";
            interactions.Add(infoInteraction);
        }

        private void Info()
        {
            Menu infoMenu = new Menu();
            infoMenu.Text = Lore;
            Game1.OpenMenu(infoMenu);
        }
    }
}
