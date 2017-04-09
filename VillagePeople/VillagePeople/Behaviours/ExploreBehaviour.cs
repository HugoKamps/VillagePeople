using VillagePeople.Entities;
using VillagePeople.Util;

namespace VillagePeople.Behaviours
{
    internal class ExploreBehaviour : SteeringBehaviour
    {
        private float _radius;
        private MovingEntity _self;
        private bool _up;

        public ExploreBehaviour(MovingEntity m, float radius) : base(m)
        {
            _self = m;
            _radius = radius;
            _up = true;
        }

        public override Vector2D Calculate()
        {
            Vector2D calculated;

            if (_up && _self.Position.Y < _self.Position.Y + _radius)
            {
                calculated =
                    new SeekBehaviour(_self, new Vector2D(_self.Position.X + _radius, _self.Position.Y + _radius))
                        .Calculate();
                _up = !(_self.Position.Y < _self.Position.Y - _radius);
            }
            else
            {
                calculated =
                    new SeekBehaviour(_self, new Vector2D(_self.Position.X + _radius, _self.Position.Y - _radius))
                        .Calculate();
                _up = _self.Position.Y > _self.Position.Y + _radius;
            }

            return calculated;
        }
    }
}