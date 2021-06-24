using System.Collections.Generic;

using Microsoft.Xna.Framework;

using AstrologyGame.Entities;
using AstrologyGame.Entities.ComponentInteractions;

namespace AstrologyGame
{
    class InteractionMenu : SelectMenu
    {
        List<Interaction> interactions;
        Entity interactor;

        public InteractionMenu(Entity interactor, Entity objectToInteractWith)
        {
            this.interactor = interactor;

            BackgroundColor = Color.Black;

            interactions = objectToInteractWith.GetInteractions();
            selectionCount = interactions.Count;

            int idx = 0;
            while(idx < interactions.Count)
            {
                Interaction interaction = interactions[idx];

                // if the interactor does not meet the condition to perform this interaction
                if (!interaction.Condition(interactor))
                {
                    // remove it from the list
                    interactions.RemoveAt(idx);
                    // reduce the amount of selections in the menu
                    selectionCount--;
                    continue;
                }


                Text += interaction.Name + "\n";

                idx++;
            }
        }

        public override void SelectionMade()
        {
            // close this menu when a selection is made
            Game1.CloseMenu(this);

            // do the interaction
            interactions[selectedIndex].Perform(interactor);

            // refresh the menus incase this interaction changed them
            Game1.QueueRefreshAllMenus();
        }
    }
}
