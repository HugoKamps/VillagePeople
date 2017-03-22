using System.Collections.Generic;
using System.Windows.Forms;
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

        public static Vector2D CalculateDithered(List<SteeringBehaviour> sb)
        {
            Vector2D calculated = new Vector2D();
            foreach (var behaviour in sb)
            {
                calculated.Add(behaviour.Calculate());
            }
            return calculated;
        }
    }

    
}
