using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VillagePeople.Behaviours;
using VillagePeople.Entities;

namespace VillagePeople.StateMachine.States
{
    public class BeingPossessed : State<MovingEntity>
    {
        public override void Enter(MovingEntity me)
        {
            me.SteeringBehaviours = new List<SteeringBehaviour> { new SeekBehaviour(me, me.Position) };
            me.Possessed = true;
            Console.WriteLine("Damn, I'm being possessed by something!");
        }

        public override void Execute(MovingEntity me)
        {
            throw new NotImplementedException();
        }

        public override void Exit(MovingEntity me)
        {
            me.Possessed = false;
            Console.WriteLine("I'm no longer possessed.");
        }
    }
}
