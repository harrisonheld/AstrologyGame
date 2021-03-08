using System;
using System.Collections.Generic;
using System.Text;

using AstrologyGame.MapData;

namespace AstrologyGame.Entities
{
    public static class RenderingSystem
    {
        public static void Render()
        {
            // TODO: draw tiles

            foreach (Entity e in Zone.Entities)
            {
                if(e.HasComponent<Position>() && e.HasComponent<Display>())
                {
                    Position p = e.GetComponent<Position>();
                    Utility.DrawEntity(e, p.X, p.Y);
                }
            }
        }
    }
}
