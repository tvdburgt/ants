﻿using System;
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
        private int team;
        private StateMachine fsm;
        private Vector2 position;

        public Square Square { get; private set; }
        public int AttackRate { get; set; }
        public int Health { get; set; }
        public Stack<Square> Path { get; set; }

        public Ant(AntsGame game, Square square, int team)
        {
            this.game = game;
            this.team = team;
            Square = square;
            Health = 5;
            AttackRate = 1;
            position = square.ScreenPosition;

            fsm = new StateMachine();
        }

        public void Update(GameTime time, bool tick, float progress)
        {
            if (tick)
            {
                
                if (Path != null && Path.Count > 0)
                {
                    Square = Path.Pop();
                }
            }

            else if (position != Square.ScreenPosition)
            {
                Vector2 delta = Square.ScreenPosition - position;
                position += delta * progress;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            game.FillSquare(position, game.AntColors[team]);
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

        public void ChangeState(State state)
        {
            fsm.ChangeState(state);
        }
    }
}
