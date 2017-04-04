using System;
using System.Collections.Generic;
using VillagePeople.Behaviours;
using VillagePeople.Entities;

namespace VillagePeople.StateMachine.States
{
    class HerdingSheep : State<MovingEntity>
    {
        public override void Enter(MovingEntity me)
        {
            // Move to sheep
            me.SteeringBehaviours = new List<SteeringBehaviour> { new SeekBehaviour(me, me.World.StaticEntities[0].Position) };
            Console.WriteLine("Herding sheep");

        }

        public override void Execute(MovingEntity me) {
            if (me.Position.X - me.World.StaticEntities[0].Position.X < 5) {
                Console.WriteLine("Food: " + me.Resource.Food);

                if (me.Resource.TotalResources() < me.MaxInventorySpace) {
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
