using System;
using System.Collections.Generic;
using VillagePeople.Behaviours;
using VillagePeople.Entities;
using VillagePeople.Entities.Structures;
using VillagePeople.Util;

namespace VillagePeople.StateMachine.States
{
    class MiningGold : State<MovingEntity> {
        private StaticEntity target;
        public override void Enter(MovingEntity me)
        {
            // Move to gold
            target = me.World.StaticEntities.Find(m => m.GetType() == typeof(GoldMine));
            me.SteeringBehaviours = new List<SteeringBehaviour> { new SeekBehaviour(me, target.Position) };
            Console.WriteLine("Mining gold");
        }

        public override void Execute(MovingEntity me)
        {
            if (me.CloseEnough(me.Position, target.Position)) {
                Console.WriteLine("Gold: " + me.Resource.Gold);

                if (me.Resource.TotalResources() < me.MaxInventorySpace && target.Resource.Gold > 0) {
                    me.Resource.Gold += 1;
                }
                else {
                    me.StateMachine.ChangeState(new ReturningResources());
                }
            }
        }

        public override void Exit(MovingEntity me)
        {
            Console.WriteLine("Stop mining gold");
            // Stop mining gold
        }
    }
}
