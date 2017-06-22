using VillagePeople.Entities;
using VillagePeople.Entities.Structures;

namespace VillagePeople.StateMachine.States {
    internal class MiningStone : State<MovingEntity> {
        private StoneMine _stone;

        public override void Enter(MovingEntity me) {
            _stone =
                (StoneMine) me.World.StaticEntities.Find(m => m.GetType() == typeof(StoneMine) && m.Resource.Stone > 0);
            me.SetNewTarget(_stone.Position);
        }

        public override void Execute(MovingEntity me) {
            if (me.CloseEnough(me.Position, _stone.Position, 5))
                if (me.Resource.TotalResources() < me.MaxInventorySpace && _stone.Resource.Stone > 0)
                    _stone.Gather(me);
                else
                    me.StateMachine.ChangeState(new ReturningResources());
        }

        public override void Exit(MovingEntity me) { }
    }
}