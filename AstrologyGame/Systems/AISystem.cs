using System;
using System.Collections.Generic;
using System.Text;

using AstrologyGame.Entities;
using AstrologyGame.Entities.Components;
using AstrologyGame.MapData;

namespace AstrologyGame.Systems
{
    public static class AISystem
    {
        private static readonly ComponentFilter filter = new ComponentFilter()
            .AddNecessary(typeof(ActionTaker))
            .AddForbidden(typeof(PlayerControlled));

        public static void Run()
        {
            foreach(Entity e in Zone.Entities.FindAll(filter.Match))
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
}
