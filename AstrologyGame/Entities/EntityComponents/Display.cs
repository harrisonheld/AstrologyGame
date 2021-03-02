
using Microsoft.Xna.Framework;

namespace AstrologyGame.Entities
{
    public class Display : EntityComponent
    {
        public bool shouldRender = true;
        public string name = "default";
        public string lore = "default";
        public string textureName = "default";
        public Color color = Color.White;
    }
}
