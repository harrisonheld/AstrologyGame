using System;
using System.Collections.Generic;
using System.Text;

using AstrologyGame.Entities;
using AstrologyGame.Components;
using AstrologyGame.MapData;

namespace AstrologyGame.Systems
{
    public class AISystem : ISystem
    {
        ComponentFilter ISystem.Filter => new ComponentFilter()
            .AddNecessary(typeof(ActionTaker))
            .AddForbidden(typeof(PlayerControlled));

        void ISystem.OperateOnEntity(Entity e)
        {
            ActionTaker actionTakerComp = e.GetComponent<ActionTaker>();
            if (actionTakerComp.CanTakeAction())
            {
                actionTakerComp.Energy -= Utility.COST_MOVE;

                Position position = e.GetComponent<Position>();
                position.X += 1;
            }
        }
    }
}
