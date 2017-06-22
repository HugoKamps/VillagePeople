using System;
using System.Collections.Generic;
using System.Drawing;
using VillagePeople.Entities;
using VillagePeople.Entities.Structures;
using VillagePeople.Util;

namespace VillagePeople.Behaviours {
    class WallAvoidance : SteeringBehaviour {
        private MovingEntity _self;

        private Vector2D[] _feelers;
        private List<Wall> _walls;

        public WallAvoidance(MovingEntity m) : base(m) {
            _self = m;
            _feelers = new Vector2D[3];
            _walls = _self.World.Walls;
        }

        public override Vector2D Calculate() {
            SetFeelers();

            const double distToThisIp = 0.0;
            var distToClosestIp = double.MaxValue;

            var closestWall = -1;

            var steeringForce = new Vector2D();
            var point = new Vector2D();
            var closestPoint = new Vector2D();

            foreach (var feeler in _feelers) {
                for (var w = 0; w < _walls.Count; w++) {
                    if (!Vector2D.LineIntersection2D(_self.Position,
                        feeler,
                        _walls[w].Position,
                        _walls[w].EndPosition,
                        ref point)) continue;

                    if (!(distToThisIp < distToClosestIp)) continue;
                    distToClosestIp = distToThisIp;

                    closestWall = w;

                    closestPoint = point;
                }

                if (closestWall < 0) continue;
                var overShoot = feeler - closestPoint;
                steeringForce = _walls[closestWall].Normal * overShoot.Length();
            }

            return steeringForce;
        }

        private void SetFeelers() {
            const float halfPi = (float) Math.PI / 2;

            _feelers[0] = _self.Position + 100 * _self.Heading;

            var heading = Vector2D.Vec2DRotateAroundOrigin(_self.Heading, halfPi * 3.5f);
            _feelers[1] = _self.Position + 15 * heading;

            heading = Vector2D.Vec2DRotateAroundOrigin(_self.Heading, halfPi * 0.5f);
            _feelers[2] = _self.Position + 15 * heading;
        }

        public override void RenderSB(Graphics g) {
            if (_feelers == null || _feelers.Length <= 0)
                return;

            foreach (var feeler in _feelers)
                if (feeler != null)
                    g.DrawLine(Pens.Purple, _self.Position.X, _self.Position.Y, feeler.X, feeler.Y);
        }
    }
}