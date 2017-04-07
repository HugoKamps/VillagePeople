using System.Collections.Generic;
using VillagePeople.Entities;
using VillagePeople.Util;

namespace VillagePeople.Behaviours {
    class Alignment : SteeringBehaviour {
        private MovingEntity _self;
        private List<MovingEntity> _movingEntities;

        public Alignment(MovingEntity m, List<MovingEntity> movingEntities) : base(m) {
            _self = m;
            _movingEntities = movingEntities;
        }

        public override Vector2D Calculate() {
            Vector2D v = new Vector2D();
            int neighborCount = 0;

            foreach (MovingEntity me in _movingEntities) {
                if (me != _self && _self.CloseEnough(_self.Position, me.Position, 40)) {
                    v.X += me.Velocity.X;
                    v.Y += me.Velocity.Y;
                    neighborCount += 1;
                }
            }

            if (neighborCount == 0) return v;
            v.X /= neighborCount;
            v.Y /= neighborCount;
            v.Normalize();
            return v;
        }
    }

    class Cohesion : SteeringBehaviour {
        private MovingEntity _self;
        private List<MovingEntity> _movingEntities;

        public Cohesion(MovingEntity m, List<MovingEntity> movingEntities) : base(m) {
            _self = m;
            _movingEntities = movingEntities;
        }

        public override Vector2D Calculate() {
            Vector2D v = new Vector2D();
            int neighborCount = 0;

            foreach (MovingEntity me in _movingEntities) {
                if (me != _self && _self.CloseEnough(_self.Position, me.Position, 80)) {
                    v.X += me.Position.X;
                    v.Y += me.Position.Y;
                    neighborCount += 1;
                }
            }

            if (neighborCount == 0) return v;
            v.X /= neighborCount;
            v.Y /= neighborCount;
            v = new Vector2D(v.X - _self.Position.X, v.Y - _self.Position.Y);
            v.Normalize();
            return v;
        }
    }


    class Separation : SteeringBehaviour
    {
        private MovingEntity _self;
        private List<MovingEntity> _movingEntities;

        public Separation(MovingEntity m, List<MovingEntity> movingEntities) : base(m)
        {
            _self = m;
            _movingEntities = movingEntities;
        }

        public override Vector2D Calculate()
        {
            Vector2D v = new Vector2D();
            int neighborCount = 0;

            foreach (MovingEntity me in _movingEntities)
            {
                if (me != _self && _self.CloseEnough(_self.Position, me.Position, 40))
                {
                    v.X += me.Position.X - _self.Position.X;
                    v.Y += me.Position.Y - _self.Position.Y;
                    neighborCount += 1;
                }
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
