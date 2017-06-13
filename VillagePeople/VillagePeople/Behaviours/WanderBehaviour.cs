using System;
using System.Drawing;
using System.Windows.Forms;
using VillagePeople.Entities;
using VillagePeople.Util;

namespace VillagePeople.Behaviours
{
    internal class WanderBehaviour : SteeringBehaviour
    {
        private MovingEntity _self;

        private Random _random = new Random();

        private PointF _circleCenterG;
        private Vector2D _wanderTarget;
        private const float WanderRadius = 40;
        private const float WanderDistance = 10;
        private const int WanderJitter = 30;
        private Vector2D _pointOnCircle;

        public WanderBehaviour(MovingEntity m) : base(m)
        {
            _wanderTarget = new Vector2D();
            _self = m;
        }

        public override Vector2D Calculate()
        {
            _wanderTarget = _wanderTarget + new Vector2D(_random.Next(-WanderJitter, WanderJitter), _random.Next(-WanderJitter, WanderJitter));
            _wanderTarget = _wanderTarget.Normalize();
            _wanderTarget *= WanderRadius / 2;

            _circleCenterG = new PointF(_self.Heading.X * WanderDistance + _self.Position.X - WanderRadius / 2, _self.Heading.Y * WanderDistance + _self.Position.Y - WanderRadius / 2);
            Vector2D circleCenterM = new Vector2D(_self.Heading.X * WanderDistance + _self.Position.X, _self.Heading.Y * WanderDistance + _self.Position.Y);
            _pointOnCircle = new Vector2D(circleCenterM.X + _wanderTarget.X, circleCenterM.Y + _wanderTarget.Y);

            return _pointOnCircle - _self.Position;
        }

        public override void RenderSB(Graphics g) {
            if (_pointOnCircle == null)
                return;

            g.DrawEllipse(Pens.LightGreen, new RectangleF(new PointF(_circleCenterG.X, _circleCenterG.Y), new SizeF(WanderRadius, WanderRadius)));
            g.FillEllipse(Brushes.Red, new RectangleF(new PointF(_pointOnCircle.X - 4, _pointOnCircle.Y - 4), new SizeF(8, 8)));
        }
    }
}