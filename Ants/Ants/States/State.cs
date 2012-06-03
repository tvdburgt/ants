using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Ants.Model;

namespace Ants.States
{
    public abstract class State
    {
        protected Ant Agent { get; private set; }
        protected AntsGame Game { get; private set; }

        public State(AntsGame game, Ant agent)
        {
            Agent = agent;
            Game = game;
        }

        public abstract void Update(GameTime time);

        public virtual void Enter()
        {
            Console.WriteLine("Entered state {0} for entity {1}", GetType(), Agent);
        }

        public virtual void Exit()
        {
            Console.WriteLine("Exited state {0} for entity {1}", GetType(), Agent);
        }
    }
}
