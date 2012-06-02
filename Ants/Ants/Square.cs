using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ants
{
    class Square
    {
        public bool IsPassable { get; set; }

        public readonly int X;
        public readonly int Y;

        public Square(int x, int y, bool isPassable)
        {
            IsPassable = isPassable;
            X = x;
            Y = y;
        }
    }
}
