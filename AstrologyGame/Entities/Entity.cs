using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using System.Xml;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using AstrologyGame.MapData;


namespace AstrologyGame.Entities
{
    public class Entity
    {
        private List<EntityComponent> components = new List<EntityComponent>();

        public void AddComponent(EntityComponent componentToAdd)
        {
            componentToAdd.ParentEntity = this;
            components.Add(componentToAdd);
        }

        public bool RemoveComponent(EntityComponent componentToRemove)
        {
            return components.Remove(componentToRemove);
        }

        public bool RemoveComponentsOfType<T>()
        {
            List<T> componentsOfType = GetComponents<T>();

            // if no components of type, return false
            if (componentsOfType.Count == 0)
                return false;

            // otherwise, remove them
            foreach(T component in GetComponents<T>())
            {
                RemoveComponent(component as EntityComponent);
            }

            return true;
        }

        public bool HasComponent<T>()
        {
            return components.OfType<T>().Count() > 0;
        }

        public T GetComponent<T>()
        {
            T component = GetComponents<T>().FirstOrDefault();

            if(component == null)
            {
                string message = "This entity does not have a component of type " + typeof(T).Name;
                throw (new Exception(message));
            }
            return component;
        }

        public List<T> GetComponents<T>()
        {
            return components.OfType<T>().ToList();
        }


        // send the event to all components
        public bool FireEvent(ComponentEvent e)
        {
            bool success = false;

            foreach(EntityComponent comp in components)
            {
                if (comp.FireEvent(e))
                    success = true;
            }

            return success;
        }

        // overload to fire an event with just an id
        public bool FireEvent(EventId eventId)
        {
            ComponentEvent e = new ComponentEvent(eventId);
            return FireEvent(e);
        }

        // overload to fire an event with an id and an interactor
        public bool FireEvent(EventId eventId, Entity interactor)
        {
            ComponentEvent e = new ComponentEvent(eventId);
            e[ParameterId.Interactor] = interactor;
            return FireEvent(e);
        }
    }
}