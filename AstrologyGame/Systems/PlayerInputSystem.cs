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

        void ISystem.OperateOnEntity(Entity entity)
        {
            while(Input.Controls.Count == 0)
            {
                Utility.Log("holy fuck");
            }

            Utility.Log(Input.Controls[0]);
        }
    }
}
