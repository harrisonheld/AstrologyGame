using System;
using System.Collections.Generic;
using System.Text;

using AstrologyGame.Entities;
using AstrologyGame.Components;
using AstrologyGame.MapData;

namespace AstrologyGame.Systems
{
    public sealed class PlayerInputSystem : ISystem
    {
        ComponentFilter ISystem.Filter => new ComponentFilter()
             .AddNecessary(typeof(PlayerControlled));

        private bool finished = false;
        public bool Finished { get { return finished;  } }

        private int timeSinceLastInput = 0; // in milliseconds
        private bool inputLastUpdate = false;

        void ISystem.OperateOnEntity(Entity entity)
        {
            Input.Update();

            timeSinceLastInput += Game1.DeltaTime;

            // if the Input Stagger time has elapsed, or if the user didn't press anything during the last frame
            // and if the user pressed a control
            if ((timeSinceLastInput > Utility.INPUT_STAGGER || !inputLastUpdate) && Input.Controls.Count != 0)
            {
                // this loop executing means we should handle the input
                if(Input.Controls.Contains(Control.Favorites))
                {
                    finished = true;
                }

                timeSinceLastInput = 0;
            }

            if (Input.Controls.Count == 0)
                inputLastUpdate = false;
            else
                inputLastUpdate = true;
        }

        public void Reset()
        {
            finished = false;
        }
    }
}
