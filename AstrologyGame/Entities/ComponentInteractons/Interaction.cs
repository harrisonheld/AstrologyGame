using System;
using System.Collections.Generic;
using System.Text;

namespace AstrologyGame.Entities.ComponentInteractions
{
    public class Interaction
    {
        public string Name; // what will be displayed to the user
        public Action Perform; // what this interaction does when performed
        public Action<Entity> Condition; // what needs to be true about the entity for this action to be avialable to it

        public Interaction()
        {

        }
    }
}
