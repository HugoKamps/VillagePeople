using System;
using VillagePeople.Entities;

namespace VillagePeople.StateMachine.States
{
    class MiningStone : State<MovingEntity>
    {
        public override void Enter(MovingEntity me)
        {
            // Move to stone
            Console.WriteLine("Mining gold");
        }

        public override void Execute(MovingEntity me)
        {
            Console.WriteLine("Stone: " + me.Resource.Stone);

            if (me.Resource.TotalResources() < me.MaxInventorySpace)
            {
                me.Resource.Stone += 1;
            } else
            {
                me.StateMachine.ChangeState(new ReturningResources());
            }
        }

        public override void Exit(MovingEntity me)
        {
            // Stop mining stone
            Console.WriteLine("Stop mining stone");
        }
    }
}
