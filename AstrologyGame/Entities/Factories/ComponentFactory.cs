using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using System.Reflection;
using System.Xml;

using AstrologyGame.Components;

namespace AstrologyGame.Entities.Factories
{
    public static class ComponentFactory
    {
        public static Component ComponentFromXmlNode(XmlNode node)
        {
            // make a component of the right type, and throw an error if no class of that name exists
            Type componentType = Type.GetType("AstrologyGame.Components." + node.Name, true);
            Component component = Activator.CreateInstance(componentType) as Component;

            // set the properties of the component
            foreach(XmlAttribute attribute in node.Attributes)
            {
                string propertyName = attribute.Name;
                string propertyValueAsString = attribute.Value;

                PropertyInfo propertyInfo = componentType.GetProperty(propertyName);
                Type propertyType = propertyInfo.PropertyType;

                object propertyValue;

                if (propertyType == typeof(Color))
                {
                    propertyValue = Utility.ColorFromString(propertyValueAsString);
                }
                else
                {
                    System.ComponentModel.TypeConverter typeConverter = System.ComponentModel.TypeDescriptor.GetConverter(propertyType);
                    propertyValue = typeConverter.ConvertFromString(propertyValueAsString);
                }


                propertyInfo.SetValue(component, propertyValue);
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
                        inventoryComponent.Contents.Add(entity);

                        break;
                }
            }

            return inventoryComponent;
        }
    }
}
