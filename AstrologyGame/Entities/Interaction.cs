using System;
using System.Collections.Generic;
using System.Text;

namespace AstrologyGame.Entities
{
    public class Interaction : Menus.IMenuItem
    {
        public string Name; // what will be displayed to the user
        public Action<Entity> Perform; // what this interaction does when performed
        public Predicate<Entity> Condition; // what needs to be true about an interacting entity for this action to be avialable to it

        public Interaction()
        {
            // by default, Perform will just throw an exception. You should an Action to it.
            Perform = (Entity e) => { throw new Exception("This Interaction has no associated Action to perform."); };
            // by default, the condition will always be met
            Condition = (Entity e) => { return true; };
        }

        string Menus.IMenuItem.GetText() => Name;
    }
}