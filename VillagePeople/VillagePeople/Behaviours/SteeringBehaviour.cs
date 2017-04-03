using System.Collections.Generic;
using VillagePeople.Entities;
using VillagePeople.Util;

namespace VillagePeople.Behaviours
{
    abstract class SteeringBehaviour
    {

        private const float PrArrive = 0.7f;
        private const float PrSeek = 0.7f;
        private const float PrSeparation = 0.9f;
        private const float PrAlignment = 0.6f;
        private const float PrCohesion = 0.5f;
        private const float PrWander = 1.0f;
        
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
                if (behaviour.GetType() == typeof(ArriveBehaviour))
                    calculated += behaviour.Calculate() * PrArrive;

                if (behaviour.GetType() == typeof(SeekBehaviour))
                    calculated += behaviour.Calculate() * PrSeek;

                if (behaviour.GetType() == typeof(Separation))
                    calculated += behaviour.Calculate() * PrSeparation;

                if (behaviour.GetType() == typeof(Alignment))
                    calculated += behaviour.Calculate() * PrAlignment;

                if (behaviour.GetType() == typeof(Cohesion))
                    calculated += behaviour.Calculate() * PrCohesion;

                if (behaviour.GetType() == typeof(WanderBehaviour))
                    calculated += behaviour.Calculate() * PrWander;
            }
            return calculated;
        }
    }


}
