using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VillagePeople.Entities;
using VillagePeople.Util;

namespace VillagePeople.Behaviours
{
    class WanderBehaviour : SteeringBehaviour
    {
        private MovingEntity _self;

        public WanderBehaviour(MovingEntity m) : base(m)
        {
            _self = m;
        }

        public override Vector2D Calculate()
        {
            var me = _self.Position.Clone();
            throw new NotImplementedException();
        }
    }
}
