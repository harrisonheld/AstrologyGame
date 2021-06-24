﻿using System;
using System.Text;
using System.Collections.Generic;

using AstrologyGame.Entities.ComponentInteractions;

namespace AstrologyGame.Entities
{
    public abstract class EntityComponent
    {
        private Entity parentEntity;
        public Entity ParentEntity { get { return parentEntity; } }

        protected List<Interaction> interactions { get; set; } = new List<Interaction>();

        public void SetParentEntity(Entity parent)
        {
            parentEntity = parent;
        }

        public List<Interaction> GetInteractions()
        {
            return interactions;
        }
    }
}
