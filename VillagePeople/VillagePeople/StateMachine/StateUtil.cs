using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VillagePeople.StateMachine.States;

namespace VillagePeople.StateMachine
{
    public static class StateUtil
    {
        public readonly static Wandering Wandering = new Wandering();
        public readonly static Fleeing Fleeing = new Fleeing();
    }
}
