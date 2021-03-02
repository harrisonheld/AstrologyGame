using System.Collections.Generic;

namespace AstrologyGame.Entities
{
    public class ComponentEvent
    {
        private EventId eventId; // it says what the event wants to do
        private Dictionary<ParameterId, object> parameters = new Dictionary<ParameterId, object>();

        public ComponentEvent(EventId _eventId)
        {
            eventId = _eventId;
        }

        public EventId EventId
        {
            get => eventId;
        }

        // an indexer to easily edit or add parameters
        public object this[ParameterId parameterName]
        {
            get => parameters[parameterName];
            set => parameters.Add(parameterName, value);
        }
    }

    public enum EventId
    {
        Read,
        OpenInventory,
        /// <summary>Remove an item from the Inventory. Generally requires Target parameter, to decide which item will be removed.</summary>
        RemoveItem,
        Get,
        Drop,
    }

    public enum ParameterId
    {
        /// <summary>The entity that caused this event to fire. Data type hould be an Entity. For example, the entity who attacked us.</summary>
        Interactor,
        /// <summary>The entity on which to do this event. Data type should be an Entity. For example, the entity in our inventory we should remove.</summary>
        Target,
        Name,
    }
}
