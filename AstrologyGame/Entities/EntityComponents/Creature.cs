using System;

using AstrologyGame.MapData;

namespace AstrologyGame.Entities
{
    public class Creature : EntityComponent
    {
        // TODO: remove this component

        public int health;
        public int maxHealth;
        public int quickness;
        public int actionPoints;

        public void AiTurn()
        {
            RechargeAP();
            Seek(Zone.Player);
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
            foreach(Entity item in inv.Contents)
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
}
