using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Ants.Model;
using Ants.PathFinding;

namespace Ants.States
{
    class FlockState : State
    {
        public const int MinCount = 4;
        public const int MaxDistance = MinCount * 4;

        public FlockState(AntsGame game, Ant agent)
            : base(game, agent)
        {
        }

        public override void Update(GameTime time)
        {
            var allies = Game.Ants[Agent.Team].OrderBy(ant => Game.Map.Distance(Agent.Square, ant.Square));

            if (allies.Count() > MinCount && MaxDistance > Game.Map.Distance(Agent.Square, allies.Last().Square))
            {
                Agent.ChangeState(new AttackState(Game, Agent));
                return;
            }

            int x = (int)allies.Average(ant => ant.Square.X);
            int y = (int)allies.Average(ant => ant.Square.Y);

            Agent.Path = AStar.Search(Game.Map, Agent.Square, Game.Map.Squares[y, x], Game.Map.Distance);
           
        }
    }
}
