using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using System.Reflection;
using System.Xml;

namespace AstrologyGame.Entities
{
    public static class ComponentFactory
    {
        public static EntityComponent ComponentFromXmlNode(XmlNode node)
        {
            // make a component of the right type, and throw an error if no class of that name exists
            Type componentType = Type.GetType("AstrologyGame.Entities." + node.Name, true);
            EntityComponent component = Activator.CreateInstance(componentType) as EntityComponent;

            // set the properties of the component
            foreach(XmlAttribute attribute in node.Attributes)
            {
                string propertyName = attribute.Name;
                string propertyValueAsString = attribute.Value;

                PropertyInfo propertyInfo = componentType.GetProperty(propertyName);
                Type propertyType = propertyInfo.PropertyType;

                object propertyValue = null;

                if(propertyType != typeof(string)) // if it shouldn't be a string
                {
                    try // first, try converting the string to a base data type if possible
                    {
                        propertyValue = Convert.ChangeType(propertyValueAsString, propertyType);
                    }
                    catch // and if that doesn't work
                    {
                        // try converting to a color
                        if (propertyType == typeof(Color))
                        {
                            propertyValue = Utility.ColorFromString(propertyValueAsString);
                        }
                    }
                }

                propertyInfo.SetValue(component, propertyValue ?? propertyValueAsString);
            }

            return component;
        }

        private static Inventory InventoryFromXmlNode(XmlNode node)
        {
            Inventory inventoryComponent = new Inventory();

            foreach (XmlNode child in node.ChildNodes)
            {
                switch (child.Name)
                {
                    case "entity": // this is an entity inside the chest
                        Entity entity = EntityFactory.EntityFromNode(child);

                        // add the entity to the inventory
                        inventoryComponent.AddEntity(entity);

                        break;
                }
            }

            return inventoryComponent;
        }
    }
}
