using VillagePeople.Entities;
using VillagePeople.Util;

namespace VillagePeople.Behaviours
{
    public enum Decelerations { Slow = 3, Normal = 2, Fast = 1 }
    class ArriveBehaviour : SteeringBehaviour
    {
        public Vector2D Target { get; set; }
        private MovingEntity _self;
        private Decelerations _deceleration;

        public ArriveBehaviour(MovingEntity me, Vector2D target) : base(me)
        {
            Target = target.Clone();
            _self = me;
            _deceleration = Decelerations.Normal;
        }

        public override Vector2D Calculate()
        {
            var vehicle = _self.Position;
            Target -= vehicle;
            var toTarget = Target;
            var distance = toTarget.Length();

            if (distance > 0)
            {
                const float decelTweaker = 0.5f;
                var speed = distance / ((float)_deceleration * decelTweaker);

                toTarget *= speed;
                toTarget /= distance;
                var desiredVelocity = toTarget;

                desiredVelocity -= _self.Velocity;
                return desiredVelocity;
            }

            return new Vector2D(0, 0);
        }
    }
}
