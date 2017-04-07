using System;
using VillagePeople.Entities;
using VillagePeople.Util;

namespace VillagePeople.Behaviours {
    class WanderBehaviour : SteeringBehaviour {
        private MovingEntity _self;
        public Vector2D Target { get; set; }
        public int ElapsedTicks;
        Random random = new Random();

        public WanderBehaviour(MovingEntity m, int elapsedTicks) : base(m) {
            _self = m;
            SetRandomTarget();
            ElapsedTicks = elapsedTicks;
        }

        public void SetRandomTarget() {
            float x = random.Next(_self.World.Width / 2, _self.World.Width);
            float y = random.Next(0, _self.World.Height);
            Target = new Vector2D(x, y);
        }

        public override Vector2D Calculate() {
            ElapsedTicks += 1;
            if (ElapsedTicks % 50 == 0) SetRandomTarget();
            return new SeekBehaviour(_self, Target).Calculate();
        }
    }
}