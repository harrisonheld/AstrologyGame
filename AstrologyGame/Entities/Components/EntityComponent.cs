using System;
using System.Text;
using System.Collections.Generic;

using AstrologyGame.Entities.Components;

namespace AstrologyGame.Entities.Components
{
    public abstract class Component
    {
        public Entity Owner { get; set; }

        protected List<Interaction> interactions = new List<Interaction>();

        public List<Interaction> GetInteractions()
        {
            return interactions;
        }
    }
}
