using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VillagePeople.Util;

namespace VillagePeople.Entities.NPC
{
    class Villager : MovingEntity
    {
        public Color color;
        public Villager(Vector2D position, World world) : base(position, world)
        {
        }
    }
}
