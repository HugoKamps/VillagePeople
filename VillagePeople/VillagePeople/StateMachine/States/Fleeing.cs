using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VillagePeople.Entities;

namespace VillagePeople.StateMachine.States
{
    public class Fleeing : State<MovingEntity>
    {
        public override void Enter(MovingEntity me)
        {
        }

        public override void Execute(MovingEntity me)
        {
            // if villiger is far enough change to wander
            // else keep fleeing
            me.StateMachine.ChangeState(StateUtil.Wandering);
        }

        public override void Exit(MovingEntity me)
        {
        }
    }
}
