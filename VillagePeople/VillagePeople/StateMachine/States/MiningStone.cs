using System;
using System.Collections.Generic;
using VillagePeople.Behaviours;
using VillagePeople.Entities;

namespace VillagePeople.StateMachine.States
{
    class MiningStone : State<MovingEntity>
    {
        public override void Enter(MovingEntity me)
        {
            // Move to stone
            me.SteeringBehaviours = new List<SteeringBehaviour> { new SeekBehaviour(me, me.World.StaticEntities[0].Position) };
            Console.WriteLine("Mining gold");
        }

        public override void Execute(MovingEntity me)
        {
            if (me.Position.X - me.World.StaticEntities[0].Position.X < 5) {
                Console.WriteLine("Stone: " + me.Resource.Stone);

                if (me.Resource.TotalResources() < me.MaxInventorySpace) {
                    me.Resource.Stone += 1;
                }
                else {
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
