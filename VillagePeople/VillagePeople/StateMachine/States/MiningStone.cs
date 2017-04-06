using System;
using VillagePeople.Entities;
using VillagePeople.Entities.Structures;

namespace VillagePeople.StateMachine.States
{
    class MiningStone : State<MovingEntity> {
        private StoneMine _stone;
        public override void Enter(MovingEntity me)
        {
            // Move to stone
            _stone = (StoneMine)me.World.StaticEntities.Find(m => m.GetType() == typeof(StoneMine) && m.Resource.Stone > 0);
            me.SetSteeringBehaviours(me.Position, _stone.Position);
            Console.WriteLine("Mining gold");
        }

        public override void Execute(MovingEntity me)
        {
            if (me.CloseEnough(me.Position, _stone.Position))
            {
                Console.WriteLine("Stone: " + me.Resource.Stone);

                if (me.Resource.TotalResources() < me.MaxInventorySpace && _stone.Resource.Stone > 0)
                {
                    //me.Resource.Stone += 1;
                    _stone.Gather(me);
                } else
                {
                    me.StateMachine.ChangeState(new ReturningResources());
                }
            }
        }

        public override void Exit(MovingEntity me)
        {
            // Stop mining stone
            Console.WriteLine("Stop mining stone");
        }
    }
}
