using System;
using System.Collections.Generic;
using System.Text;

using AstrologyGame.Entities;
using AstrologyGame.Entities.Components;
using AstrologyGame.MapData;

namespace AstrologyGame.Systems
{
    public static class EnergyRechargingSystem
    {
        private static readonly ComponentFilter filter = new ComponentFilter()
             .AddNecessary(typeof(ActionTaker));

        public static void Run()
        {
            foreach(Entity e in Zone.Entities.FindAll(filter.Match))
            {
                ActionTaker comp = e.GetComponent<ActionTaker>();
                comp.Energy += comp.Speed;

                if (comp.Energy >= Utility.ENERGY_CAP)
                    comp.Energy = Utility.ENERGY_CAP;
            }
        }
    }
}
