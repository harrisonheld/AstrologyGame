using System;
using System.Collections.Generic;
using System.Text;

namespace AstrologyGame.Components
{
    class Gas : Component
    {
        public int Density { get; set; } // how much gas is in this cloud
        public int Viscosity { get; set; } // the rate at which the gas will spread itself out
    }
}
