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
            .AddNecessary(typeof(AI))
            .AddNecessary(typeof(EnergyHaver))
            .AddForbidden(typeof(PlayerControlled));

        void ISystem.OperateOnEntity(Entity e)
        {
            EnergyHaver actionTakerComp = e.GetComponent<EnergyHaver>();
            AI ai = e.GetComponent<AI>();

            if (ai.Target == null)
                if (!AssignTarget(e))
                    return;

            if (actionTakerComp.CanTakeAction())
            {
                actionTakerComp.Energy -= Utility.COST_MOVE;

                Position posComp = e.GetComponent<Position>();
                Position targetPosComp = ai.Target.GetComponent<Position>();

                OrderedPair towards = Utility.OrderedPairTowards(posComp.Pos, targetPosComp.Pos);
                posComp.Pos += towards;
            }
        }

        /// <summary>
        /// Attempts to assign a target to the given entity's AI.
        /// </summary>
        /// <param name="requiresTarget">The entity that requires a target.</param>
        /// <returns>True if a target was succesfully found, false if not.</returns>
        private bool AssignTarget(Entity requiresTarget)
        {
            // currently just sets the target to the first Human found

            AI ai = requiresTarget.GetComponent<AI>();
            ai.Target = null;

            foreach (Entity potentialTarget in Zone.Entities)
            {
                if (potentialTarget.HasComponent<FactionInfo>())
                    if (potentialTarget.GetComponent<FactionInfo>().Faction == Faction.Human)
                    {
                        ai.Target = potentialTarget;
                        return true;
                    }
            }

            return false;
        }
    }
}
