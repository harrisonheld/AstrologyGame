using Microsoft.Xna.Framework;
using System;

namespace AstrologyGame.Entities
{
    class Wall : Entity
    {
        public Wall()
        {
            Name = "Cyclopean masonry";
            Lore = "The use of large interlocking stones reduces the amount of joints, making the wall stronger. " +
                "Fable attributes its invention to a race of giants led by a one-eyed king named Cyclops.";
            TextureName = "bricks";
            Solid = true;
            Color = Color.Gray;
        }
    }
}
