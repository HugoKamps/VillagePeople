using System;
using VillagePeople.Entities;
using VillagePeople.Util;

namespace VillagePeople.Behaviours
{
    class ExploreBehaviour : SteeringBehaviour
    {
        private MovingEntity _self;
        private float _radius;

        public ExploreBehaviour(MovingEntity m, float radius) : base(m) {
            _self = m;
            _radius = radius;
        }

        public override Vector2D Calculate() {
            Vector2D calculated = new Vector2D();

            var vehicle = _self.Position;
            calculated *= vehicle.Normalize();

            

            return calculated;
        }
    }
}
