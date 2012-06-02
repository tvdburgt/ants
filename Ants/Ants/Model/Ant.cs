using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ants.Model
{
    class Ant
    {
        public Square Square { get; private set; }
        public int AttackRate { get; set; }
        public int Health { get; set; }

        public Ant(Square square)
        {
            Square = square;
            Health = 5;
            AttackRate = 1;
        }
    }
}
