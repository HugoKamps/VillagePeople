using System;
using System.Collections.Generic;
using System.Linq;
using VillagePeople.Behaviours;
using VillagePeople.Entities;
using VillagePeople.Util;

namespace VillagePeople.StateMachine.States
{
    class ReturningResources : State<MovingEntity> {
        private int _index = -1;

        public override void Enter(MovingEntity me)
        {
            me.SetNewTarget(me.Position, new Vector2D((float)me.World.Width / 2, (float)me.World.Height / 2));
        }

        public override void Execute(MovingEntity me) {
            if (me.CloseEnough(me.Position, new Vector2D((float)me.World.Width / 2, (float)me.World.Height / 2), 5))
            {
                Resource.DepositResources(me);

                if(_index == -1) _index = Resource.GetLowestResource(me);
                if (Resource.IsResourceAvailable(me, _index)) {
                    switch (_index) {
                        case 0:
                            me.StateMachine.ChangeState(new CuttingWood());
                            break;
                        case 1:
                            me.StateMachine.ChangeState(new MiningStone());
                            break;
                        case 2:
                            me.StateMachine.ChangeState(new MiningGold());
                            break;
                        case 3:
                            me.StateMachine.ChangeState(new HerdingSheep());
                            break;
                        default:
                            me.StateMachine.ChangeState(new CuttingWood());
                            break;
                    }
                }
                else {
                    _index++;
                }
            }
        }

        public override void Exit(MovingEntity me) { }
    }
}
