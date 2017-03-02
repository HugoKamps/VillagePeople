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
        public Color Color;

        public Villager(Vector2D position, World world) : base(position, world)
        {
            Velocity = new Vector2D(10, 10);
            Acceleration = new Vector2D(1, 1);
            targetSpeed = Velocity.Length();
            Scale = 20;

            Color = Color.Black;
        }

        public override void Render(Graphics g)
        {
            double leftCorner = Position.X - Scale;
            double rightCorner = Position.Y - Scale;
            double size = Scale * 2;

            Pen p = new Pen(Color, 2);
            try
            {
                g.DrawEllipse(p, new Rectangle((int)leftCorner, (int)rightCorner, (int)size, (int)size));
                g.DrawLine(p, (int)Position.X, (int)Position.Y, (int)Position.X + (int)Velocity.X, (int)Position.Y + (int)Velocity.Y);
            }
            catch (Exception) { }
        }
    }
}
