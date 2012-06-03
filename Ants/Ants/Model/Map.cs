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

        private readonly Color gridColor = new Color(235, 235, 235);

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
                    if (Squares[y, x].IsObstacle)
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

            return new Square[]
            {
                GetSquare(x, y + 1), // 12
                GetSquare(x + 1, y), // 3
                GetSquare(x, y - 1), // 6
                GetSquare(x - 1, y), // 9
            };
        }

        public Square[] GetSquares(Square square, int range)
        {
            if (range < 0)
                throw new ArgumentException("Range must be greater than 0");

            // n = (range * 2 + 1)^2
            int size = range * 2 + 1;
            var squares = new Square[size * size];
            int i = 0;

            for (int y = square.Y - range; y <= square.Y + range; y++)
            {
                for (int x = square.X - range; x <= square.X + range; x++)
                {
                    squares[i++] = GetSquare(x, y);
                }
            }

            return squares;
        }

        // Helper function for toroidal movement (pathfinding etc.)
        private Square GetSquare(int x, int y)
        {
            if (x < 0)
                x = Columns - 1;

            if (y < 0)
                y = Rows - 1;

            return Squares[y % Rows, x % Columns];
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

