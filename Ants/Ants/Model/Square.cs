using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Ants.Model
{
    class Square
    {
        public bool IsPassable { get; set; }

        public readonly int X;
        public readonly int Y;

        public Square(int x, int y)
        {
            X = x;
            Y = y;
            IsPassable = true;
        }

        //public void Draw(SpriteBatch batch)
        //{
        //}
    }
}
