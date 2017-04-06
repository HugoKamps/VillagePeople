using System;
using System.Collections.Generic;
using VillagePeople.Behaviours;
using VillagePeople.Entities;
using VillagePeople.Entities.Structures;

namespace VillagePeople.StateMachine.States
{
    class CuttingWood : State<MovingEntity> {
        private Tree _tree;
        public override void Enter(MovingEntity me)
        {
            // Move to wood
            _tree = (Tree)me.World.StaticEntities.Find(m => m.GetType() == typeof(Tree) && m.Resource.Wood > 0);
            me.SetSteeringBehaviours(me.Position, _tree.Position);
            Console.WriteLine("Cutting wood");
        }

        public override void Execute(MovingEntity me)
        {
            if (_tree == null)
                me.StateMachine.ChangeState(new ReturningResources());

            if (me.CloseEnough(me.Position, _tree.Position) && _tree != null)
            {
                Console.WriteLine("Wood: " + me.Resource.Wood);

                if (me.Resource.TotalResources() < me.MaxInventorySpace && _tree.Resource.Wood > 0) {
                    //me.Resource.Wood += 1;
                    _tree.Gather(me);
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
