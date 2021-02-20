using System;
using System.Collections.Generic;
using System.Text;

using AstrologyGame.Entities;

using System.Xml;

namespace AstrologyGame
{
    public abstract class Ability
    {
        private string abilityId;
        // set the ability id and load its details from the xml
        protected void SetAbilityId(string id)
        {
            abilityId = id;
            LoadAbility();
        }

        protected string abilityName = "";
        protected string abilityLore = "";
        public int BaseCooldown { get; set; } // cooldown in turns

        public virtual void Activate(Entity caster, OrderedPair target) { }
        public virtual void Activate(Entity caster, Entity target) { }

        private void LoadAbility()
        {
            // load ability details from xml
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Utility.ABILITIES_PATH);

            XmlNode abilityNode = xmlDoc.GetElementById(abilityId);
            abilityName = abilityNode.Attributes.GetNamedItem("name").Value;
            abilityLore = abilityNode.Attributes.GetNamedItem("lore").Value;
            BaseCooldown = int.Parse(abilityNode.Attributes.GetNamedItem("baseCooldown").Value);
        }
    }

    public class Teleport : Ability
    {
        public Teleport()
        {
            SetAbilityId("teleport");
        }

        public override void Activate(Entity caster, OrderedPair destination)
        {
            caster.Pos = destination;
        }
    }
}
