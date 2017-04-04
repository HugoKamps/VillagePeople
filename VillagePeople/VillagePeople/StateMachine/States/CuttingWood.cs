using System;
using System.Collections.Generic;
using VillagePeople.Behaviours;
using VillagePeople.Entities;
using VillagePeople.Entities.Structures;

namespace VillagePeople.StateMachine.States
{
    class CuttingWood : State<MovingEntity> {
        private StaticEntity target;
        public override void Enter(MovingEntity me)
        {
            // Move to wood
            target = me.World.StaticEntities.Find(m => m.GetType() == typeof(Tree));
            me.SteeringBehaviours = new List<SteeringBehaviour> { new SeekBehaviour(me, target.Position) };
            Console.WriteLine("Cutting wood");
        }

        public override void Execute(MovingEntity me)
        {
            if (me.CloseEnough(me.Position, target.Position))
            {
                Console.WriteLine("Wood: " + me.Resource.Wood);

                if (me.Resource.TotalResources() < me.MaxInventorySpace && target.Resource.Wood > 0) {
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
