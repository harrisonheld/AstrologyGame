using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using AstrologyGame.MapData;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AstrologyGame.DynamicObjects
{
    public abstract class Creature : DynamicObject
    {
        // the cost for various actions
        public const int COST_MOVE = 10;
        public const int COST_ATTACK = 10;
        public int Quickness { get; set; } = 10; // how much AP does this creature get per turn
        public int ActionPoints { get; set; } = 0; // a.k.a. AP
        public Creature()
        {
            Solid = true;
        }
        public void GetAttacked(DynamicObject attacker)
        {

        }
        public virtual void AiTurn()
        {
            RechargeAP();
        }
        public void RechargeAP()
        {
            // give the creature its AP for this turn
            ActionPoints += Quickness;

            // cap the action points so the creature can't wait around and accumulate a massive amount of AP
            if (ActionPoints > Quickness * 2)
                ActionPoints = Quickness * 2;
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
        public bool AttemptMove(int targetX, int targetY)
        {
            // if there is a solid object in the way, return false
            foreach (DynamicObject o in Zone.objects)
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

        public bool MoveTowards(DynamicObject target)
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

            return AttemptMove(X + relMoveX, Y + relMoveY);
        }

        /// <summary>
        /// Spend all this creature's action points seeking a target.
        /// </summary>
        /// <param name="target"></param>
        public void Seek(DynamicObject target)
        {
            while (ActionPoints >= Creature.COST_MOVE)
            {
                bool successfulMove = MoveTowards(target);

                if (!successfulMove)
                    break;
            }
        }
    }

    public class Humanoid : Creature
    {
        public Humanoid()
        {
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
            Lore = "This wretched anatomy was spawned by Abhoth. Extremeties jut out from the central mass " +
                "in no particular order. A limb here, a lobe there. It makes no difference." +
                "\n\nIt meanders mindlessly.";
            Color = Color.IndianRed;
        }

        public override void AiTurn()
        {
            int newY = (Y + 1) % Zone.HEIGHT;

            // check if desired position has a solid object
            foreach(DynamicObject o in Zone.objects)
            {
                if (o.X == this.X && o.Y == newY && o.Solid)
                    return;
            }

            Y = newY;
        }
    }

    public class Catboy : Humanoid
    {
        public Catboy() 
        {
            TextureName = "ears";
            Name = "catboy";
            Lore = "Precious black marbles oggle you lovingly. Meow, meow.";

            Color = Color.Pink;
        }
    }
}
