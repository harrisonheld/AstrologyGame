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
        List<string> ids;

        public DevSpawnMenu()
        {
            ids = EntityFactory.GetIdsInXML();
            selectionCount = ids.Count;

            foreach(string id in ids)
            {
                Text += id + "\n";
            }
        }

        public override void SelectionMade()
        {
            string id = ids[selectedIndex];
            Entity e = EntityFactory.EntityFromId(id, 0, 0);
            Zone.AddEntity(e);
        }
    }
}
