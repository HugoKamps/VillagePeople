using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VillagePeople.Util;

namespace VillagePeople.Entities
{
    abstract class StaticEntity : BaseGameEntity
    {
        public bool Walkable = false;
        public Resource GatherRate;

        public StaticEntity(Vector2D position, World world) : base(position, world)
        {
        }
    }
}
