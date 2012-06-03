using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Ants.Model
{
    public class Hill
    {
        private AntsGame game;
        public readonly Square Square;
        private Texture2D texture;
        private Vector2 textureOrigin;
        private Vector2 center;

        public Hill(AntsGame game, Square square, int team)
        {
            this.game = game;
            Square = square;
            LoadTexture();
        }

        private void LoadTexture()
        {
            texture = game.CreateCircle((int)(Square.Width * 0.4f), Color.White);
            textureOrigin = new Vector2(texture.Width / 2f, texture.Height / 2f);

            center = Square.ScreenPosition + new Vector2(Square.Width / 2f, Square.Height / 2f);
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            spriteBatch.Draw(texture, center, null, color, 0f, textureOrigin, 1f, SpriteEffects.None, 0f);
        }
    }
}
