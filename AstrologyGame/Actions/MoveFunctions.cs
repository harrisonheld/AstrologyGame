using System;
using System.Collections.Generic;
using System.Text;

using AstrologyGame.Entities;
using AstrologyGame.Components;
using AstrologyGame.MapData;

namespace AstrologyGame
{
    public static class MoveFunctions
    {
        public static bool CanMove(Entity mover, OrderedPair destination)
        {
            // returns true if any entites at the destination position are solid

            List<Entity> entitesAtDestination = Zone.GetEntitiesAtPosition(destination);
            foreach(Entity potentialObstacle in entitesAtDestination)
            {
                if (potentialObstacle.HasComponent<Solid>())
                    return false;
            }

            return true;
        }
        public static void Move(Entity mover, OrderedPair destination)
        {
            mover.GetComponent<Position>().Pos = destination;
        }
    }
}
