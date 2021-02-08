using System;
using System.Collections.Generic;
using System.Text;

namespace AstrologyGame.Entities
{
    public enum Interaction
    {
        Read, // read a book/paper
        Open, // open a container
        Attack, // attack this object
        Get, // another object picks up this object
        Drop, // drop the object on the ground
        Equip, // Equip this object
        DeEquip, // De-equip this object
    }
}
