using System.Collections.Generic;
using VillagePeople.Entities;
using VillagePeople.Util;

namespace VillagePeople.Behaviours
{
    class FlockingBehaviour : SteeringBehaviour
    {
        public FlockingBehaviour(MovingEntity m) : base(m)
        {
        }

        public override Vector2D Calculate()
        {
            throw new System.NotImplementedException();
        }

        public void TagNeighbors(MovingEntity me, List<MovingEntity> entities, double radius)
        {
            foreach (var entity in entities)
            {
                entity.UnTag();

                Vector2D to = entity.Position.Sub(me.Position);

                double range = radius + entity.Radius;

                if (entity != me && to.LengthSquared() < range * range)
                    entity.Tag();

            }
        }
    }

    class Alignment : SteeringBehaviour
    {
        private MovingEntity _self;
        private List<MovingEntity> _neighbors;

        public Alignment(MovingEntity m, List<MovingEntity> neighbors) : base(m)
        {
            _self = m;
            _neighbors = neighbors;
        }

        public override Vector2D Calculate()
        {
            Vector2D averageHeading = new Vector2D();
            int NeighborCount = 0;

            foreach (MovingEntity me in _neighbors)
            {
                if (me != _self && me.Tagged)
                {
                    averageHeading = averageHeading.Add(me.Acceleration);
                    NeighborCount++;
                }
            }

            if (NeighborCount > 0)
            {
                averageHeading = averageHeading.Divide(NeighborCount);
                averageHeading = averageHeading.Sub(_self.Acceleration);
            }
            return averageHeading;
        }
    }

    class Separation : SteeringBehaviour
    {
        private MovingEntity _self;
        private List<MovingEntity> _neighbors;

        public Separation(MovingEntity m, List<MovingEntity> neighbors) : base(m)
        {
            _self = m;
            _neighbors = neighbors;
        }

        public override Vector2D Calculate()
        {
            Vector2D steeringForce = new Vector2D();
            foreach (MovingEntity me in _neighbors)
            {
                if (me != _self && me.Tagged)
                {
                    Vector2D toAgent = _self.Position.Sub(me.Position);
                    steeringForce = steeringForce.Add(toAgent.Normalize().Divide(toAgent.Length()));
                }
            }
            return steeringForce;
        }
    }

    class Cohesion : SteeringBehaviour
    {
        private MovingEntity _self;
        private List<MovingEntity> _neighbors;

        public Cohesion(MovingEntity m, List<MovingEntity> neighbors) : base(m)
        {
            _self = m;
            _neighbors = neighbors;
        }

        public override Vector2D Calculate()
        {
            Vector2D centerOfMass = new Vector2D();
            Vector2D steeringForce = new Vector2D();
            int neighborCount = 0;

            foreach (MovingEntity me in _neighbors)
            {
                if (me != _self && me.Tagged)
                {
                    centerOfMass = centerOfMass.Add(me.Position);
                    neighborCount++;
                }
            }

            if (neighborCount > 0)
            {
                centerOfMass = centerOfMass.Divide(neighborCount);
                steeringForce = new SeekBehaviour(_self, centerOfMass).Calculate();
            }

            return steeringForce;
        }
    }


}
