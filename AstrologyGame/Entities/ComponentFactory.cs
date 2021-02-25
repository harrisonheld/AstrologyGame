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
    }
}
