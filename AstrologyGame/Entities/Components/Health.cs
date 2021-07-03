using System;
using System.Collections.Generic;
using System.Text;

using AstrologyGame.Systems;

namespace AstrologyGame.Entities.Components
{
    public class Health : Component
    {
        // health
        public int MaxHitPoints { get; set; } = 10;
        public int HitPoints { get; set; } = 10;

        public Health()
        {
            interactions = new List<Interaction>()
            {
                new Interaction()
                {
                    Name = "Attack",
                    Perform = (Entity attacker) => AttackFunctions.BumpAttack(attacker, this.Owner)
                }
            };
        }
    }
}
