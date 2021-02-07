using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using AstrologyGame.MapData;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AstrologyGame.DynamicObjects
{
    public abstract class Creature : DynamicObject, IAttackable
    {
        List<Interaction> IInteractable.Interactions { get { return new List<Interaction>() { Interaction.Attack }; } }

        // the cost for various actions
        public const int COST_MOVE = 100;
        public const int COST_ATTACK = 100;
        public const int AP_CAP = 200;

        public int Quickness { get; set; } = 0; // how much AP does this object get per turn.
        public int ActionPoints { get; set; } = 0; // a.k.a. AP

        public Creature()
        {
            MaxHealth = 10;
            Health = MaxHealth;
            Quickness = 100;
            Solid = true;
        }
        public void BeAttacked(DynamicObject attacker)
        {
            Health -= attacker.Stats.Prowess;

            if (Health <= 0)
                Die();
        }
        private void Die()
        {
            DropAll();
            Zone.RemoveObject(this);
        }
        private void DropAll()
        {
            while (Children.Count > 0)
                Children[0].Interact(Interaction.Drop, this);
        }

        public virtual void AiTurn()
        {
            RechargeAP();
        }
        public void RechargeAP()
        {
            // give the object its AP for this turn
            ActionPoints += Quickness;

            // cap the action points so the object can't wait around and accumulate a massive amount of AP
            ActionPoints = Math.Clamp(ActionPoints, 0, AP_CAP);
        }

        /// <summary>
        /// Attempts to remove the specified number of Action Points.
        /// Returns true if successful, returns false if there aren't enough points to remove the specified amount.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool DeductActionPoints(int amount)
        {
            int newActionPointCount = ActionPoints - amount;
            if(newActionPointCount < 0) // not enough AP to take this amount away
                return false;

            ActionPoints = newActionPointCount;
            return true;
        }
        public bool TryMove(int targetX, int targetY)
        {
            // if there is a solid object in the way, return false
            foreach (DynamicObject o in Zone.Objects)
            {
                if (o.X == targetX && o.Y == targetY && o.Solid)
                    return false;
            }

            // otherwise, do the move and return true
            X = targetX;
            Y = targetY;

            DeductActionPoints(COST_MOVE);

            return true;
        }
        public bool TryMoveTowards(DynamicObject target)
        {
            int targetX = target.X;
            int targetY = target.Y;

            int relMoveX = 0;
            int relMoveY = 0;

            if (X > targetX)
                relMoveX = -1;
            else if (X < targetX)
                relMoveX = 1;
            if (Y > targetY)
                relMoveY = -1;
            else if (Y < targetY)
                relMoveY = 1;

            return TryMove(X + relMoveX, Y + relMoveY);
        }
        public bool TryMoveAway(DynamicObject target)
        {
            int targetX = target.X;
            int targetY = target.Y;

            int relMoveX = 0;
            int relMoveY = 0;

            if (X > targetX)
                relMoveX = -1;
            else if (X < targetX)
                relMoveX = 1;
            if (Y > targetY)
                relMoveY = -1;
            else if (Y < targetY)
                relMoveY = 1;

            return TryMove(X - relMoveX, Y - relMoveY);
        }

        // learn how to use delegates and use them here! these functions are so similar its redunant
        /// <summary>
        /// Spend all this creature's action points seeking a target.
        /// </summary>
        /// <param name="target"></param>
        public void Seek(DynamicObject target)
        {
            while (ActionPoints >= Creature.COST_MOVE)
            {
                bool successfulMove = TryMoveTowards(target);

                // if the move fails, most likely because there is a wall in the way, just quit trying.
                if (!successfulMove)
                    break;
            }
        }
        public void Flee(DynamicObject target)
        {
            while (ActionPoints >= Creature.COST_MOVE)
            {
                bool successfulMove = TryMoveAway(target);

                // if the move fails, most likely because there is a wall in the way, just quit trying.
                if (!successfulMove)
                    break;
            }
        }
    }

    public class Humanoid : Creature
    {
        public Humanoid()
        {
            // add the slots
            SlotDict.Add(Slot.Head, null);
            SlotDict.Add(Slot.Body, null);
            SlotDict.Add(Slot.Legs, null);

            TextureName = "human";
        }
    }

    public class ZodiacFrog : Creature
    {
        public ZodiacFrog()
        {
            TextureName = "frog";
            Name = "zodiac frog";
            Lore = "White spots adorn its black skin. " +
                "The asterism is distorted as the creature ungulates.";
            Color = Color.DarkGray;
        }
    }

    public class ChildOfAbhoth : Creature
    { 
        public ChildOfAbhoth()
        {
            TextureName = "frog";
            Name = "child of Abhoth";
            Lore = "This wretched thing is \"alive\" only in the most abstract meaning of the word. " +
                "Extremeties jut out from the central mass in no particular order. " +
                "A limb here, or a lobe there - it makes no difference. Lacking a face " +
                "with which to divulge the true magnitude of its suffering, it locomotes silently. ";
            Color = Color.IndianRed;
            Quickness = 20;
        }

        public override void AiTurn()
        {
            Flee(Zone.Player);
            base.AiTurn();
        }
    }

    public enum Slot
    {
        Head,
        Body,
        Legs
    }
}
