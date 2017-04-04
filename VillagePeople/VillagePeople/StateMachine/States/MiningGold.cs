using System;
using VillagePeople.Entities;

namespace VillagePeople.StateMachine.States
{
    class MiningGold : State<MovingEntity>
    {
        public override void Enter(MovingEntity me)
        {
            Console.WriteLine("Mining gold");
            // Move to gold
        }

        public override void Execute(MovingEntity me)
        {
            Console.WriteLine("Gold: " + me.Resource.Gold);

            if (me.Resource.TotalResources() < me.MaxInventorySpace)
            {
                me.Resource.Gold += 1;
            } else
            {
                me.StateMachine.ChangeState(new ReturningResources());
            }
        }

        public override void Exit(MovingEntity me)
        {
            Console.WriteLine("Stop mining gold");
            // Stop mining gold
        }
    }
}
