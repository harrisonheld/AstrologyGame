using System;
using System.Collections.Generic;
using System.Text;

using AstrologyGame.Entities;
using AstrologyGame.Entities.Components;
using AstrologyGame.MapData;

namespace AstrologyGame.Systems
{
    public static class HealthSystem
    {
        public static void Run()
        {
            // use a while loop because removing entities involved in a foreach() loop will cause an exception
            int e = 0;
            while (e < Zone.Entities.Count)
            {
                if (Zone.Entities[e].HasComponent<Health>())
                {
                    Health health = Zone.Entities[e].GetComponent<Health>();

                    if (health.HitPoints <= 0)
                    {
                        Zone.RemoveEntityAt(e);
                    }
                }

                e++;
            }
        }
    }
}
