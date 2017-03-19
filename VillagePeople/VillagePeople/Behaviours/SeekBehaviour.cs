using VillagePeople.Entities;
using VillagePeople.Util;

namespace VillagePeople.Behaviours
{
    class SeekBehaviour : SteeringBehaviour
    {
        public Vector2D Target { get; set; }
        private MovingEntity _self;

        public SeekBehaviour(MovingEntity m, Vector2D target) : base(m)
        {
            Target = target;
            _self = m;
        }

        public override Vector2D Calculate()
        {
            var target = Target.Clone();
            var me = _self.Position.Clone();
            var t1 = target - me;
            var desiredVelocity = t1.Normalize() * _self.MaxSpeed;

            return desiredVelocity - _self.Velocity;
        }
    }
}
