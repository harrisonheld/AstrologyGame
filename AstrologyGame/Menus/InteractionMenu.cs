using System.Collections.Generic;

using Microsoft.Xna.Framework;

using AstrologyGame.Entities;
using AstrologyGame.Components;

namespace AstrologyGame.Menus
{
    public class InteractionMenu : SelectMenu
    {
        List<Interaction> interactions;
        Entity interactor;

        public InteractionMenu(Entity interactor, Entity objectToInteractWith)
        {
            this.interactor = interactor;

            items.AddRange(objectToInteractWith.GetInteractions());

            int idx = 0;
            while(idx < interactions.Count)
            {
                Interaction interaction = interactions[idx];

                // if the interactor does not meet the condition to perform this interaction
                if (!interaction.Condition(interactor))
                {
                    // remove it from the list
                    items.RemoveAt(idx);
                    // reduce the amount of selections in the menu
                    continue;
                }


                Text += interaction.Name + "\n";

                idx++;
            }
        }

        public override void SelectionMade()
        {
            // do the interaction
            interactions[selectedIndex].Perform(interactor);

            // close this menu when a selection is made
            Game1.RemoveMenu(this);
        }
    }
}
