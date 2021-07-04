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
            .AddNecessary(typeof(Health));

        void ISystem.OperateOnEntity(Entity entity)
        {
            Health comp = entity.GetComponent<Health>();

            // remove the enemy if its HP has run out
            if (comp.HitPoints <= 0)
                Zone.RemoveEntity(entity);
        }
    }
}
