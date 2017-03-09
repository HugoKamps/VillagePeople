using System.Drawing;
using VillagePeople.Util;

namespace VillagePeople.Entities.NPC
{
    class Villager : MovingEntity
    {
        public Color Color;

        public Villager(Vector2D position, World world) : base(position, world)
        {
            Velocity = new Vector2D(1, 1);
            Acceleration = new Vector2D(1, 1);
            targetSpeed = Velocity.Length();
            Scale = 20;
            MaxInventorySpace = 10;

            Color = Color.Black;
        }

        public override Resource AddResource(Resource r)
        {
            int capacity = MaxInventorySpace - Resource.TotalResources();

            if (capacity <=0)
            {
                return r;
            }

            if (capacity >= r.TotalResources())
            {
                return base.AddResource(r);
            } else
            {
                return base.AddResource(r.Cap(capacity));
            }
        }

        public override void Render(Graphics g)
        {
            double leftCorner = Position.X - Scale;
            double rightCorner = Position.Y - Scale;
            double size = Scale * 2;

            var p = new Pen(Color, 2);
            var b = new System.Drawing.SolidBrush(Color);

            g.FillEllipse(b, new Rectangle((int)leftCorner, (int)rightCorner, (int)size, (int)size));
            g.DrawLine(p, (int)Position.X, (int)Position.Y, (int)Position.X + (int)Velocity.X, (int)Position.Y + (int)Velocity.Y);
        }
    }
}
