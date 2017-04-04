using System;
using System.Collections.Generic;
using VillagePeople.Behaviours;
using VillagePeople.Entities;

namespace VillagePeople.StateMachine.States
{
    class CuttingWood : State<MovingEntity>
    {
        public override void Enter(MovingEntity me)
        {
            // Move to wood
            me.SteeringBehaviours = new List<SteeringBehaviour> { new SeekBehaviour(me, me.World.StaticEntities[0].Position) };
            Console.WriteLine("Cutting wood");
        }

        public override void Execute(MovingEntity me)
        {
            if (me.Position.X - me.World.StaticEntities[0].Position.X < 5) {
                Console.WriteLine("Wood: " + me.Resource.Wood);

                if (me.Resource.TotalResources() < me.MaxInventorySpace) {
                    me.Resource.Wood += 1;
                }
                else {
                    me.StateMachine.ChangeState(new ReturningResources());
                }
            }
        }

        public override void Exit(MovingEntity me)
        {
            // Stop cutting wood
            Console.WriteLine("Stop cutting wood");
        }
    }
}
