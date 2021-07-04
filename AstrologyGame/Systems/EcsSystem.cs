using System;
using System.Collections.Generic;
using System.Text;

using AstrologyGame.Entities;
using AstrologyGame.MapData;

namespace AstrologyGame.Systems
{
    public interface ISystem
    {
        // filter used to find relevant components for the System
        ComponentFilter Filter { get; }

        public void Run()
        {
            // operate on each entity matching this system's criteria
            foreach (Entity e in Zone.Entities.FindAll(Filter.Match))
                OperateOnEntity(e);
        }

        protected void OperateOnEntity(Entity entity);
    }
}
