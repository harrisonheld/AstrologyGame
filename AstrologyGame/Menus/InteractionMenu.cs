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

            foreach (Interaction interaction in objectToInteractWith.GetInteractions())
            {
                // if the interactor meets the condition to perform the interaction
                if (interaction.Condition(interactor))
                {
                    // make a new menu item
                    MenuItem menuItem = new MenuItem();
                    menuItem.Item = interaction;
                    menuItem.Text = interaction.Name;
                    items.Add(menuItem);

                    // add text to the menu
                    Text += interaction.Name + "\n";
                }
            }
        }

        public override void SelectionMade()
        {
            // do the interaction
            Interaction interaction = items[selectedIndex].Item as Interaction;
            interaction.Perform(interactor);

            // close this menu when a selection is made
            Game1.RemoveMenu(this);
        }
    }
}
