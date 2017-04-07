using System;
using System.Collections.Generic;
using VillagePeople.Behaviours;
using VillagePeople.Entities;
using VillagePeople.Entities.Structures;

namespace VillagePeople.StateMachine.States
{
    class MiningGold : State<MovingEntity> {
        private GoldMine _gold;
        public override void Enter(MovingEntity me)
        {
            _gold = (GoldMine)me.World.StaticEntities.Find(m => m.GetType() == typeof(GoldMine) && m.Resource.Gold > 0);
            me.SetNewTarget(_gold.Position);
        }

        public override void Execute(MovingEntity me)
        {
            if (me.CloseEnough(me.Position, _gold.Position, 5)) {
                if (me.Resource.TotalResources() < me.MaxInventorySpace && _gold.Resource.Gold > 0) {
                    _gold.Gather(me);
                }
                else {
                    me.StateMachine.ChangeState(new ReturningResources());
                }
            }
        }

        public override void Exit(MovingEntity me) { }
    }
}
