using VillagePeople.Entities;
using VillagePeople.Util;

namespace VillagePeople.Behaviours
{
    abstract class SteeringBehaviour
    {
        public MovingEntity M { get; set; }
        public abstract Vector2D Calculate();

        public SteeringBehaviour(MovingEntity m)
        {
            M = m;
        }
    }

    
}
