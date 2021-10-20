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
             .AddNecessary(typeof(EnergyHaver));

        void ISystem.OperateOnEntity(Entity entity)
        {
            EnergyHaver comp = entity.GetComponent<EnergyHaver>();
            comp.Energy += comp.Speed;

            if (comp.Energy >= GameManager.ENERGY_CAP)
                comp.Energy = GameManager.ENERGY_CAP;
        }
    }
}
