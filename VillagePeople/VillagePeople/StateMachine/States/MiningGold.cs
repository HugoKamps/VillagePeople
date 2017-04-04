using System;
using System.Collections.Generic;
using VillagePeople.Behaviours;
using VillagePeople.Entities;
using VillagePeople.Entities.Structures;

namespace VillagePeople.StateMachine.States
{
    class MiningGold : State<MovingEntity>
    {
        public override void Enter(MovingEntity me)
        {
            // Move to gold
            me.SteeringBehaviours = new List<SteeringBehaviour> { new SeekBehaviour(me, me.World.StaticEntities.Find(m => m.GetType() == typeof(GoldMine)).Position) };
            Console.WriteLine("Mining gold");
        }

        public override void Execute(MovingEntity me)
        {
            var gold = (GoldMine)me.World.StaticEntities.Find(m => m.GetType() == typeof(GoldMine));
            if (me.CloseEnough(me.Position, gold.Position)) {
                Console.WriteLine("Gold: " + me.Resource.Gold);

                if (me.Resource.TotalResources() < me.MaxInventorySpace) {
                    //me.Resource.Gold += 1;
                    gold.Gather(me);
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
