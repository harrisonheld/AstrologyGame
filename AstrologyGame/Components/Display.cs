using System.Collections.Generic;

using Microsoft.Xna.Framework;

using AstrologyGame.Menus;
using AstrologyGame.Entities;

namespace AstrologyGame.Components
{
    public class Display : Component
    {
        public bool ShouldRender { get; set; } = true;
        public string Name { get; set; } = "default";
        public string Lore { get; set; } = "default";
        public string TextureName { get; set; } = "default";
        public Color Color { get; set; } = Color.White;
    }
}
