using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace AstrologyGame
{
    /// <summary>
    /// An ordered pair of integers.
    /// </summary>
    public class OrderedPair : IEquatable<OrderedPair>
    {
        public int X { get; set; }
        public int Y { get; set; }

        public static OrderedPair Zero { get { return new OrderedPair(0, 0); } }

        public OrderedPair()
        {

        }
        public OrderedPair(int _x, int _y)
        {
            X = _x;
            Y = _y;
        }
        public OrderedPair(int _xAndY)
        {
            X = _xAndY;
            Y = _xAndY;
        }

        public bool Equals(OrderedPair other)
        {
            return X == other.X && Y == other.Y;
        }

        // conversions
        public static implicit operator Vector2(OrderedPair p) => new Vector2(p.X, p.Y);
        public static implicit operator OrderedPair((int, int) t) => new OrderedPair(t.Item1, t.Item2);
        public static explicit operator OrderedPair(Vector2 v) => new OrderedPair( (int)v.X, (int)v.Y );

        // math operators
        // Inverse/Negation
        public static OrderedPair operator -(OrderedPair a)
            => new OrderedPair(-a.X, -a.Y);
        // Addition
        public static OrderedPair operator +(OrderedPair a, OrderedPair b)
            => new OrderedPair(a.X + b.X, a.Y + b.Y);
        // Subtraction
        public static OrderedPair operator -(OrderedPair a, OrderedPair b)
            => a + (-b);

        public override string ToString() => $"({X}, {Y})";

        /// <summary>
        /// Given that entities can walk diagonally for the same cost as moving orthagonally, 
        /// this distance formula returns the shortest number of moves an entity would need to make to go from point a to b.
        /// </summary>
        public static int Distance(OrderedPair a, OrderedPair b)
        {
            int distX = Math.Abs(a.X - b.X);
            int distY = Math.Abs(a.Y - b.Y);
            return Math.Max(distX, distY);
        }

        /// <summary>
        /// Returns true only if two OrderedPairs are adjacent. 
        /// Returns false if they are overlapping.
        /// </summary>
        public static bool Adjacent(OrderedPair a, OrderedPair b)
        {
            return Distance(a, b) == 1;
        }

        /// <summary>
        /// Returns a vector-like ordered pair that points from one position to another 
        /// and who's X and Y values are clamped within the bounds of [-1, 1].
        /// </summary>
        /// <param name="from">The position from which to point from.</param>
        /// <param name="to">The position to point to</param>
        public static OrderedPair Towards(OrderedPair from, OrderedPair to)
        {
            OrderedPair towards = to - from;

            if (towards.X > 0)
                towards.X = 1;
            else if (towards.X < 0)
                towards.X = -1;

            if (towards.Y > 0)
                towards.Y = 1;
            else if (towards.Y < 0)
                towards.Y = -1;

            return towards;
        }
    }
}
