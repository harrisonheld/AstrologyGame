using System;
using System.Collections.Generic;
using System.Text;

namespace AstrologyGame.Components
{
    class FactionInfo : Component
    {
        /// <summary> What factions is this entity in. </summary>
        public Faction Faction { get; set; }
        /// <summary> Integer values of this entity's feelings on other factions. </summary>
        private Dictionary<Faction, int> repDict { get; set; }

        public void SetReputation(Faction faction, int rep)
        {
            if (repDict.ContainsKey(faction))
                repDict[faction] = rep;
            else
                repDict.Add(faction, rep);
        }
        public int GetReputation(Faction faction)
        {
            // return value in dict, if its not in the dict return 0

            if (repDict.ContainsKey(faction))
                return repDict[faction];

            return 0;
        }
    }

    public enum Faction
    {
        None,
        Human,
        Horror, // cosmic horrors that fit into no specific group
    }
}
