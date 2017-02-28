using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VillagePeople.Util;

namespace VillagePeople.Entities
{
    abstract class MovingEntity : BaseGameEntity
    {
        public Vector2D Velocity { get; set; }
        public float Mass { get; set; }
        public float MaxSpeed { get; set; }

        public MovingEntity(Vector2D position, World world) : base(position, world)
        {
            Mass = 30;
            MaxSpeed = 150;
            Velocity = new Vector2D();
        }

        public override void Update(float timeElapsed)
        {
            // to do
            Console.WriteLine(ToString());
        }

        public override string ToString()
        {
            return $"{Velocity}";
        }
    }
}
