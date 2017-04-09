using System.Collections.Generic;
using VillagePeople.Entities;
using VillagePeople.Util;

namespace VillagePeople.Behaviours
{
    internal class Alignment : SteeringBehaviour
    {
        private List<MovingEntity> _movingEntities;
        private MovingEntity _self;

        public Alignment(MovingEntity m, List<MovingEntity> movingEntities) : base(m)
        {
            _self = m;
            _movingEntities = movingEntities;
        }

        public override Vector2D Calculate()
        {
            var v = new Vector2D();
            var neighborCount = 0;

            foreach (var me in _movingEntities)
                if (me != _self && _self.CloseEnough(_self.Position, me.Position, 40))
                {
                    v.X += me.Velocity.X;
                    v.Y += me.Velocity.Y;
                    neighborCount += 1;
                }

            if (neighborCount == 0) return v;
            v.X /= neighborCount;
            v.Y /= neighborCount;
            v.Normalize();
            return v;
        }
    }

    internal class Cohesion : SteeringBehaviour
    {
        private List<MovingEntity> _movingEntities;
        private MovingEntity _self;

        public Cohesion(MovingEntity m, List<MovingEntity> movingEntities) : base(m)
        {
            _self = m;
            _movingEntities = movingEntities;
        }

        public override Vector2D Calculate()
        {
            var v = new Vector2D();
            var neighborCount = 0;

            foreach (var me in _movingEntities)
                if (me != _self && _self.CloseEnough(_self.Position, me.Position, 80))
                {
                    v.X += me.Position.X;
                    v.Y += me.Position.Y;
                    neighborCount += 1;
                }

            if (neighborCount == 0) return v;
            v.X /= neighborCount;
            v.Y /= neighborCount;
            v = new Vector2D(v.X - _self.Position.X, v.Y - _self.Position.Y);
            v.Normalize();
            return v;
        }
    }


    internal class Separation : SteeringBehaviour
    {
        private List<MovingEntity> _movingEntities;
        private MovingEntity _self;

        public Separation(MovingEntity m, List<MovingEntity> movingEntities) : base(m)
        {
            _self = m;
            _movingEntities = movingEntities;
        }

        public override Vector2D Calculate()
        {
            var v = new Vector2D();
            var neighborCount = 0;

            foreach (var me in _movingEntities)
                if (me != _self && _self.CloseEnough(_self.Position, me.Position, 40))
                {
                    v.X += me.Position.X - _self.Position.X;
                    v.Y += me.Position.Y - _self.Position.Y;
                    neighborCount += 1;
                }

            if (neighborCount == 0) return v;
            v.X /= neighborCount;
            v.Y /= neighborCount;
            v.X *= -1;
            v.Y *= -1;
            v.Normalize();
            return v;
        }
    }
}