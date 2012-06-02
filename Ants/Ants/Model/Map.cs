using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Ants.Model
{
    class Map
    {
        private AntsGame game;
        private Square[,] squares;
        private Texture2D blankTexture;

        public float SquareWidth { get; private set; }
        public float SquareHeight { get; private set; }

        public int Width
        {
            get
            {
                return squares.GetLength(1);
            }
        }

        public int Height
        {
            get
            {
                return squares.GetLength(0);
            }
        }

        public Map(AntsGame game, Square[,] squares)
        {
            this.game = game;
            this.squares = squares;
            Initialize();
        }

        public void Initialize()
        {
            blankTexture = new Texture2D(game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blankTexture.SetData(new[] { Color.White });

            var viewport = game.GraphicsDevice.Viewport;
            SquareHeight = (float)viewport.Height / Height;
            SquareWidth = (float)viewport.Width / Width;
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            var viewport = game.GraphicsDevice.Viewport;

            for (int y = 0; y < Height; y++)
            {

                var yCoord = y * SquareHeight - 1;

                // Horizontal line
                DrawGridLine(spriteBatch, Color.Black, 0, yCoord, viewport.Width, yCoord);

                for (int x = 0; x < Width; x++)
                {
                    var xCoord = x * SquareWidth;

                    // Vertical line
                    DrawGridLine(spriteBatch, Color.Black, xCoord, 0, xCoord, viewport.Height);



                    if (!squares[y, x].IsPassable)
                        DrawSquare(spriteBatch, Color.LightGray, x * SquareWidth, y * SquareHeight);

                    //squares[y, x].Draw(spriteBatch);
                }
            }
        }

        private void DrawSquare(SpriteBatch spriteBatch, Color color, float x, float y)
        {
            spriteBatch.Draw(blankTexture, new Rectangle((int)x, (int)y, (int)SquareWidth, (int)SquareHeight), color);
        }

        private void DrawGridLine(SpriteBatch spriteBatch, Color color, float x1, float y1, float x2, float y2)
        {
            var point1 = new Vector2(x1, y1);
            var point2 = new Vector2(x2, y2);

            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            spriteBatch.Draw(blankTexture, point1, null, color,
                       angle, Vector2.Zero, new Vector2(length, 1),
                       SpriteEffects.None, 0);
        }
    }
}

