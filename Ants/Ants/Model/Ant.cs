using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Ants.Model
{
    public class Ant
    {
        private AntsGame game;
        private int team;
        public Square Square { get; private set; }
        public int AttackRate { get; set; }
        public int Health { get; set; }

        public Ant(AntsGame game, Square square, int team)
        {
            this.game = game;
            this.team = team;
            Square = square;
            Health = 5;
            AttackRate = 1;
        }

        public void Update(GameTime time)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            game.FillSquare(Square, game.AntColors[team]);
        }

        private IEnumerable<Ant> GetEnemyAnts()
        {
            for (int i = 0; i < game.Ants.Length; i++)
            {
                if (i == team)
                    continue;

                foreach (var ant in game.Ants[i])
                {
                    yield return ant;
                }
            }
        }
    }
}
