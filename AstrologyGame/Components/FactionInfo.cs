using System;
using System.Collections.Generic;
using System.Text;

namespace AstrologyGame.Components
{
    public class FactionInfo : Component
    {
        /// <summary> What factions is this entity in. </summary>
        public Faction Faction { get; set; } = Faction.None;
        /// <summary> Integer values of this entity's feelings on other factions. </summary>
        public SerializableDictionary<Faction, int> ReputationDictionary { get; set; } = new SerializableDictionary<Faction, int>();

        public void SetReputation(Faction faction, int rep)
        {
            if (ReputationDictionary.ContainsKey(faction))
                ReputationDictionary[faction] = rep;
            else
                ReputationDictionary.Add(faction, rep);
        }
        public int GetReputation(Faction faction)
        {
            // return value in dict, if its not in the dict return 0

            if (ReputationDictionary.ContainsKey(faction))
                return ReputationDictionary[faction];

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
