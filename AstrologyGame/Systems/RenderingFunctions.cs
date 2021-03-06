﻿using System;
using System.Collections.Generic;
using System.Text;

using AstrologyGame.MapData;
using AstrologyGame.Entities;
using AstrologyGame.Components;

namespace AstrologyGame.Systems
{
    public static class RenderingFunctions
    {
        // render the Zone
        public static void RenderZone()
        {
            // draw tiles
            for (int y = 0; y < Zone.HEIGHT; y++)
                for (int x = 0; x < Zone.WIDTH; x++)
                    Utility.DrawEntity(Zone.GetTileAtPosition((x, y)), x, y);

            // draw other entities
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
