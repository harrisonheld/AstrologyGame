﻿using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Input;

namespace AstrologyGame
{
    static class Input
    {
        private static List<Control> controls = new List<Control>() { };
        public static List<Control> Controls { get { return controls; } }

        public static void Update()
        {
            KeyboardState kbState = Keyboard.GetState();

            Clear();

            #region Directional Keys
            if (kbState.IsKeyDown(Keys.Up))
                controls.Add(Control.Up);

            if (kbState.IsKeyDown(Keys.Down))
                controls.Add(Control.Down);

            if (kbState.IsKeyDown(Keys.Right))
                controls.Add(Control.Right);

            if (kbState.IsKeyDown(Keys.Left))
                controls.Add(Control.Left);

            if (kbState.IsKeyDown(Keys.NumPad5))
                controls.Add(Control.Here);
            #endregion

            #region Actions
            if (kbState.IsKeyDown(Keys.Space))
                controls.Add(Control.Interact);
            if (kbState.IsKeyDown(Keys.G))
                controls.Add(Control.Get);

            if (kbState.IsKeyDown(Keys.L))
                controls.Add(Control.Look);

            if (kbState.IsKeyDown(Keys.I))
                controls.Add(Control.Inventory);

            if (kbState.IsKeyDown(Keys.Q))
                controls.Add(Control.Favorites);

            if (kbState.IsKeyDown(Keys.Escape))
                controls.Add(Control.Back);
            #endregion

            #region Misc.
            if (kbState.IsKeyDown(Keys.F))
                controls.Add(Control.Fullscreen);
            if (kbState.IsKeyDown(Keys.Enter))
                controls.Add(Control.Enter);
            if (kbState.IsKeyDown(Keys.Tab))
                controls.Add(Control.Tab);
            #endregion

            #region Modifier Keys
            if (kbState.IsKeyDown(Keys.LeftControl))
                controls.Add(Control.Alternate);
            #endregion

            #region Developer Functions
            if (kbState.IsKeyDown(Keys.OemTilde))
                controls.Add(Control.DevInfo);
            // D1 is the number 1 on the row of numbers, and so on
            if (kbState.IsKeyDown(Keys.D1))
                controls.Add(Control.DevFunc1);
            if (kbState.IsKeyDown(Keys.D2))
                controls.Add(Control.DevFunc2);
            if (kbState.IsKeyDown(Keys.D3))
                controls.Add(Control.DevFunc3);
            #endregion
        }

        public static void Clear()
        {
            controls.Clear();
        }

        public static OrderedPair ControlsToMovePair(List<Control> controls)
        {
            int x = 0;
            int y = 0;

            // cardinal directions
            if (controls.Contains(Control.Down))
                y++;
            if (controls.Contains(Control.Up))
                y--;
            if (controls.Contains(Control.Right))
                x++;
            if (controls.Contains(Control.Left))
                x--;

            // diagonal directions
            if (controls.Contains(Control.UpRight))
            {
                x++;
                y--;
            }
            if (controls.Contains(Control.UpLeft))
            {
                x--;
                y--;
            }
            if (controls.Contains(Control.DownRight))
            {
                x++;
                y++;
            }
            if (controls.Contains(Control.DownLeft))
            {
                x--;
                y++;
            }

            // make corrections in case more than one directional key is pressed
            if (x > 1)
                x = 1;
            if (x < -1)
                x = -1;
            if (y > 1)
                y = 1;
            if (y < -1)
                y = -1;

            return new OrderedPair(x, y);
        }
    }

    public enum Control
    {
        // directions
        Up,
        Down,
        Left,
        Right,
        UpRight,
        UpLeft,
        DownRight,
        DownLeft,
        Here, // space or 5 on keypad

        // actions
        Interact,
        Get,
        Look,
        Inventory,
        Favorites, // the weapon wheel
        Back, // leave a menu

        // misc
        Fullscreen,
        Enter,
        Tab,

        // modifier keys
        Alternate,

        // dev functions
        DevInfo, // shows debug info, kinda like the F3 key in Minecraft
        DevFunc1,
        DevFunc2,
        DevFunc3,
    }
}
