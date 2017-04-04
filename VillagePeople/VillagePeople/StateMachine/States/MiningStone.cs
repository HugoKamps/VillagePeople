using System;
using System.Collections.Generic;
using VillagePeople.Behaviours;
using VillagePeople.Entities;
using VillagePeople.Entities.Structures;

namespace VillagePeople.StateMachine.States
{
    class MiningStone : State<MovingEntity>
    {
        public override void Enter(MovingEntity me)
        {
            // Move to stone
            me.SteeringBehaviours = new List<SteeringBehaviour> { new SeekBehaviour(me, me.World.StaticEntities.Find(m => m.GetType() == typeof(StoneMine)).Position) };
            Console.WriteLine("Mining gold");
        }

        public override void Execute(MovingEntity me)
        {
            if (me.CloseEnough(me.Position, me.World.StaticEntities.Find(m => m.GetType() == typeof(StoneMine)).Position))
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
        }

        public override void Exit(MovingEntity me)
        {
            // Stop mining stone
            Console.WriteLine("Stop mining stone");
        }
    }
}
