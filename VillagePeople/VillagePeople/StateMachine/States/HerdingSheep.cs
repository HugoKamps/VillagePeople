using System;
using VillagePeople.Entities;

namespace VillagePeople.StateMachine.States
{
    class HerdingSheep : State<MovingEntity>
    {
        public override void Enter(MovingEntity me)
        {
            // Move to sheep
            Console.WriteLine("Herding sheep");
        }

        public override void Execute(MovingEntity me) {
            Console.WriteLine("Food: " + me.Resource.Food);

            if (me.Resource.TotalResources() < me.MaxInventorySpace)
            {
                me.Resource.Food += 1;
            } else
            {
                me.StateMachine.ChangeState(new ReturningResources());
            }
        }

        public override void Exit(MovingEntity me) {
            // Stop herding sheep
            Console.WriteLine("Stop herding sheep");
        }
    }
}
