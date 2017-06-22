using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using VillagePeople.Entities;
using VillagePeople.Util;

namespace VillagePeople.Behaviours {
    public abstract class SteeringBehaviour {
        public SteeringBehaviour(MovingEntity m) {
            M = m;
        }

        public MovingEntity M { get; set; }
        public abstract Vector2D Calculate();

        public static Vector2D CalculateWeightedAverage(List<SteeringBehaviour> steeringBehaviours, float maxSpeed) {
            var accumulate = new Vector2D();

            return steeringBehaviours.Aggregate(accumulate, (current, sb) => current + sb.Calculate());
        }

        public abstract void RenderSB(Graphics g);
    }
}