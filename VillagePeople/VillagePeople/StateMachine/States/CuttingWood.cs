using System;
using System.Collections.Generic;
using VillagePeople.Behaviours;
using VillagePeople.Entities;
using VillagePeople.Entities.Structures;

namespace VillagePeople.StateMachine.States
{
    class CuttingWood : State<MovingEntity>
    {
        public override void Enter(MovingEntity me)
        {
            // Move to wood
            me.SteeringBehaviours = new List<SteeringBehaviour> { new SeekBehaviour(me, me.World.StaticEntities.Find(m => m.GetType() == typeof(Tree)).Position) };
            Console.WriteLine("Cutting wood");
        }

        public override void Execute(MovingEntity me)
        {
            if (me.CloseEnough(me.Position, me.World.StaticEntities.Find(m => m.GetType() == typeof(Tree)).Position))
            {
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
