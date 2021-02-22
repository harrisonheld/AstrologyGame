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

        public Entity()
        {
            AddComponent(new Position());
            AddComponent(new Display());
        }

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
        public void UpdateAllComponents()
        {
            foreach(EntityComponent comp in components)
            {
                comp.Update();
            }
        }

        /// <summary>
        /// Using this Entity as a root node, remove the given Entity from any of it's descendants.
        /// </summary>
        /// <param name="toRemove">The object to remove from the tree.</param>
        /// <returns>True if the removal was successful, False if the given object wasn't in the tree to begin with.</returns>
        public bool RemoveFromDescendants(Entity toRemove)
        {
            Stack<Entity> stack = new Stack<Entity>();
            stack.Push(this);

            while(stack.Count > 0)
            {
                Entity node = stack.Pop();
                Inventory inv = node.GetComponent<Inventory>();
                if (inv.entites.Remove(toRemove))
                    return true;

                foreach(Entity child in inv.entites)
                {
                    Inventory childInv = child.GetComponent<Inventory>();
                    if (childInv.entites.Remove(toRemove))
                        return true;
                    else
                        stack.Push(child);
                }
            }

            return false; 
        }

        public virtual void AnimationTurn()
        {
            // things like changing the texture, changing color, etc.
        }
    }
}