using System;
using System.Collections.Generic;
using System.Text;

namespace AstrologyGame.Components
{
    public class EnergyHaver : Component
    {
        // potential idea - two kinds of energy, mental and physical
        // you can do mental activities while being physically exhausted

        public int Speed { get; set; } = 1000; // how much energy is recovered per round
        public int Energy { get; set; } = 0;

        public bool CanTakeAction() => Energy > 0; // can only take actions if we have positive energy
    }
}
