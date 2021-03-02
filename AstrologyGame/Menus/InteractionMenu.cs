using Microsoft.Xna.Framework;

using AstrologyGame.Entities;

namespace AstrologyGame
{
    class InteractionMenu : SelectMenu
    {
        Entity objectToInteractWith;

        /// <param name="forbiddenInteraction">A list of interactions that will not be included in this menu, even if the object to interact with has them available.</param>
        public InteractionMenu(Entity _objectToInteractWith)
        {
            BackgroundColor = Color.Black;
            objectToInteractWith = _objectToInteractWith;

            // make a copy of the items interactions so we don't have to worry about changing things in the object itself

        }

        public override void SelectionMade()
        {
            /*
            // close this menu when a selection is made
            Game1.CloseMenu(this);

            // TODO: DO THE INTERACTION

            // refresh the menus incase this interaction changed them
            Game1.QueueRefreshAllMenus();
            */
        }
    }
}
