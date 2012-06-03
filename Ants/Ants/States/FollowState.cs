using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Ants.Model;
using Ants.PathFinding;

namespace Ants.States
{
    class FollowState : State
    {
        private Ant target;

        public FollowState(AntsGame game, Ant agent, Ant target)
            : base(game, agent)
        {
            this.target = target;
        }

        public override void Update(GameTime time)
        {
            if (Agent.GetEnemyAnts().Count() > 0 || target.IsDead)
            {
                Agent.ChangeState(new AttackState(Game, Agent));
            }
            else
            {
                Agent.Path = AStar.Search(Game.Map, Agent.Square, target.Square, Game.Map.Distance);
            }
        }
    }
}
