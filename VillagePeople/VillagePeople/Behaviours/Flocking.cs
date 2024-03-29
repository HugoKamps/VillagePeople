﻿using System.Drawing;
using VillagePeople.Entities;
using VillagePeople.Util;

namespace VillagePeople.Behaviours {
    internal class Alignment : SteeringBehaviour {
        private MovingEntity _self;
        public int AlignmentRadius = 100;

        public Alignment(MovingEntity m) : base(m) {
            _self = m;
        }

        public override Vector2D Calculate() {
            var j = 0;

            var averageDirection = new Vector2D();
            foreach (var sheep in _self.World.GetLivingSheep()) {
                var distance = _self.Position - sheep.Position;
                if (sheep == _self || !(distance.Length() < AlignmentRadius)) continue;
                j++;
                averageDirection = averageDirection + sheep.Velocity;
            }
            if (j == 0)
                return new Vector2D();

            averageDirection = averageDirection / j;

            return averageDirection - _self.Velocity;
        }

        public override void RenderSB(Graphics g) { }
    }

    internal class Cohesion : SteeringBehaviour {
        private MovingEntity _self;
        public int CohesionRadius = 30;

        public Cohesion(MovingEntity m) : base(m) {
            _self = m;
        }

        public override Vector2D Calculate() {
            var j = 0;

            var averagePosition = new Vector2D();
            foreach (var sheep in _self.World.GetLivingSheep()) {
                var distance = _self.Position - sheep.Position;
                if (sheep == _self || !(distance.Length() < CohesionRadius)) continue;
                j++;
                averagePosition = averagePosition + sheep.Position;
            }
            if (j == 0)
                return new Vector2D();

            averagePosition /= j;

            var desired = (averagePosition - _self.Position).Normalize() * _self.MaxSpeed;

            return desired - _self.Velocity;
        }

        public override void RenderSB(Graphics g) { }
    }


    internal class Separation : SteeringBehaviour {
        private MovingEntity _self;
        private int _separationRadius = 30;

        public Separation(MovingEntity m) : base(m) {
            _self = m;
        }

        public override Vector2D Calculate() {
            var j = 0;

            var separationForce = new Vector2D();
            var averageDirection = new Vector2D();

            foreach (var sheep in _self.World.GetLivingSheep()) {
                var distance = _self.Position - sheep.Position;
                if (sheep == _self || !(distance.Length() < _separationRadius)) continue;
                j++;
                separationForce += _self.Position - sheep.Position;
                separationForce = separationForce.Normalize();
                separationForce = separationForce * (1 / .7f);
                averageDirection = averageDirection + separationForce;
            }

            return j == 0 ? new Vector2D() : averageDirection;
        }

        public override void RenderSB(Graphics g) { }
    }
}