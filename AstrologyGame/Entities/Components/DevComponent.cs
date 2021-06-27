using System;
using System.Collections.Generic;
using System.Text;

using AstrologyGame.MapData;

namespace AstrologyGame.Entities.Components
{
    public class DevComponent : Component
    {
        public DevComponent()
        {
            interactions = new List<Interaction>()
            {
                new Interaction()
                {
                    Name="Save World Test",
                    Perform = (Entity e) => World.Save(@"C:\Users\Held\Desktop\world.xml")
                },
                new Interaction()
                {
                    Name="Dev only secret",
                    Perform = (Entity e) => Game1.OpenMenu(new Menus.Menu() 
                        {Text = "you found the dev secret! FUCK YOU", BackgroundColor=Microsoft.Xna.Framework.Color.Red})
                }
            };
        }
    }
}
