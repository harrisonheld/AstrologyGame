using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using AstrologyGame.MapData;

namespace AstrologyGame.Entities
{
    public abstract class EntityComponent
    {
        public Entity ParentEntity { get; set; }
        public bool interactable = false; // can other entities interact with this component?
        public static string InteractionName { get; set; } // "read" for book, "attack" for creature, etc

        public EntityComponent()
        {

        }

        public virtual void Update() { } // this is done every game Tick
        public virtual void Interact(Entity interactor) { } // this is done when another entity interacts with this component
    }

    public class Display : EntityComponent
    {
        public bool shouldRender = true;
        public string name = "default";
        public string lore = "default";
        public string textureName = "default";
        public Color color = Color.White;
    }

    public class Position : EntityComponent
    {
        public int x = 0;
        public int y = 0;

        public OrderedPair Pos
        { 
            get
            {
                return new OrderedPair(x, y);
            }
            set
            {
                x = value.X;
                y = value.Y;
            }
        }
    }

    public class Inventory : EntityComponent
    {
        public List<Entity> entites = new List<Entity>();

        // you must set interactable to true to use this
        public override void Interact(Entity interactor)
        {
            if(interactor == Zone.Player)
            {
                InventoryMenu iMenu = new InventoryMenu(ParentEntity);
                Game1.OpenMenu(iMenu);
            }
        }
        /// <summary>
        /// Add an item to this inventory and remove it from where it came from.
        /// </summary>
        public void AddEntity(Entity entityToAdd)
        {
            Zone.RemoveObject(entityToAdd); // remove it from the zone
            entites.Add(entityToAdd); // and put it in this inventory   
        }
    }

    public class Equipment : EntityComponent
    {
        private Dictionary<Slot, Entity> slotDict  = new Dictionary<Slot, Entity>();

        public void Equip(Entity toEquip)
        {
            Slot slot = toEquip.GetComponent<Equippable>().slot;
            // if we don't have the appropriate equip slot
            if (!slotDict.ContainsKey(slot))
            {
                Utility.Log("You can't equip this item.");
                return;
            }
            // if it's not in our inventory, add it
            Inventory inv = ParentEntity.GetComponent<Inventory>();
            if (!inv.entites.Contains(toEquip))
            {
                inv.entites.Add(toEquip);
            }

            slotDict[slot] = toEquip;
        }

        public void TryDeEquip(Entity toDeEquip)
        {
            Slot slot = toDeEquip.GetComponent<Equippable>().slot;

            // if we have it equipped
            if (slotDict[slot] == toDeEquip)
            {
                // remove it from the SlotDict
                slotDict[slot] = null;
            }
        }

        public bool HasEquipped(Entity equippable)
        {
            return slotDict.ContainsValue(equippable);
        }

        public void AddSlot(Slot slotToAdd)
        {
            slotDict.Add(slotToAdd, null);
        }
    }

    public class Equippable : EntityComponent
    {
        public Slot slot;
    }

    public class Abilities : EntityComponent
    {
        public List<Ability> abilities = new List<Ability>();

        public void AddAbility(Ability abilityToAdd)
        {
            abilities.Add(abilityToAdd);
        }
        public bool RemoveAbility(Ability abilityToRemove)
        {
            return abilities.Remove(abilityToRemove);
        }
    }

    public class Creature : EntityComponent
    {
        public int health;
        public int maxHealth;
        public int quickness;
        public int actionPoints;

        public override void Update()
        {
            AiTurn();
        }
        public void AiTurn()
        {
            RechargeAP();
        }
        public void RechargeAP()
        {
            // give the object its AP for this turn
            actionPoints += quickness;

            // cap the action points so the object can't wait around and accumulate a massive amount of AP
            actionPoints = Math.Clamp(actionPoints, 0, Utility.AP_CAP);
        }
        public void Die()
        {
            DropAll();
            Zone.RemoveObject(ParentEntity);
        }
        private void DropAll()
        {
            Inventory inv = ParentEntity.GetComponent<Inventory>();
            foreach(Entity item in inv.entites)
            {
                // TODO: drop all items
            }
        }
        public bool DeductActionPoints(int amountToDeduct)
        {
            int newActionPointCount = actionPoints - amountToDeduct;
            if (newActionPointCount < 0) // not enough AP to take this amount away
                return false;

            actionPoints = newActionPointCount;
            return true;
        }
        public bool TryMove(int targetX, int targetY)
        {
            // return false if not enough AP
            // if (!DeductActionPoints(Utility.COST_MOVE))
            //    return false;

            // TODO: check if solid object is in the way

            // otherwise, do the move and return true
            ParentEntity.GetComponent<Position>().Pos = new OrderedPair(targetX, targetY);

            return true;
        }
        public bool TryMoveTowards(Entity target)
        {
            OrderedPair myPos = ParentEntity.GetComponent<Position>().Pos;
            OrderedPair targetPos = target.GetComponent<Position>().Pos;

            int targetX = targetPos.X;
            int targetY = targetPos.Y;

            int relMoveX = 0;
            int relMoveY = 0;

            if (myPos.Y > targetX)
                relMoveX = -1;
            else if (myPos.X < targetX)
                relMoveX = 1;
            if (myPos.Y > targetY)
                relMoveY = -1;
            else if (myPos.Y < targetY)
                relMoveY = 1;

            return TryMove(myPos.X + relMoveX, myPos.Y + relMoveY);
        }

        /// <summary>
        /// Spend all this creature's action points seeking a target.
        /// </summary>
        /// <param name="target"></param>
        public void Seek(Entity target)
        {
            while (actionPoints >= Utility.COST_MOVE)
            {
                bool successfulMove = TryMoveTowards(target);

                // if the move fails, most likely because there is a wall in the way, just quit trying.
                if (!successfulMove)
                    break;
            }
        }
    }

    public class Item : EntityComponent
    {
        public int count;
    }

    public class Book : EntityComponent
    {
        public string bookId;

        public Book()
        {
            interactable = true;
        }

        public override void Interact(Entity interactor)
        {
            BookMenu menu = new BookMenu(bookId);
            Game1.OpenMenu(menu);
        }
    }
}
