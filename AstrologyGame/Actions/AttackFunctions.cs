using System;
using System.Collections.Generic;
using System.Text;

using AstrologyGame.Entities;
using AstrologyGame.Components;

namespace AstrologyGame.Systems
{
    public static class AttackFunctions
    {
        public static void BumpAttack(Entity attacker, Entity reciever)
        {
            reciever.GetComponent<Health>().HitPoints -= reciever.GetComponent<Attributes>().Prowess;
        }
    }
}
