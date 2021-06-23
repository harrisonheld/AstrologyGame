using System;
using System.Text;
using System.Collections.Generic;

using AstrologyGame.Entities.ComponentInteractions;

namespace AstrologyGame.Entities
{
    public abstract class EntityComponent
    {
        public Entity ParentEntity { get; set; }
        protected List<Interaction> interactions { get; set; } = new List<Interaction>();

        public List<Interaction> GetInteractions()
        {
            return interactions;
        }
    }
}
