using VillagePeople.Entities;
using VillagePeople.Entities.Structures;

namespace VillagePeople.StateMachine.States
{
    internal class CuttingWood : State<MovingEntity>
    {
        private Tree _tree;

        public override void Enter(MovingEntity me)
        {
            _tree = (Tree) me.World.StaticEntities.Find(m => m.GetType() == typeof(Tree) && m.Resource.Wood > 0);
            me.SetNewTarget(_tree.Position);
        }

        public override void Execute(MovingEntity me)
        {
            if (_tree == null)
                me.StateMachine.ChangeState(new ReturningResources());
            else if (me.CloseEnough(me.Position, _tree.Position, 5) && _tree != null)
                if (me.Resource.TotalResources() < me.MaxInventorySpace && _tree.Resource.Wood > 0)
                    _tree.Gather(me);
                else
                    me.StateMachine.ChangeState(new ReturningResources());
        }

        public override void Exit(MovingEntity me)
        {
        }
    }
}