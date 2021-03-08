using System;
using System.Collections.Generic;
using System.Text;

namespace AstrologyGame.Entities
{
    public class InInventory : EntityComponent
    {
        public Inventory ContainingInventory { get; set; }
    }
}
