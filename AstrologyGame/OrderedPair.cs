using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace AstrologyGame
{
    /// <summary>
    /// An ordered pair of integers.
    /// </summary>
    public struct OrderedPair : IEquatable<OrderedPair>
    {
        public int X { get; set; }
        public int Y { get; set; }

        public OrderedPair(int _x, int _y)
        {
            X = _x;
            Y = _y;
        }
        public bool Equals(OrderedPair other)
        {
            return other.X == X && other.Y == Y;
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
            => new OrderedPair(a.X + b.X, a.Y - b.Y);
        // Subtraction
        public static OrderedPair operator -(OrderedPair a, OrderedPair b)
            => a + (-b);

        public override string ToString() => "({X}, {Y})";
    }
}
