using VillagePeople.Entities;
using VillagePeople.Entities.NPC;

namespace VillagePeople.StateMachine.States
{
    internal class HerdingSheep : State<MovingEntity>
    {
        private Sheep _sheep;

        public override void Enter(MovingEntity me)
        {
            _sheep = (Sheep) me.World.MovingEntities.Find(m => m.GetType() == typeof(Sheep) && m.Resource.Food > 0);
            me.SetNewTarget(_sheep.Position);
        }

        public override void Execute(MovingEntity me)
        {
            me.SetNewTarget(_sheep.Position);

            if (me.CloseEnough(me.Position, _sheep.Position) && _sheep != null)
                if (me.Resource.TotalResources() < me.MaxInventorySpace && _sheep.Resource.Food > 0) _sheep.Gather(me);
                else
                    me.StateMachine.ChangeState(new ReturningResources());
        }

        public override void Exit(MovingEntity me)
        {
        }
    }
}