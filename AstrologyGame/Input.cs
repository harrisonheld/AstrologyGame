using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Input;

namespace AstrologyGame
{
    static class Input
    {
        public static List<Control> GetInput()
        {
            KeyboardState kbState = Keyboard.GetState();
            Keys[] keys = kbState.GetPressedKeys();

            List<Control> controls = new List<Control>() { };

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
            {
                if (kbState.IsKeyDown(Keys.LeftControl))
                    controls.Add(Control.InteractSpecific);
                else
                    controls.Add(Control.Interact);
            }
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

            #region Developer Functions
            // D1 is the number 1 on the row of numbers
            if (kbState.IsKeyDown(Keys.D1))
                controls.Add(Control.DevFunc1);

            if (kbState.IsKeyDown(Keys.D2))
                controls.Add(Control.DevFunc2);

            if (kbState.IsKeyDown(Keys.D3))
                controls.Add(Control.DevFunc3);
            #endregion

            #region Misc.
            if (kbState.IsKeyDown(Keys.F))
                controls.Add(Control.Fullscreen);
            if (kbState.IsKeyDown(Keys.Enter))
                controls.Add(Control.Enter);
            #endregion

            return controls;
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
        InteractSpecific, // the user chooses which interaction type they want to do
        Get,
        Look,
        Inventory,
        Favorites, // the weapon wheel
        Back, // leave a menu

        // dev functions
        DevFunc1,
        DevFunc2,
        DevFunc3,

        // misc
        Fullscreen,
        Enter,
    }
}
