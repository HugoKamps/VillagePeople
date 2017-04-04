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
            var tree = (Tree)me.World.StaticEntities.Find(m => m.GetType() == typeof(Tree));

            if (tree == null)
                me.StateMachine.ChangeState(new ReturningResources());

            if (me.CloseEnough(me.Position, tree.Position))
            {
                Console.WriteLine("Wood: " + me.Resource.Wood);

                if (me.Resource.TotalResources() < me.MaxInventorySpace) {
                    //me.Resource.Wood += 1;
                    tree.Gather(me);
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
