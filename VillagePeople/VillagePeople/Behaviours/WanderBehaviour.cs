using System;
using VillagePeople.Entities;
using VillagePeople.Util;

namespace VillagePeople.Behaviours
{
    internal class WanderBehaviour : SteeringBehaviour
    {
        private float _elapsedTime;
        private MovingEntity _self;
        private Vector2D Target { get; set; }

        public WanderBehaviour(MovingEntity m, float elapsedTime) : base(m)
        {
            _self = m;
            SetRandomTarget();
            _elapsedTime = elapsedTime;
        }

        public void SetRandomTarget()
        {
            var random = new Random();
            float x = random.Next(_self.World.Width / 2, _self.World.Width);
            float y = random.Next(0, _self.World.Height);
            Target = new Vector2D(x, y).Truncate(_self.World.Width);
        }

        public override Vector2D Calculate()
        {
            if (_elapsedTime % 100 == 0) SetRandomTarget();
            return new SeekBehaviour(_self, Target).Calculate();
        }
    }
}