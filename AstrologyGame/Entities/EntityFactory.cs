﻿using System;
using System.Collections.Generic;
using System.Text;

using System.Xml;

namespace AstrologyGame.Entities
{
    public static class EntityFactory
    {
        public static Entity EntityFromId(string entityId)
        {
            // get the xml node by id
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Utility.ENTITIES_PATH);
            XmlNode entityNode = xmlDoc.GetElementById(entityId);

            // create an entity with it
            return EntityFromXmlNode(entityNode);
        }

        private static Entity EntityFromXmlNode(XmlNode node)
        {
            Entity entity = new Entity();

            // each child node should be a component
            foreach(XmlNode child in node.ChildNodes)
            {
                EntityComponent newComponent = ComponentFactory.ComponentFromXmlNode(child);
                entity.AddComponent(newComponent);
            }

            return entity;
        }
    }
}
