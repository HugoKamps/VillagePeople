
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VillagePeople.Entities;
using VillagePeople.Util;

namespace VillagePeople.Behaviours
{
    class FleeBehaviour : SteeringBehaviour
    {
        private Vector2D _targetPos;
        private MovingEntity _self;

        public FleeBehaviour(MovingEntity m) : base(m) {
            _self = m;
        }

        public override Vector2D Calculate()
        {
            if (_targetPos == null ||
                (_targetPos - _self.Position).Length()  > 100)
                return new Vector2D();

            var desiredV = (_self.Position - _targetPos).Normalize() * _self.MaxSpeed;
            return desiredV - _self.Velocity;
        }

        public override void RenderSB(Graphics g) { }
    }
}
