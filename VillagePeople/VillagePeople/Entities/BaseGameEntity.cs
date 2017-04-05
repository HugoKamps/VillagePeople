using System.Drawing;
using VillagePeople.Util;

namespace VillagePeople.Entities
{
    public abstract class BaseGameEntity
    {
        public Vector2D Position { get; set; }
        public float Scale { get; set; }
        public World World { get; set; }
        public bool Tagged { get; set; }
        public Resource Resource = new Resource();

        public BaseGameEntity(Vector2D position, World world)
        {
            Position = position;
            World = world;
        }

        public virtual Resource AddResource(Resource r)
        {
            Resource += r;
            return new Resource() - r;
        }

        public abstract void Update(float delta);

        public virtual void Render(Graphics g)
        {
        }

        public void Tag() => Tagged = true;
        public void UnTag() => Tagged = false;

        public bool CloseEnough(Vector2D from, Vector2D to)
        {
            float x;
            float y;

            if (from.X < to.X) x = to.X - from.X;
            else x = from.X - to.X;

            if (from.Y < to.Y) y = to.Y - from.Y;
            else y = from.Y - to.Y;

            return x < 5 && y < 5;
        }

        public override string ToString()
        {
            return Position.ToString();
        }
    }
}
