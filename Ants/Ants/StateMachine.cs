using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ants.States;
using Microsoft.Xna.Framework;

namespace Ants
{
    public class StateMachine
    {
        private State currentState;

        public StateMachine()
        {
        }

        public StateMachine(State state)
        {
            ChangeState(state);
        }

        public void Update(GameTime time)
        {
            if (currentState != null)
                currentState.Update(time);
        }

        public void ChangeState(State state)
        {
            if (currentState != null)
                currentState.Exit();

            currentState = state;
            currentState.Enter();
        }
    }
}
