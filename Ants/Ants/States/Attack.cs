using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ants.Model;
using Microsoft.Xna.Framework;
using Ants.PathFinding;

namespace Ants.States
{
    class Attack : State
    {
        public Attack(AntsGame game, Ant agent)
            : base(game, agent)
        {
        }

        public override void Update(GameTime time)
        {
            if (Agent.Path.Count == 0)
            {
                var enemyAnts = Agent.GetEnemyAnts();

                if (enemyAnts.Count() == 0)
                    return;

                Ant nearestAnt = enemyAnts.OrderBy(a => Game.Map.Distance(Agent.Square, a.Square)).First();
                Square target = null; 

                foreach (var square in Game.Map.GetNeighbors(nearestAnt.Square))
                {
                    if (square.IsPassable)
                    {
                        target = square;
                        break;
                    }
                }

                if (target != null)
                    Agent.Path = AStar.Search(Game.Map, Agent.Square, target, Game.Map.Distance);

            }
        }
    }
}
