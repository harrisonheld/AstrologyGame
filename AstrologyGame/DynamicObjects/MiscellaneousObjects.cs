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

    public class MortarStrike : DynamicObject
    {

    }
}