using VillagePeople.Entities;
using VillagePeople.Entities.Structures;

namespace VillagePeople.StateMachine.States
{
    public class Wandering : State<MovingEntity>
    {
        public override void Enter(MovingEntity me)
        {
        }

        public override void Execute(MovingEntity me)
        {
            // if villiger gets too close change to fleeing
            
            me.StateMachine.ChangeState(StateUtil.Fleeing);
        }

        public override void Exit(MovingEntity me)
        {
        }
    }
}