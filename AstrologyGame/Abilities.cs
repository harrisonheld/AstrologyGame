/*using System;
using System.Collections.Generic;
using System.Text;

using AstrologyGame.Entities;
using AstrologyGame.MapData;

using System.Xml;

namespace AstrologyGame
{
    public abstract class Ability
    {
        private string abilityId;

        protected string abilityName = "";
        protected string abilityLore = "";
        protected int baseCooldown; // cooldown in turns
        protected TargetType targetType;

        public int BaseCooldown { get { return baseCooldown;  } }

        public virtual void Activate(Entity caster, OrderedPair target) { }
        public void Activate(Entity caster) // activate with no target
        {
            Activate(caster, null);
        }

        protected void LoadFromId(string id)
        {
            abilityId = id;

            // load ability details from xml
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Utility.ABILITIES_PATH);

            XmlNode abilityNode = xmlDoc.GetElementById(id);
            abilityName = abilityNode.Attributes.GetNamedItem("name").Value;
            abilityLore = abilityNode.Attributes.GetNamedItem("lore").Value;
            baseCooldown = int.Parse(abilityNode.Attributes.GetNamedItem("baseCooldown").Value);
            string target = abilityNode.Attributes.GetNamedItem("target").Value;

            switch(target)
            {
                case "self":
                    targetType = TargetType.Self;
                    break;
                case "position":
                    targetType = TargetType.Position;
                    break;
                default:
                    targetType = TargetType.Position;
                    break;
            }
        }
    }

    public enum TargetType
    { 
        Self,
        Position
    }

    public class Teleport : Ability
    {
        public Teleport()
        {
            LoadFromId("teleport");
        }

        public override void Activate(Entity caster, OrderedPair destination)
        {
            caster.GetComponent<Position>().Pos = destination;
        }
    }

    public class Bind : Ability
    { 
        public Bind()
        {
            LoadFromId("bind");
        }

        public override void Activate(Entity caster, OrderedPair target)
        {
            Entity targetEntity = Zone.GetEntitiesAtPosition(target)[0];
            targetEntity.GetComponent<Creature>().quickness = 0;
        }
    }
}
*/