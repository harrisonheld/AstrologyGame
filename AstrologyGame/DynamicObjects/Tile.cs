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
            Color = new Color(25, 25, 25);
        }
    }

    class Stone : Tile
    {
        public Stone()
        {
            Name = "stone";
            Lore = "Nonmetallic mineral matter.";
            Color = new Color(86, 72, 67);
        }
    }
    class Sand : Tile
    {
        public Sand()
        {
            Name = "sand";
            Lore = "sand";
            Color = Color.LightYellow;
        }
    }
    class BleachedSand : Sand
    {
        public BleachedSand()
        {
            TextureName = "dots2x2";
            Color = new Color(Color.R + 10, Color.G + 10, Color.B + 10);
        }
    }

    class Offal: Tile
    {
        public Offal()
        {
            TextureName = "dots3x3";
            Name = "boiling offal";
            Lore = "A gratuitous mess of blood and guts. Occasionally, it gives rise to something organic. " +
                "The majority of these 'somethings' are amorphous giblets or lone body parts, " +
                "but some are so unfortunate as to be viable.";
            Color = Color.Red;
        }

        public override void AnimationTurn()
        {
            Color = RandomRed();
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
