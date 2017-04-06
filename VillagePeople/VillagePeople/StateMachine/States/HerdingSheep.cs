using System;
using System.Collections.Generic;
using VillagePeople.Behaviours;
using VillagePeople.Entities;
using VillagePeople.Entities.NPC;

namespace VillagePeople.StateMachine.States
{
    class HerdingSheep : State<MovingEntity> {
        private Sheep _sheep;
        public override void Enter(MovingEntity me)
        {
            // Move to sheep
            _sheep = (Sheep)me.World.MovingEntities.Find(m => m.GetType() == typeof(Sheep) && m.Resource.Food > 0);
            me.SetSteeringBehaviours(me.Position, _sheep.Position);
            Console.WriteLine("Herding sheep");

        }

        public override void Execute(MovingEntity me) {
            if (me.CloseEnough(me.Position, _sheep.Position))
            {
                Console.WriteLine("Food: " + me.Resource.Food);

                if (me.Resource.TotalResources() < me.MaxInventorySpace && _sheep.Resource.Food > 0) {
                    me.Resource.Food += 1;
                }
                else {
                    me.StateMachine.ChangeState(new ReturningResources());
                }
            }
        }

        public override void Exit(MovingEntity me) {
            // Stop herding sheep
            Console.WriteLine("Stop herding sheep");
        }
    }
}
