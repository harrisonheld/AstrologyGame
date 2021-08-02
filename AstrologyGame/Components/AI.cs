using System;
using System.Collections.Generic;
using System.Text;

using AstrologyGame.Entities;

namespace AstrologyGame.Components
{
    class AI : Component
    {
        public Entity Target { get; set; } = null;
        public AIState State { get; set; } = AIState.Idle;
    }

    public enum AIState
    { 
        Idle,
        Pursuing
    }
}
