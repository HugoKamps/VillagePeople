using System;
using System.Drawing;
using VillagePeople.Entities;
using VillagePeople.Util;

namespace VillagePeople.Behaviours {
    internal class ExploreBehaviour : SteeringBehaviour {
        private MovingEntity _self;

        public ExploreBehaviour(MovingEntity m) : base(m) {
            _self = m;
            SetRandomTarget();
        }

        private Vector2D Target { get; set; }

        public void SetRandomTarget() {
            var random = new Random();
            float x = random.Next(0, _self.World.Width);
            float y = random.Next(0, _self.World.Height);
            Target = new Vector2D(x, y).Truncate(_self.World.Width);
        }

        public override Vector2D Calculate() {
            if (Vector2D.Distance(M.Position, Target) < 2) SetRandomTarget();
            return new SeekBehaviour(_self, Target).Calculate();
        }

        public override void RenderSB(Graphics g) { }
    }
}