using System;
using System.Collections.Generic;
using System.Text;

using System.Xml;

using AstrologyGame.Components;

namespace AstrologyGame.Entities.Factories
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
            return EntityFromEntityNode(entityNode);
        }
        // create a new entity, and add a Position component with the specified coords
        public static Entity EntityFromId(string entityId, int xPos, int yPos)
        {
            Entity entity = EntityFromId(entityId);
            entity.AddComponent(new Position { X = xPos, Y = yPos });
            return entity;
        }

        public static Entity EntityFromNode(XmlNode node)
        { 
            string idRef = node.Attributes.GetNamedItem("ref").Value;
            if (idRef != null)
                return (EntityFromId(idRef));

            return EntityFromEntityNode(node);
        }

        private static Entity EntityFromEntityNode(XmlNode node)
        {
            Entity entity = new Entity();

            // each child node should be a component
            foreach(XmlNode child in node.ChildNodes)
            {
                Component newComponent = ComponentFactory.ComponentFromXmlNode(child);
                entity.AddComponent(newComponent);
            }

            return entity;
        }


        /// <summary>
        /// Returns a list of all the ids in entities.xml
        /// </summary>
        /// <returns>A list of ids</returns>
        public static List<string> GetIdsInXML()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Utility.ENTITIES_PATH);

            List<string> ids = new List<string>();

            foreach(XmlNode node in xmlDoc.ChildNodes[1])
            {
                string id = node.Attributes.GetNamedItem("id").Value;
                ids.Add(id);
            }

            return ids;
        }
    }
}
