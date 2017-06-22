using System.Drawing;
using VillagePeople.Entities;
using VillagePeople.Util;

namespace VillagePeople.Behaviours {
    public enum Decelerations {
        Slow = 3,
        Normal = 2,
        Fast = 1
    }

    internal class ArriveBehaviour : SteeringBehaviour {
        private Decelerations _deceleration;
        private MovingEntity _self;

        public ArriveBehaviour(MovingEntity me, Vector2D target) : base(me) {
            Target = target;
            _self = me;
            _deceleration = Decelerations.Fast;
        }

        public Vector2D Target { get; set; }

        public override Vector2D Calculate() {
            var vehicle = _self.Position;
            var toTarget = Target - vehicle;
            var distance = toTarget.Length();

            if (distance > 0) {
                const float decelTweaker = 0.5f;
                var speed = distance / ((float) _deceleration * decelTweaker);

                toTarget *= speed;
                toTarget /= distance;
                var desiredVelocity = toTarget;

                desiredVelocity -= _self.Velocity;
                return desiredVelocity;
            }

            return new Vector2D(0, 0);
        }

        public override void RenderSB(Graphics g) { }
    }
}