using System;
using VillagePeople.Entities;
using VillagePeople.Util;

namespace VillagePeople.Behaviours {
    class WanderBehaviour : SteeringBehaviour {
        private MovingEntity _self;
        private Vector2D Target { get; set; }
        private float _elapsedTime;
        Random random = new Random();

        public WanderBehaviour(MovingEntity m, float elapsedTime) : base(m) {
            _self = m;
            SetRandomTarget();
            _elapsedTime = elapsedTime;
        }

        public void SetRandomTarget() {
            float x = random.Next(_self.World.Width / 2, _self.World.Width);
            float y = random.Next(0, _self.World.Height);
            Target = new Vector2D(x, y);
        }

        public override Vector2D Calculate() {
            _elapsedTime += 1;
            if (_elapsedTime % 50 == 0) SetRandomTarget();
            return new SeekBehaviour(_self, Target).Calculate();
        }
    }
}