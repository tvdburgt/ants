using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Ants.Model
{
    public class Square
    {
        public bool IsPassable { get; set; }

        public readonly int X;
        public readonly int Y;
        public readonly Vector2 ScreenPosition;

        public static float Width { get; set; }
        public static float Height { get; set; }

        public Square(int x, int y, Vector2 screenPosition)
        {
            X = x;
            Y = y;
            IsPassable = true;
            ScreenPosition = screenPosition;
        }
    }
}
