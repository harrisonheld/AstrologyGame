using System.Collections.Generic;

using Microsoft.Xna.Framework;

using AstrologyGame.Entities;
using AstrologyGame.Entities.ComponentInteractions;

namespace AstrologyGame
{
    class InteractionMenu : SelectMenu
    {
        List<Interaction> interactions;

        public InteractionMenu(Entity interactor, Entity objectToInteractWith)
        {
            BackgroundColor = Color.Black;

            interactions = objectToInteractWith.GetInteractions();
            selectionCount = interactions.Count;

            foreach(Interaction interaction in interactions)
            {
                Text += interaction.Name + "\n";
            }
        }

        public override void SelectionMade()
        {
            // close this menu when a selection is made
            Game1.CloseMenu(this);

            // do the interaction
            interactions[selectedIndex].Perform();

            // refresh the menus incase this interaction changed them
            Game1.QueueRefreshAllMenus();
        }
    }
}
