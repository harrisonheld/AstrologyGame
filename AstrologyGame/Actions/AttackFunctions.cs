using System;
using System.Collections.Generic;
using System.Text;

using AstrologyGame.Entities;
using AstrologyGame.Components;

using AstrologyGame.MapData;

namespace AstrologyGame.Systems
{
    public static class AttackFunctions
    {
        public static void BumpAttack(Entity attacker, Entity reciever)
        {
            Attributes recieverAttr = reciever.GetComponent<Attributes>();
            Attributes attackerAttr = attacker.GetComponent<Attributes>();

            recieverAttr.Health -= attackerAttr.Prowess;
            if(recieverAttr.Health < 0) // death
            {
                Zone.RemoveEntity(reciever);
            }
        }
    }
}
