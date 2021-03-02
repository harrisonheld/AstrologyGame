using System.Text;
using System.Collections.Generic;

namespace AstrologyGame.Entities
{
    public abstract class EntityComponent
    {
        public Entity ParentEntity { get; set; }

        public virtual bool FireEvent(ComponentEvent cEvent)
        {
            return false;
        }
    }
}
