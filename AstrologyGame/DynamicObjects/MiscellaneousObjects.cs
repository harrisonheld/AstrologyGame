using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using AstrologyGame.MapData;

namespace AstrologyGame.DynamicObjects
{
    public class Sign : DynamicObject
    {
        public Sign()
        {
            TextureName = "sign";
            Name = "sign";
            Lore = "It's a rickety old wooden sign.";
            Color = Color.Chocolate;

            SignText = "[DEV] This is the default contents of a sign.";
        }
        protected override void Read(DynamicObject reader)
        {
            if(reader == Zone.Player)
            {
                Menu m = new Menu();
                m.Text = SignText;
            }
        }
    }
    public class Chest : Item
    {
        public Chest()
        {
            interactions.Insert(0, Interaction.Open);
            TextureName = "chest";
            Name = "chest";
            Lore = "a container";
            Color = Color.Chocolate;
        }

        protected override void Open(DynamicObject opener)
        {
            // TODO: make this code open a TradeMenu so you can swap items with it
            InventoryMenu menu = new InventoryMenu(this);
            Game1.OpenMenu(menu);
        }
    }

    public class MortarStrike : Creature
    {
        public MortarStrike(int radius)
        {
            Quickness = 30;
            Solid = false;
            ShouldRender = false;
            // no interactions.
            interactions = new List<Interaction>();

            // generate indicators
            for (int y = -radius; y <= radius; y++)
            {
                for(int x = -radius; x <= radius; x++)
                {
                    Children.Add(new MortarIndicator(x, y));
                }
            }
        }

        public override void Draw()
        {
            // draw all indicators
            foreach(MortarIndicator indicator in Children)
            {
                Utility.DrawDynamicObject(indicator, this.X + indicator.relX, this.Y + indicator.relY);
            }
        }

        public override void AiTurn()
        {
            base.AiTurn();

            Seek(Zone.Player);
        }
    }
    public class MortarIndicator : DynamicObject
    {
        public int relX, relY; // position relative to the center of the strike
        public MortarIndicator(int _relX, int _relY)
        {
            relX = _relX;
            relY = _relY;

            Color = Color.Red;
            TextureName = "speckled2";
        }
    }
}