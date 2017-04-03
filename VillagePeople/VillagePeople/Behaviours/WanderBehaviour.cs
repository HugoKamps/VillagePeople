using System;
using System.Configuration.Assemblies;
using VillagePeople.Entities;
using VillagePeople.Util;

namespace VillagePeople.Behaviours
{
    class WanderBehaviour : SteeringBehaviour
    {
        private MovingEntity _self;
        private float _wanderRadius;
        private float _wanderDistance;
        private float _wanderJitter;

        public Vector2D Target { get; set; }

        public WanderBehaviour(MovingEntity m, Vector2D target) : base(m)
        {
            _self = m;
            Target = target;
            _wanderRadius = 10;
            _wanderDistance = 10;
            _wanderJitter = 10;
        }

        public int RandomClamped()
        {
            Random random = new Random();
            return random.Next(-1, 1);
        }

        public override Vector2D Calculate()
        {
            Vector2D me = _self.Position.Clone();
            Vector2D target = Target.Clone();
            target += new Vector2D(RandomClamped() * _wanderJitter, RandomClamped() * _wanderJitter);
            target.Normalize();
            target = target * _wanderRadius;

            Vector2D targetLocal = target + new Vector2D(_wanderDistance, 0);
            Vector2D targetWorld = PointToWorldSpace(targetLocal,
            _self.Velocity,
            _self.Velocity * Matrix.Identity().Rotate(90),
            me);

            return targetWorld - me;
        }

        public Vector2D PointToWorldSpace(Vector2D targetLocal, Vector2D heading, Vector2D side, Vector2D position)
        {
            var transformPoint = new Matrix(1, 1, targetLocal.X, targetLocal.Y, 1, 1, 1, 1, 1);

            var transformMatrix = new Matrix(heading.X, heading.Y, 40,
                                                    side.X, side.Y, 60,
                                                    position.X, position.Y, 1);

            transformPoint = transformPoint * transformMatrix;

            var tempX = transformPoint.M[1, 1];
            var tempY = transformPoint.M[1, 2];

            return new Vector2D(tempX, tempY);
        }
    }
}
