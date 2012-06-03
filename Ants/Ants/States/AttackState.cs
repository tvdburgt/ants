using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ants.Model;
using Microsoft.Xna.Framework;
using Ants.PathFinding;

namespace Ants.States
{
    class AttackState : State
    {
        public AttackState(AntsGame game, Ant agent)
            : base(game, agent)
        {
        }

        public override void Update(GameTime time)
        {
            Ant nearestFoe = GetNearestFoe();
            Ant nearestAlly = GetNearestAlly();

            int foeDistance = nearestFoe == null ? int.MaxValue : Game.Map.Distance(Agent.Square, nearestFoe.Square);
            int allyDistance = nearestAlly == null ? int.MaxValue : Game.Map.Distance(Agent.Square, nearestAlly.Square);
            var allies = Game.Ants[Agent.Team].OrderBy(ant => Game.Map.Distance(Agent.Square, ant.Square));

            if (allies.Count() < FlockState.MinCount)
            {
                Agent.ChangeState(new FlockState(Game, Agent));
                return;
            }
            
            
            if (nearestFoe != null)
            {
                Agent.Path = AStar.Search(Game.Map, Agent.Square, nearestFoe.Square, Game.Map.Distance);
            }
        }

        private Ant GetNearestAlly()
        {
            var allies = Game.Ants[Agent.Team].Where(ant => ant != Agent);

            if (allies.Count() == 0)
                return null;

            return allies.OrderBy(ant => Game.Map.Distance(Agent.Square, ant.Square)).First();
        }

        private Ant GetNearestFoe()
        {
            var enemyAnts = Agent.GetEnemyAnts();

            if (enemyAnts.Count() == 0)
                return null;

            return enemyAnts.OrderBy(a => Game.Map.Distance(Agent.Square, a.Square)).First();
        }
    }
}
