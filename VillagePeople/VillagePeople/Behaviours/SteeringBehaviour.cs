using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using VillagePeople.Entities;
using VillagePeople.Util;

namespace VillagePeople.Behaviours
{
    public abstract class SteeringBehaviour
    {
        private const float DArrive = 1f;
        private const float DSeek = 1f;
        private const float DSeparation = 10f;
        private const float DAlignment =  1.5f;
        private const float DCohesion = 1f;
        private const float DWander = 0.1f;
        private const float DExplore = 1.0f;
        private const float DWallAvoidance = 30.0f;


        public SteeringBehaviour(MovingEntity m)
        {
            M = m;
        }

        public MovingEntity M { get; set; }
        public abstract Vector2D Calculate();

        public static Vector2D CalculateWeightedAverage(List<SteeringBehaviour> steeringBehaviours, float maxSpeed)
        {
            Vector2D accumulate = new Vector2D();

            return steeringBehaviours.Aggregate(accumulate, (current, sb) => current + sb.Calculate());
        }

        public static Vector2D CalculateWts(List<SteeringBehaviour> sb, float maxSpeed)
        {
            var calculated = new Vector2D();
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

                if (behaviour.GetType() == typeof(WallAvoidance))
                    calculated += behaviour.Calculate() * DWallAvoidance;
            }
            return calculated.Truncate(maxSpeed);
        }

        public abstract void RenderSB(Graphics g); 
    }
}