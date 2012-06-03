using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ants.Model;
using Microsoft.Xna.Framework;
using Ants.PathFinding;

namespace Ants.States
{
    class FleeState : State
    {
        public FleeState(AntsGame game, Ant agent)
            : base(game, agent)
        {
        }

        public override void Update(GameTime time)
        {
            Agent.Path = AStar.Search(Game.Map, Agent.Square, Game.Hills[Agent.Team].Square, Game.Map.Distance);
            Console.WriteLine(Agent.Path);
        }
    }
}
