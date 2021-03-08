
using Microsoft.Xna.Framework;

namespace AstrologyGame.Entities
{
    public class Display : EntityComponent
    {
        public bool ShouldRender { get; set; } = true;
        public string Name { get; set; } = "default";
        public string Lore { get; set; } = "default";
        public string TextureName { get; set; } = "default";
        public Color Color { get; set; } = Color.White;
    }
}
