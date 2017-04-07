using System;
using System.Collections.Generic;
using System.Drawing;
using VillagePeople.Util;

namespace VillagePeople.Entities.Structures
{
    class GoldMine : StaticEntity
    {
        public GoldMine(Vector2D position, World world) : base(position, world)
        {
            Scale = 20;
            Resource.Gold = 200;
            GatherRate = new Resource() { Gold = 2 };
            UnwalkableSpace = new List<Vector2D>()
            {
                new Vector2D(position.X - Scale / 2 - 5, position.Y - Scale / 2 - 5), // Top Left
                new Vector2D(position.X + Scale / 2 + 5, position.Y + Scale / 2 + 5), // Bottom Right
            };
        }

        public void Gather(BaseGameEntity e)
        {
            if (Resource.Gold > GatherRate.Gold)
                Resource += e.AddResource(GatherRate);
            else
                Resource += e.AddResource(Resource);
        }

        public override void Update(float delta)
        {
            //if (Resource.Gold == 0)
            //    Walkable = false;
            //else
            //    Resource -= GatherRate;
        }

        public override void Render(Graphics g)
        {
            var b = new System.Drawing.SolidBrush(Color.Gold);

            if (Resource.Gold > 100)
            {
                double size = Scale;
                double leftCorner = Position.X - size / 2;
                double rightCorner = Position.Y - size / 2;
                g.FillRectangle(b, new Rectangle((int)leftCorner, (int)rightCorner, (int)size, (int)size));
            }
            else if (Resource.Gold > 0)
            {
                double size = Scale / 2;
                double leftCorner = Position.X - size / 2;
                double rightCorner = Position.Y - size / 2;
                g.FillRectangle(b, new Rectangle((int)leftCorner, (int)rightCorner, (int)size, (int)size));
            }
            else
            {
                b = new System.Drawing.SolidBrush(Color.Black);
                double size = Scale / 4;
                double leftCorner = Position.X - 5;
                double rightCorner = Position.Y - 5;
                g.FillRectangle(b, new Rectangle((int)leftCorner, (int)rightCorner, (int)10, (int)10));
            }

            g.DrawString(Resource.Gold.ToString(), new System.Drawing.Font("Arial", 9), new SolidBrush(Color.Black), Position.X, Position.Y);
        }
    }
}
