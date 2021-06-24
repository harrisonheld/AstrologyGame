using System.Collections.Generic;

using AstrologyGame.Entities.ComponentInteractions;

namespace AstrologyGame.Entities
{
    public class Equippable : EntityComponent
    {
        public Slot Slot { get; set; }
        public Entity Wearer { get; set; } = null;

        public Equippable()
        {
            interactions = new List<Interaction>()
            {
                new Interaction()
                {
                    Name = "Equip",
                    Perform = (Entity e) => EquipToEntity(e),
                    Condition = (Entity e) => EquipToEntityPredicate(e)
                },
                new Interaction()
                {
                    Name = "Unequip",
                    Perform = (Entity e) => UnEquip(),
                    Condition = (Entity e) => UnEquipPredicate()
                }
            };
        }

        public bool IsEquipped()
        {
            return Wearer != null;
        }

        private void EquipToEntity(Entity equipper)
        {
            BodyPlan bodyPlan = equipper.GetComponent<BodyPlan>();
            bodyPlan.Equip(ParentEntity);
            Wearer = equipper;
        }
        private bool EquipToEntityPredicate(Entity equipper)
        {
            BodyPlan equipperBodyPlan = equipper.GetComponent<BodyPlan>();
            Item itemComp = ParentEntity.GetComponent<Item>();

            bool eqipperHasSlot = equipperBodyPlan.HasSlot(this.Slot);
            bool onGround = itemComp.OnGround;
            bool alreadyEquipped = IsEquipped();

            return eqipperHasSlot && !onGround && !alreadyEquipped;
        }

        private void UnEquip()
        {
            BodyPlan wearerBodyPlan = Wearer.GetComponent<BodyPlan>();
            wearerBodyPlan.Unequip(ParentEntity);
            Wearer = null;
        }
        private bool UnEquipPredicate()
        {
            return IsEquipped();
        }
    }
}
