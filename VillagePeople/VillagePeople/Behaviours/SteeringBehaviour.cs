using System.Collections.Generic;
using VillagePeople.Entities;
using VillagePeople.Util;

namespace VillagePeople.Behaviours
{
    public abstract class SteeringBehaviour
    {
        private const float DArrive = 0.7f;
        private const float DSeek = 1.0f;
        private const float DSeparation = 0.0f;
        private const float DAlignment = 1.0f;
        private const float DCohesion = 0.7f;
        private const float DWander = 1.0f;
        private const float DExplore = 1.0f;
        
        public MovingEntity M { get; set; }
        public abstract Vector2D Calculate();

        public SteeringBehaviour(MovingEntity m)
        {
            M = m;
        }

        public static Vector2D CalculateWTS(List<SteeringBehaviour> sb, float maxSpeed)
        {
            Vector2D calculated = new Vector2D();
            foreach (var behaviour in sb)
            {
                if (behaviour.GetType() == typeof(ArriveBehaviour))
                    calculated += behaviour.Calculate() * DArrive;

                if (behaviour.GetType() == typeof(SeekBehaviour))
                    calculated += behaviour.Calculate() * DSeek;

                if (behaviour.GetType() == typeof(Separation))
                    calculated += behaviour.Calculate() * DSeparation;

                if (behaviour.GetType() == typeof(Alignment))
                    calculated += behaviour.Calculate() * DAlignment;

                if (behaviour.GetType() == typeof(Cohesion))
                    calculated += behaviour.Calculate() * DCohesion;

                if (behaviour.GetType() == typeof(WanderBehaviour))
                    calculated += behaviour.Calculate() * DWander;

                if (behaviour.GetType() == typeof(ExploreBehaviour))
                    calculated += behaviour.Calculate() * DExplore;
            }
            return calculated.Truncate(maxSpeed);
        }
    }
}
