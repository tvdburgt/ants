using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Ants.Model
{
    public class Map
    {
        private const bool ShowGrid = true;
        private AntsGame game;
        public Square[,] Squares { get; private set; }

        private readonly Color gridColor = new Color(200, 200, 200);

        public int Columns
        {
            get
            {
                return Squares.GetLength(1);
            }
        }

        public int Rows
        {
            get
            {
                return Squares.GetLength(0);
            }
        }

        public Map(AntsGame game, Square[,] squares)
        {
            this.game = game;
            Squares = squares;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (ShowGrid)
                DrawGrid(spriteBatch);

            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Columns; x++)
                {
                    if (!Squares[y, x].IsPassable)
                        game.FillSquare(Squares[y, x].ScreenPosition, Color.Black);
                }
            }
        }

        private void DrawGrid(SpriteBatch spriteBatch)
        {
            var viewport = game.GraphicsDevice.Viewport;

            for (int y = 0; y < Rows; y++)
            {
                var yCoord = y * Square.Height;

                // Horizontal
                game.DrawLine(gridColor, 0, yCoord, viewport.Width, yCoord);

                for (int x = 0; x < Columns; x++)
                {
                    var xCoord = x * Square.Width;

                    // Vertical
                    game.DrawLine(gridColor, xCoord, 0, xCoord, viewport.Height);
                }
            }
        }

        public Square[] GetNeighbors(Square square)
        {
            int x = square.X;
            int y = square.Y;

            var a = -1 % 60;

            return new Square[]
            {
                Squares[(y + 1) % Rows, x], // 12
                Squares[y, (x + 1) % Columns], // 3

                // C#'s modulo doesn't work well with negative numbers, so use ternary statement here
                Squares[y - 1 < 0 ? Rows - 1 : y - 1, x], // 6
                Squares[y, x - 1 < 0 ? Columns - 1 : x - 1], // 9
            };
        }

        public int Distance(Square square1, Square square2)
        {
            int deltaX = Math.Abs(square1.X - square2.X);
            int deltaY = Math.Abs(square1.Y - square2.Y);

            // Get shortest distance (possibly by crossing edges of map)
            return Math.Min(deltaX, Columns - deltaX) + Math.Min(deltaY, Rows - deltaY);
        }
    }
}

