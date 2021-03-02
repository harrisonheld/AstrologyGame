using System;
using System.Collections.Generic;
using System.Text;

using System.Xml;

namespace AstrologyGame.Entities
{
    public static class ComponentFactory
    {
        public static EntityComponent ComponentFromXmlNode(XmlNode node)
        {
            EntityComponent component = null;

            switch(node.Name)
            {
                case "Display":
                    component = DisplayFromXmlNode(node);
                    break;
                case "Inventory":
                    component = InventoryFromXmlNode(node);
                    break;
                case "Item":
                    component = ItemFromXmlNode(node);
                    break;
                case "Creature": // this is temporary
                    component = new Creature();
                    break;
            }

            return component;
        }

        private static Display DisplayFromXmlNode(XmlNode node)
        {
            Display displayComponent = new Display();

            foreach (XmlNode child in node.ChildNodes)
            {
                string innerText = child.InnerText;
                switch (child.Name)
                {
                    case "shouldRender":
                        displayComponent.shouldRender = bool.Parse(innerText);
                        break;
                    case "name":
                        displayComponent.name = innerText;
                        break;
                    case "lore":
                        displayComponent.lore = innerText;
                        break;
                    case "textureName":
                        displayComponent.textureName = innerText;
                        break;
                    case "color":
                        displayComponent.color = Utility.ColorFromString(innerText);
                        break;
                }
            }

            return displayComponent;
        }
        private static Inventory InventoryFromXmlNode(XmlNode node)
        {
            Inventory inventoryComponent = new Inventory();

            foreach (XmlNode child in node.ChildNodes)
            {
                string innerText = child.InnerText;
                switch (child.Name)
                {
                    case "otherEntitiesCanOpen":
                        inventoryComponent.OtherEntitiesCanOpen = bool.Parse(innerText);
                        break;
                    case "entity": // this is an entity inside the chest
                        Entity entity = EntityFactory.EntityFromNode(child);

                        // add the entity to the inventory
                        ComponentEvent addItemEvent = new ComponentEvent(EventId.AddItem);
                        addItemEvent[ParameterId.Target] = entity;
                        inventoryComponent.FireEvent(addItemEvent);

                        break;
                }
            }

            return inventoryComponent;
        }
        private static Item ItemFromXmlNode(XmlNode node)
        {
            Item itemComponent = new Item();

            foreach (XmlNode child in node.ChildNodes)
            {
                string innerText = child.InnerText;

                switch (child.Name)
                {
                    case "count":
                        itemComponent.count = int.Parse(innerText);
                        break;
                }
            }

            return itemComponent;
        }
    }
}
