using System;
using System.Collections.Generic;
using System.Text;

namespace AstrologyGame.DynamicObjects
{
    public interface IInteractable
    {
        List<Interaction> Interactions { get; }
    }

    public interface IAttackable : IInteractable
    {
        void BeAttacked(DynamicObject attacker);
    }
    public interface IGettable : IInteractable
    {
        void BeGot(DynamicObject getter);
    }
    public interface IDroppable : IInteractable
    {
        void BeDropped(DynamicObject dropper);
    }
    public interface IReadable : IInteractable
    {
        void BeRead(DynamicObject reader);
    }
    public interface IOpenable : IInteractable
    {
        void BeOpened(DynamicObject opener);
    }
    public interface IEquipment : IInteractable
    {
        Slot EquipSlot { get; }
        void BeEquipped(DynamicObject equipper);
        void BeDeEquipped(DynamicObject deequipper);
    }
}
