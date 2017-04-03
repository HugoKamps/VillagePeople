using System;
using VillagePeople.Entities;

namespace VillagePeople.StateMachine.States
{
    class CuttingWood : State<MovingEntity>
    {
        public override void Enter(MovingEntity me)
        {
            // Move to wood
            Console.WriteLine("Cutting wood");
        }

        public override void Execute(MovingEntity me) {
            Console.WriteLine("Wood: " + me.Resource.Wood);

            if (me.Resource.TotalResources() < me.MaxInventorySpace) {
                me.Resource.Wood += 1;
            }
            else {
                me.StateMachine.ChangeState(new ReturningResources());
            }
        }

        public override void Exit(MovingEntity me) {
            // Stop cutting wood
            Console.WriteLine("Stop cutting wood");
        }
    }
}
