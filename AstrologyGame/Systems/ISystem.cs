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
        protected ComponentFilter Filter { get; }

        // some systems (namely the PlayerInputSystem) take multiple frames to run
        // most systems only take one frame to run so they are Finished by default.
        public bool Finished { get { return true; } }

        public sealed void Run()
        {
            OperateOnAllEntities(Zone.Entities.FindAll(Filter.Match));
        }

        protected void OperateOnAllEntities(List<Entity> entities)
        {
            foreach (Entity e in entities)
                OperateOnEntity(e);
        }

        // call this on systems after they are done running
        public void Reset() { }
        protected void OperateOnEntity(Entity entity);
    }
}
