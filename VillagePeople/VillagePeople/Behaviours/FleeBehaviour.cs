using System.Drawing;
using VillagePeople.Entities;
using VillagePeople.Util;

namespace VillagePeople.Behaviours {
    internal class FleeBehaviour : SteeringBehaviour {
        private MovingEntity _self;
        private Vector2D _targetPos;

        public FleeBehaviour(MovingEntity m) : base(m) {
            _self = m;
        }

        public override Vector2D Calculate() {
            var desiredV = (_self.Position - _targetPos).Normalize() * _self.MaxSpeed;
            return desiredV - _self.Velocity;
        }

        public override void RenderSB(Graphics g) { }
    }
}