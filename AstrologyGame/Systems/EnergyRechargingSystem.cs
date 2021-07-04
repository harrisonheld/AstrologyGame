using System;
using System.Collections.Generic;
using System.Text;

using AstrologyGame.Entities;
using AstrologyGame.Components;
using AstrologyGame.MapData;

namespace AstrologyGame.Systems
{
    public sealed class EnergyRechargingSystem : ISystem
    {
        ComponentFilter ISystem.Filter => new ComponentFilter()
             .AddNecessary(typeof(ActionTaker));

        void ISystem.OperateOnEntity(Entity entity)
        {
            ActionTaker comp = entity.GetComponent<ActionTaker>();
            comp.Energy += comp.Speed;

            if (comp.Energy >= Utility.ENERGY_CAP)
                comp.Energy = Utility.ENERGY_CAP;
        }
    }
}
