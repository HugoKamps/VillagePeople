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
            TargetSpeed = Velocity.Length();
            Scale = 5;

            Color = Color.Black;
        }

        public override void Render(Graphics g)
        {
            double leftCorner = Position.X - Scale;
            double rightCorner = Position.Y - Scale;
            double size = Scale * 2;

            var p = new Pen(Color, 2);
            var b = new SolidBrush(Color);

            g.FillEllipse(b, new Rectangle((int)leftCorner, (int)rightCorner, (int)size, (int)size));
            g.DrawLine(p, (int)Position.X, (int)Position.Y, (int)Position.X + (int)Velocity.X, (int)Position.Y + (int)Velocity.Y);
        }
    }
}
