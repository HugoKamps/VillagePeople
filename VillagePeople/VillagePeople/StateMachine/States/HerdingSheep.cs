using System;
using System.Collections.Generic;
using VillagePeople.Behaviours;
using VillagePeople.Entities;
using VillagePeople.Entities.NPC;

namespace VillagePeople.StateMachine.States
{
    class HerdingSheep : State<MovingEntity>
    {
        public override void Enter(MovingEntity me)
        {
            // Move to sheep
            me.SteeringBehaviours = new List<SteeringBehaviour> { new SeekBehaviour(me, me.World.MovingEntities.Find(m => m.GetType() == typeof(Sheep)).Position) };
            Console.WriteLine("Herding sheep");

        }

        public override void Execute(MovingEntity me) {
            if (me.CloseEnough(me.Position, me.World.MovingEntities.Find(m => m.GetType() == typeof(Sheep)).Position))
            {
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
