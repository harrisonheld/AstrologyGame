using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace AstrologyGame.Entities.Components
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
            interactions = new List<Interaction>()
            {
                new Interaction()
                {
                    Name = "Info",
                    Perform = (Entity e) => ShowInfo(),
                }
            };
        }

        private void ShowInfo()
        {
            Menu infoMenu = new Menu();
            infoMenu.Text = Lore;
            Game1.OpenMenu(infoMenu);
        }
    }
}
