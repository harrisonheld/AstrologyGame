using System;
using System.Text;
using System.Collections.Generic;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

using AstrologyGame.Entities;

namespace AstrologyGame.Components
{
    [Serializable]
    public abstract class Component
    {
        public Entity Owner { get; set; }

        protected List<Interaction> interactions = new List<Interaction>();

        public Component() { }

        public List<Interaction> GetInteractions()
        {
            return interactions;
        }
    }
}
