using VillagePeople.Entities;
using VillagePeople.Util;

namespace VillagePeople.Behaviours
{
    class ExploreBehaviour : SteeringBehaviour
    {
        private MovingEntity _self;
        private float _radius;
        private bool up;

        public ExploreBehaviour(MovingEntity m, float radius) : base(m)
        {
            _self = m;
            _radius = radius;
            up = true;
        }

        public override Vector2D Calculate()
        {
            Vector2D calculated;

            if (up && _self.Position.Y < _self.Position.Y + _radius)
            {
                calculated = new SeekBehaviour(_self, new Vector2D(_self.Position.X + _radius, _self.Position.Y + _radius)).Calculate();
                up = !(_self.Position.Y < _self.Position.Y - _radius);
            } else
            {
                calculated = new SeekBehaviour(_self, new Vector2D(_self.Position.X + _radius, _self.Position.Y - _radius)).Calculate();
                up = _self.Position.Y > _self.Position.Y + _radius;
            }

            return calculated;
        }
    }
}
