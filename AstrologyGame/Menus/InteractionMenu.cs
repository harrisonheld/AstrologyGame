using System.Collections.Generic;

using Microsoft.Xna.Framework;

using AstrologyGame.Entities;
using AstrologyGame.Components;

namespace AstrologyGame.Menus
{
    public class InteractionMenu : SelectMenu
    {
        Entity interactor;

        public InteractionMenu(Entity interactor, Entity objectToInteractWith)
        {
            this.interactor = interactor;

            items.AddRange(objectToInteractWith.GetInteractions());

            int idx = 0;
            while(idx < items.Count)
            {
                Interaction interaction = items[idx] as Interaction;

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
            Interaction interaction = items[selectedIndex] as Interaction;
            interaction.Perform(interactor);

            // close this menu when a selection is made
            Game1.RemoveMenu(this);
        }
    }
}
