using System;
using System.Collections.Generic;
using System.Linq;

using AstrologyGame.Components;

namespace AstrologyGame.Entities
{
    public class Entity : Menus.IMenuItem
    {
        public List<Component> components = new List<Component>();

        ~Entity()
        {
            // is a finalizer even necessary for this sort of thing
            // fuck if i know
            components.Clear();
        }

        public void AddComponent(Component componentToAdd)
        {
            componentToAdd.Owner = this;
            components.Add(componentToAdd);
        }

        public bool RemoveComponent(Component componentToRemove)
        {
            return components.Remove(componentToRemove);
        }
        public bool RemoveComponentsOfType<T>()
            where T : Component
        {
            List<T> componentsOfType = GetComponents<T>();

            // if no components of type, return false
            if (componentsOfType.Count == 0)
                return false;

            // otherwise, remove them and return true
            foreach(T component in GetComponents<T>())
            {
                RemoveComponent(component as Component);
            }

            return true;
        }

        public bool HasComponent<T>()
            where T : Component
        {
            return components.OfType<T>().Count() > 0;
        }
        public bool HasComponent(Type type)
        {
            return components.Exists((Component comp) => (comp.GetType() == type));
        }

        public T GetComponent<T>()
            where T : Component
        {
            T component = GetComponents<T>().FirstOrDefault();

            if(component == null)
            {
                string exceptionMessage = $"This entity does not have a component of type {typeof(T).Name}.";
                throw (new Exception(exceptionMessage));
            }

            return component;
        }
        public List<T> GetComponents<T>()
            where T : Component
        {
            return components.OfType<T>().ToList();
        }

        public List<Interaction> GetInteractions()
        {
            List<Interaction> interactions = new List<Interaction>();

            foreach(Component component in components)
            {
                interactions.AddRange(component.GetInteractions());
            }

            return interactions;
        }

        // IMenuItem implementation
        string Menus.IMenuItem.GetText()
        {
            if (HasComponent<Display>())
                return GetComponent<Display>().Name;

            return "THIS ENTITY HAS NO DISPLAY COMPONENT";
        }
    }
}