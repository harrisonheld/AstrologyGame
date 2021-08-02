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
            EnergyHaver energyComp = e.GetComponent<EnergyHaver>();
            AI ai = e.GetComponent<AI>();

            if (ai.Target == null)
                AssignTarget(e);

            if (ai.State == AIState.Pursuing && energyComp.CanTakeAction())
            {
                Position posComp = e.GetComponent<Position>();
                Position targetPosComp = ai.Target.GetComponent<Position>();

                // if the entity is adjacent to its target
                if (OrderedPair.Adjacent(posComp.Pos, targetPosComp.Pos))
                {
                    // attack the target
                    energyComp.Energy -= Utility.COST_ATTACK;
                    AttackFunctions.BumpAttack(e, ai.Target);
                }
                else
                {
                    // move towards the target
                    energyComp.Energy -= Utility.COST_MOVE;
                    OrderedPair towards = OrderedPair.Towards(posComp.Pos, targetPosComp.Pos);
                    MoveFunctions.Move(e, posComp.Pos + towards);
                }
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
            FactionInfo faction = requiresTarget.GetComponent<FactionInfo>();

            foreach (Entity potentialTarget in Zone.Entities)
            {
                if (potentialTarget.HasComponent<FactionInfo>())
                {
                    // faction of the potential target
                    Faction targetFaction = potentialTarget.GetComponent<FactionInfo>().Faction;

                    if (faction.GetReputation(targetFaction) < 0)
                    {
                        ai.Target = potentialTarget;
                        ai.State = AIState.Pursuing;
                        return true;
                    }
                }
            }

            ai.Target = null;
            ai.State = AIState.Idle;
            return false;
        }
    }
}
