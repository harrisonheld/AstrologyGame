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

        // TODO: cache relevant components so they don't have to be found every time the system is run
        public sealed void Run()
        {
            // operate on each entity matching this system's criteria
            foreach (Entity e in Zone.Entities.FindAll(Filter.Match))
                OperateOnEntity(e);
        }

        // call this on systems after they are done running
        public void Reset() { }
        protected void OperateOnEntity(Entity entity);
    }
}
