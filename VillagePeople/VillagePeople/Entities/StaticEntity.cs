using System.Collections.Generic;
using VillagePeople.Util;

namespace VillagePeople.Entities {
    public abstract class StaticEntity : BaseGameEntity {
        public int BaseAmount;
        public Resource GatherRate;
        public List<Vector2D> UnwalkableSpace;
        public bool Walkable = false;

        public StaticEntity(Vector2D position, World world) : base(position, world) { }

        public bool IsWalkable(Vector2D v) {
            return !(v.X >= UnwalkableSpace[0].X && v.X <= UnwalkableSpace[1].X && v.Y >= UnwalkableSpace[0].Y &&
                     v.Y <= UnwalkableSpace[1].Y);
        }
    }
}