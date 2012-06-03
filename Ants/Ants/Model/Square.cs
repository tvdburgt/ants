using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Ants.Model
{
    public class Square : IComparable<Square>
    {
        public bool IsPassable { get; set; }

        public readonly int X;
        public readonly int Y;
        public readonly Vector2 ScreenPosition;
        public static float Width { get; set; }
        public static float Height { get; set; }
        public SquareScore Score { get; set; }

        public Square(int x, int y, Vector2 screenPosition)
        {
            X = x;
            Y = y;
            IsPassable = true;
            ScreenPosition = screenPosition;
            Score = new SquareScore();
        }

        public int CompareTo(Square other)
        {
            if (other == null)
                throw new ArgumentNullException("Square can't be null");

            // Identical vertices
            if (ReferenceEquals(this, other))
                return 0;

            // Comparison value
            int value = Score.Total.CompareTo(other.Score.Total);

            // Same score; compare hash instead (for deterministic comparison)
            if (value == 0)
                value = GetHashCode().CompareTo(other.GetHashCode());

            return value;
        }
    }
}
