using System;
using System.Collections.Generic;
using System.Text;

using AstrologyGame.Entities;
using AstrologyGame.Components;
using AstrologyGame.MapData;

namespace AstrologyGame.Systems
{
    public sealed class HealthSystem : ISystem
    {
        ComponentFilter ISystem.Filter => new ComponentFilter()
            .AddNecessary(typeof(Attributes));

        void ISystem.OperateOnEntity(Entity entity)
        {
            Attributes comp = entity.GetComponent<Attributes>();

            // remove the enemy if its HP has run out
            if (comp.Health <= 0)
                Zone.RemoveEntity(entity);
        }
    }
}
