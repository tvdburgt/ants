using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Ants.States;

namespace Ants.Model
{
    public class Ant
    {
        private AntsGame game;
        private StateMachine fsm;
        private Vector2 position;
        private Vector2 animationMargin;
        private Square square;

        public Square Square
        {
            get
            {
                return square;
            }
            set
            {
                if (square != null)
                    square.Ant = null;

                square = value;
                square.Ant = this;
            }
        }

        public readonly int Team;
        public int AttackRate { get; set; }
        public int Health { get; set; }
        public Stack<Square> Path { get; set; }

        public Ant(AntsGame game, Square square, int team)
        {
            this.game = game;
            Team = team;
            Square = square;
            Health = 5;
            AttackRate = 1;
            position = square.ScreenPosition;
            Path = new Stack<Square>();
            fsm = new StateMachine();
            animationMargin = new Vector2(Square.Width * 2, Square.Height * 2);
        }

        public void Update(GameTime time, bool tick, float progress)
        {
            if (tick)
            {
                fsm.Update(time);

                if (Path.Count > 0)
                {
                    Square nextSquare = Path.Pop();

                    if (nextSquare.Ant != null)
                        Path.Clear();
                    else
                        Square = nextSquare;
                }
            }

            else if (position != Square.ScreenPosition)
            {
                Vector2 delta = Square.ScreenPosition - position;

                if (Math.Abs(delta.X) > animationMargin.X|| Math.Abs(delta.Y) > animationMargin.Y)
                {
                    position = Square.ScreenPosition;
                }

                position += delta * progress;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            game.FillSquare(position, game.AntColors[Team]);
        }

        public IEnumerable<Ant> GetEnemyAnts()
        {
            for (int i = 0; i < game.Ants.Length; i++)
            {
                if (i == Team)
                    continue;

                foreach (var ant in game.Ants[i])
                {
                    yield return ant;
                }
            }
        }

        public void ChangeState(State state)
        {
            fsm.ChangeState(state);
        }

        /// <summary>
        /// Attacks enemy ant by decreasing the target's health by the attacker's attack rate
        /// </summary>
        /// <param name="target"></param>
        /// <returns>true if target ant is killed, false otherwise</returns>
        public bool Attack(Ant target)
        {
            // Check if ant is already dead
            if (target.Health <= 0)
                return false;


            target.Health -= AttackRate;

            if (target.Health <= 0)
            {
                AttackRate++;
                Console.WriteLine("{0} killed {1} (new attack rate: {2})", this, target, AttackRate);
                return true;
            }

            return false;
        }
    }
}
