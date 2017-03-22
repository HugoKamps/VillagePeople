using System.Drawing;
using VillagePeople.Util;

namespace VillagePeople.Entities
{
    abstract class BaseGameEntity
    {
        public Vector2D Position { get; set; }
        public float Scale { get; set; }
        public World World { get; set; }
        public Resource Resource = new Resource();

        public BaseGameEntity(Vector2D position, World world)
        {
            Position = position;
            World = world;
        }

        public virtual Resource AddResource(Resource r)
        {
            Resource += r;
            return new Resource();
        }

        public abstract void Update(float delta);

        public virtual void Render(Graphics g)
        {
        }
    }
}
