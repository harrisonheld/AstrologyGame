using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AstrologyGame.DynamicObjects
{
    public abstract class Tile : DynamicObject
    {
        public Tile()
        {
            TextureName = "dots3x3";
        }
    }
    class DebugTile : Tile
    { 
        public DebugTile()
        {
            Name = "debug tile";
            Lore = "It's pure black, and very easy to read debug text on.";
            TextureName = "debugTile";
            color = new Color(25, 25, 25);
        }
    }

    class Stone : Tile
    {
        public Stone()
        {
            Name = "stone";
            Lore = "Nonmetallic mineral matter.";
            color = new Color(86, 72, 67);
        }
    }
    class Sand : Tile
    {
        public Sand()
        {
            Name = "sand";
            Lore = "sand";
            color = Color.LightYellow;
        }
    }
    class BleachedSand : Sand
    {
        public BleachedSand()
        {
            TextureName = "dots2x2";
            color = new Color(color.R + 10, color.G + 10, color.B + 10);
        }
    }

    class Offal: Tile
    {
        public Offal()
        {
            TextureName = "dots3x3";
            Name = "boiling offal";
            Lore = "A gratuitous mess of blood and guts. Occasionally, it gives rise to something resembling life. " +
                "The majority of these 'somethings' are amorphous blobs or single body parts, " +
                "but some are so unfortunate as to be viable.";
            color = Color.Red;
        }

        public override void AnimationTurn()
        {
            color = RandomRed();
        }
        // generate a random shade of red
        private Color RandomRed()
        {
            Random random = new Random();

            int red = random.Next(192, 256);
            int green = random.Next(0, 32);
            int blue = random.Next(0, 32);

            return new Color(red, green, blue);
        }
    }
}
