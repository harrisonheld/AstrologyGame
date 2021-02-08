using System;
using System.Collections.Generic;
using System.Text;

namespace AstrologyGame.Entities
{
    public interface IInteractable
    {
        List<Interaction> Interactions { get; }
    }

    public interface IAttackable : IInteractable
    {
        void BeAttacked(Entity attacker);
    }
    public interface IGettable : IInteractable
    {
        void BeGot(Entity getter);
    }
    public interface IDroppable : IInteractable
    {
        void BeDropped(Entity dropper);
    }
    public interface IReadable : IInteractable
    {
        void BeRead(Entity reader);
    }
    public interface IOpenable : IInteractable
    {
        void BeOpened(Entity opener);
    }
    public interface IEquipment : IInteractable
    {
        Slot EquipSlot { get; }
        void BeEquipped(Entity equipper);
        void BeDeEquipped(Entity deequipper);
    }
}
