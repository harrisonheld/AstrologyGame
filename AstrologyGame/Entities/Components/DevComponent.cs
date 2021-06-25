using System;
using System.Collections.Generic;
using System.Text;

using AstrologyGame.MapData;

namespace AstrologyGame.Entities.Components
{
    public class DevComponent : EntityComponent
    {
        public DevComponent()
        {
            interactions = new List<Interaction>()
            {
                new Interaction()
                {
                    Name="SAVE WORLD",
                    Perform = (Entity e) => World.Save(@"C:\Users\Held\Desktop\world.xml")
                }
            };
        }
    }
}
