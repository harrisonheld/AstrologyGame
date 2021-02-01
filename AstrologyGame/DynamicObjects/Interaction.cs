using System;
using System.Collections.Generic;
using System.Text;

namespace AstrologyGame.DynamicObjects
{
    public enum Interaction
    {
        Read, // read a book/paper
        Open, // open a container
        Attack, // attack this object
        Get, // another object picks up this object
    }
}
