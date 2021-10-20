using System;
using System.Collections.Generic;
using System.Text;

using AstrologyGame.MapData;
using AstrologyGame.Entities;
using AstrologyGame.Entities.Factories;

namespace AstrologyGame.Menus
{
    public class DevSpawnMenu : SelectMenu
    {
        public DevSpawnMenu()
        {
            foreach (string id in EntityFactory.GetIdsInXML())
            {
                // append a menu item
                MenuItem menuItem = new MenuItem();
                menuItem.Item = id;
                menuItem.Text = id;
                items.Add(menuItem);
            }
        }

        public override void SelectionMade()
        {
            string id = items[selectedIndex].Item as string;
            Entity e = EntityFactory.EntityFromId(id, 0, 0);
            Zone.AddEntity(e);
        }
    }
}
