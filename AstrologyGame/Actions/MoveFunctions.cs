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
            return true;
        }
        public static void Move(Entity mover, OrderedPair destination)
        {
            mover.GetComponent<Position>().Pos = destination;
        }
    }
}
