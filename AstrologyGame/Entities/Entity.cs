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

        public bool HasComponent<T>()
        {
            Type type = typeof(T);
            return (components.Where(x => x.GetType() == type).Count() != 0);
        }

        public T GetComponent<T>()
        {
            Type type = typeof(T);
            EntityComponent component = components.Where(x => x.GetType() == type ).FirstOrDefault();
            return (T)(object)component;
        }


        // send the event to all components
        public void FireEvent(ComponentEvent e)
        {
            foreach(EntityComponent comp in components)
            {
                comp.FireEvent(e);
            }
        }

        // overload to fire an event with just an id
        public void FireEvent(EventId eventId)
        {
            ComponentEvent e = new ComponentEvent(eventId);
            FireEvent(e);
        }

        // overload to fire an event with an id and an interactor
        public void FireEvent(EventId eventId, Entity interactor)
        {
            ComponentEvent e = new ComponentEvent(eventId);
            e[ParameterId.Interactor] = interactor;
            FireEvent(e);
        }
    }
}