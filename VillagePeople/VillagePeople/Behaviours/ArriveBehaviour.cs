using VillagePeople.Entities;
using VillagePeople.Util;

namespace VillagePeople.Behaviours
{
    public enum Decelerations { slow = 3, normal = 2, fast = 1 }
    class ArriveBehaviour : SteeringBehaviour
    {
        public Vector2D Target { get; set; }
        private MovingEntity _self;
        private Decelerations _deceleration;

        public ArriveBehaviour(MovingEntity me, Vector2D target) : base(me)
        {
            Target = target.Clone();
            _self = me;
            _deceleration = Decelerations.normal;
        }

        public override Vector2D Calculate()
        {
            var vehicle = _self.Position;
            var toTarget = Target - vehicle;

            var distance = toTarget.Length();

            if (distance > 0)
            {
                const float decelTweaker = 0.5f;
                var speed = distance / ((float)_deceleration * decelTweaker);

                var desiredVelocity = toTarget * speed / distance;

                return desiredVelocity - _self.Velocity;
            }

            return new Vector2D(0, 0);

        }
    }
}
