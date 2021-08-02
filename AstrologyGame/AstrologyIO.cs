using System;
using System.Collections.Generic;
using System.Text;

using AstrologyGame.Components;
using AstrologyGame.Entities;
using AstrologyGame.MapData;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace AstrologyGame
{
    public static class AstrologyIO
    {
        public static void SerializeEntity(Entity entity)
        {

        }

        public static void SerializeComponent(Component component)
        {
            XmlSerializer serializer = new XmlSerializer(component.GetType());

            using (FileStream fs = new FileStream(@"C:\Users\Held\Desktop\shit.xml", FileMode.Create))
            {
                serializer.Serialize(fs, component);
                fs.Flush();
            }
        }
    }
}
