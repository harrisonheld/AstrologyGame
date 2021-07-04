using System;
using System.Collections.Generic;

using AstrologyGame.Entities;

namespace AstrologyGame.Systems
{
    // Inspiration from: https://github.com/adizhavo/ECS/blob/master/ECS/Filter.cs
    public class ComponentFilter
    {
        private readonly HashSet<Type> Necessary = new HashSet<Type>(); // filters for entities with ALL of these component types
        private readonly HashSet<Type> Forbidden = new HashSet<Type>(); // excludes entities containing ANY of these component types

        public ComponentFilter AddNecessary(params Type[] toAdd)
        {
            foreach (Type t in toAdd)
                Necessary.Add(t);

            return this;
        }
        public ComponentFilter AddForbidden(params Type[] toAdd)
        {
            foreach (Type t in toAdd)
                Forbidden.Add(t);

            return this;
        }

        /// <summary>
        /// Check if an entity should pass through this filter.
        /// </summary>
        public bool Match(Entity entity)
        {
            // don't match if any necessary components are missing
            foreach(Type t in Necessary)
            {
                if (!entity.HasComponent(t))
                    return false;
            }
            // don't match if any forbidden components are present
            foreach(Type t in Forbidden)
            {
                if (entity.HasComponent(t))
                    return false;
            }

            return true;
        }
    }
}
