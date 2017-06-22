using VillagePeople.Entities;
using VillagePeople.Entities.NPC;
using VillagePeople.Util;

namespace VillagePeople.StateMachine.States {
    internal class ReturningResources : State<MovingEntity> {
        private int _index = -1;

        public override void Enter(MovingEntity me) {
            me.SetNewTarget(new Vector2D(25, 375));
        }

        public override void Execute(MovingEntity me) {
            if (me.CloseEnough(me.Position, new Vector2D(25, 375))) {
                Resource.DepositResources(me);

                var v = (Villager) me;
                _index = v.GetNextResource();

                if (Resource.IsResourceAvailable(me.World, _index)) {
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
                    if (_index == 3) _index = 0;
                    else _index++;
                }
            }
        }

        public override void Exit(MovingEntity me) { }
    }
}