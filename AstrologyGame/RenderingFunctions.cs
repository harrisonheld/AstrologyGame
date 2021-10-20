using System;
using System.Collections.Generic;
using System.Text;

using AstrologyGame.MapData;
using AstrologyGame.Entities;
using AstrologyGame.Components;
using AstrologyGame.Menus;
using AstrologyGame.Systems;

using Microsoft.Xna.Framework;

namespace AstrologyGame
{
    public static class RenderingFunctions
    {
        /// <summary>
        /// Draws the entity. If the entity cannot/should not be rendered, it won't be.
        /// </summary>
        /// <param name="entity"></param>
        private static void DrawEntity(Entity entity)
        {
            if (!entity.HasComponent<Display>() || !entity.HasComponent<Position>())
                return;

            Display display = entity.GetComponent<Display>();
            OrderedPair pos = entity.GetComponent<Position>().Pos;

            if (!display.ShouldRender) return;

            GameManager.DrawSprite(display.TextureName, pos.X, pos.Y, display.Color);
        }
        public static void RenderZone()
        {
            // draw tiles
            for (int y = 0; y < Zone.HEIGHT; y++)
                for (int x = 0; x < Zone.WIDTH; x++)
                    DrawEntity(Zone.GetTileAtPosition((x, y)));

            // draw other entities
            foreach (Entity e in Zone.Entities)
                DrawEntity(e);
        }

        public static void RenderLookCursor()
        {
            if (GameManager.LookCursorPos == null) return;

            int cursorX = GameManager.LookCursorPos.X;
            int cursorY = GameManager.LookCursorPos.Y;
            GameManager.DrawSprite(GameManager.LOOK_CURSOR_TEXTURE_NAME, cursorX, cursorY, Color.White);
        }

        public static void RenderMenus(List<Menu> menus)
        {
            foreach(Menu m in menus)
            {
                m.Refresh();
                GameManager.DrawMenu(m);
            }
        }
    }
}
