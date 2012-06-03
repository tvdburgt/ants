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
                        game.FillSquare(Squares[y, x], Color.Black);
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
                Squares[(y + 1) % Rows, x], // 12
                Squares[y, (x + 1) % Columns], // 3
                Squares[(y - 1) % Rows, x], // 6
                Squares[y, (x - 1) % Columns], // 9
            };
        }

        //private void DrawSquare(SpriteBatch spriteBatch, Color color, float x, float y)
        //{
        //    spriteBatch.Draw(blankTexture, new Rectangle((int)x, (int)y, (int)Square.Width, (int)Square.Height), color);
        //}

        //private void DrawGridLine(SpriteBatch spriteBatch, Color color, float x1, float y1, float x2, float y2)
        //{
        //    var point1 = new Vector2(x1, y1);
        //    var point2 = new Vector2(x2, y2);

        //    float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
        //    float length = Vector2.Distance(point1, point2);

        //    spriteBatch.Draw(blankTexture, point1, null, color,
        //               angle, Vector2.Zero, new Vector2(length, 1),
        //               SpriteEffects.None, 0);
        //}
    }
}

