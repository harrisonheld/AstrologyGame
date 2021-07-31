
using System;
using System.Collections.Generic;
using System.Text;

using AstrologyGame.Entities;
using AstrologyGame.Components;
using AstrologyGame.MapData;

namespace AstrologyGame.Systems
{
    public sealed class GasSystem : ISystem
    {
        ComponentFilter ISystem.Filter => new ComponentFilter()
            .AddNecessary(typeof(Gas))
            .AddNecessary(typeof(Position));

        void ISystem.OperateOnEntity(Entity entity)
        {
            Gas gasComp = entity.GetComponent<Gas>();
            gasComp.Density--; // decrease density

            // if gas is depleted, destroy the object and stop there         
            if(gasComp.Density == 0)
            {
                Zone.RemoveEntity(entity);
                return;
            }

            if(entity.HasComponent<Display>())
            {
                Display d = entity.GetComponent<Display>();
                d.TextureName = "speckled" + gasComp.Density;
            }
        }
    }
}
